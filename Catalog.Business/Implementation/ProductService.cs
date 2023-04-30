﻿using Catalog.Business.Configuration;
using Catalog.Business.Exceptions;
using Catalog.Business.Filters.Interfaces;
using Catalog.Business.Interfaces;
using Catalog.Business.Mappers;
using Catalog.Business.Models;
using Catalog.Business.Models.Queries;
using Catalog.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Business.Implementation
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductFilterBuilder _productFilterBuilder;
        private readonly IRabbitMqService _rabbitMqService;
        private readonly AppSettings _appSettings;

        public ProductService(IUnitOfWork unitOfWork,
            IRabbitMqService rabbitMqService,
            IProductFilterBuilder productFilterBuilder,
            AppSettings appSettings)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _rabbitMqService = rabbitMqService ?? throw new ArgumentNullException(nameof(rabbitMqService));
            _productFilterBuilder = productFilterBuilder ?? throw new ArgumentNullException(nameof(productFilterBuilder));
            _appSettings = appSettings ?? throw new ArgumentNullException(nameof(appSettings));
        }

        public async Task AddAsync(ProductEntity entity)
        {
            await ValidateAddedEntityAsync(entity);

            await _unitOfWork.ProductRepository.AddAsync(entity.ToDal());
            await _unitOfWork.CommitAsync();
        }

        public async Task DeleteAsync(int id)
        {
            await _unitOfWork.ProductRepository.DeleteAsync(id);
            await _unitOfWork.CommitAsync();
        }

        public async Task UpdateAsync(ProductEntity entity)
        {
            await ValidateUpdatedEntityAsync(entity);

            await _unitOfWork.ProductRepository.UpdateAsync(entity.ToDal());
            
            SendMessageInQueue(entity);

            await _unitOfWork.CommitAsync();
        }

        public async Task<ProductEntity> GetAsync(int id)
            => (await _unitOfWork.ProductRepository.GetByIdAsync(id))?.ToBusiness() ?? throw new KeyNotFoundException(nameof(id));

        public async Task<IEnumerable<ProductEntity>> GetAllAsync(ProductQueryEntity query)
        {
            var filter = _productFilterBuilder
                .WhereCategoryId(query.CategoryId)
                .Filter;

            return (await _unitOfWork.ProductRepository
                .GetAllQuery()
                .Where(filter)
                .Skip(query.Limit * (query.Page - 1))
                .Take(query.Limit)
                .ToArrayAsync())
                .Select(productDal => productDal.ToBusiness());
        }

        private async Task ValidateUpdatedEntityAsync(ProductEntity product)
        {
            ArgumentNullException.ThrowIfNull(product);

            if (!await _unitOfWork.ProductRepository.IsExistsAsync(product.Id))
            {
                throw new EntityNotFountException(nameof(product.Id));
            }

            if (product.CategoryId.HasValue && !await IsCategoryExistsAsync(product.CategoryId.Value))
            {
                throw new EntityNotFountException(nameof(product.CategoryId.Value));
            }
        } 

        private async Task ValidateAddedEntityAsync(ProductEntity product)
        {
            ArgumentNullException.ThrowIfNull(product);

            if (product.CategoryId.HasValue && !await IsCategoryExistsAsync(product.CategoryId.Value))
            {
                throw new EntityNotFountException(nameof(product.CategoryId.Value));
            }
        }

        private Task<bool> IsCategoryExistsAsync(int categoryId)
            => _unitOfWork.CategoryRepository.IsExistsAsync(categoryId);

        private void SendMessageInQueue(ProductEntity product)
        {
            try
            {
                _rabbitMqService.SendMessage(_appSettings.RabbitMqServerSettings.Queue, product.ToMessage());
            }
            catch(Exception ex)
            {
                throw new Exception("Rebbit Mq Server isn't available", ex);
            }
        }
    }
}