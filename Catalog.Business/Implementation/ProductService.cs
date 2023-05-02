using AutoMapper;
using Catalog.Business.Exceptions;
using Catalog.Business.Interfaces;
using Catalog.Business.Models;
using Catalog.Business.Models.Queries;
using Catalog.DataAccess.Interfaces;
using Catalog.DataAccess.Models.Filters;
using ORM.Entities;

namespace Catalog.Business.Implementation
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task AddAsync(ProductEntity entity)
        {
            await ValidateAddedEntityAsync(entity);

            await _unitOfWork.ProductRepository.AddAsync(_mapper.Map<Product>(entity));
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

            await _unitOfWork.ProductRepository.UpdateAsync(_mapper.Map<Product>(entity));
            await _unitOfWork.CommitAsync();
        }

        public async Task<ProductEntity> GetAsync(int id)
        {
            var product = await _unitOfWork.ProductRepository.GetByIdAsync(id) ?? throw new KeyNotFoundException(nameof(id));

            return _mapper.Map<ProductEntity>(product);
        }

        public async Task<IEnumerable<ProductEntity>> GetAllAsync(ProductQueryEntity query)
        {
            var products = await _unitOfWork.ProductRepository
                .GetWithFiltrationAsync(_mapper.Map<ProductFilter>(query));

            return products.Select(_mapper.Map<ProductEntity>);
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
