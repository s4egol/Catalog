﻿using Catalog.Business.Exceptions;
using Catalog.Business.Interfaces;
using Catalog.Business.Mappers;
using Catalog.Business.Models;
using Catalog.Business.Models.Queries;
using Catalog.DataAccess.Interfaces;
using Catalog.DataAccess.Models.Filters;

namespace Catalog.Business.Implementation
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
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
            await _unitOfWork.CommitAsync();
        }

        public async Task<ProductEntity> GetAsync(int id)
            => (await _unitOfWork.ProductRepository.GetByIdAsync(id))?.ToBusiness() ?? throw new KeyNotFoundException(nameof(id));

        public async Task<IEnumerable<ProductEntity>> GetAllAsync(ProductQueryEntity query)
        {
            var products = await _unitOfWork.ProductRepository
                .GetWithFiltrationAsync(new ProductFilter
                {
                    Limit = query.Limit,
                    Page = query.Page,
                    CategoryId = query.CategoryId,
                });

            return products.Select(productDal => productDal.ToBusiness());
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
    }
}
