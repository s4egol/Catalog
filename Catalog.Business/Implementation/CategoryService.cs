using Catalog.Business.Exceptions;
using Catalog.Business.Interfaces;
using Catalog.Business.Mappers;
using Catalog.Business.Models;
using Catalog.DataAccess.Interfaces;

namespace Catalog.Business.Implementation
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<CategoryEntity> GetByIdAsync(int id)
            => (await _unitOfWork.CategoryRepository.GetByIdAsync(id))?.ToBusiness() ?? throw new KeyNotFoundException(nameof(id));

        public async Task DeleteAsync(int id)
        {
            var category = (await _unitOfWork.CategoryRepository.GetByIdAsync(id))?.ToBusiness();

            if (category == null)
            {
                throw new EntityNotFountException(nameof(id));
            }

            await RemoveEntityReferences(category);
            await _unitOfWork.CategoryRepository.DeleteAsync(category.Id);
            await _unitOfWork.CommitAsync();
        }

        public async Task<IEnumerable<CategoryEntity>> GetAllAsync()
            => (await _unitOfWork.CategoryRepository
                .GetAllAsync())
                .Select(categoryDal => categoryDal.ToBusiness())
                .ToList();

        public async Task UpdateAsync(CategoryEntity entity)
        {
            await ValidateUpdatedEntityAsync(entity);

            await _unitOfWork.CategoryRepository.UpdateAsync(entity.ToDal());
            await _unitOfWork.CommitAsync();
        }

        public async Task AddAsync(CategoryEntity entity)
        {
            await ValidateAddedEntityAsync(entity);

            await _unitOfWork.CategoryRepository.AddAsync(entity.ToDal());
            await _unitOfWork.CommitAsync();
        }

        private async Task RemoveEntityReferences(CategoryEntity entity)
        {
            var children = await _unitOfWork.CategoryRepository.GetChildrenAsync(entity.Id);

            foreach (var child in children)
            {
                child.ParentId = default(int?);
                await _unitOfWork.CategoryRepository.UpdateAsync(child);
            }

            var products = await _unitOfWork.ProductRepository.GetByCategoryIdAsync(entity.Id);

            foreach(var product in products)
            {
                product.CategoryId = default(int?);
                await _unitOfWork.ProductRepository.UpdateAsync(product);
            }
        }

        private async Task ValidateAddedEntityAsync(CategoryEntity entity)
        {
            ArgumentNullException.ThrowIfNull(entity);

            if (entity.ParentId.HasValue)
            {
                await ValidateParentEntityAsync(entity);
            }
        }

        public async Task ValidateUpdatedEntityAsync(CategoryEntity entity)
        {
            ArgumentNullException.ThrowIfNull(entity);

            if (!await _unitOfWork.CategoryRepository.IsExistsAsync(entity.Id))
            {
                throw new EntityNotFountException(nameof(entity));
            }

            if (entity.ParentId.HasValue)
            {
                await ValidateParentEntityAsync(entity);
            }
        }

        public async Task ValidateParentEntityAsync(CategoryEntity entity)
        {
            if (entity.Id == entity.ParentId)
            {
                throw new Exception("An entity cannot be a parent to itself");
            }

            if (!await _unitOfWork.CategoryRepository.IsExistsAsync(entity.ParentId.Value))
            {
                throw new EntityNotFountException(nameof(entity.ParentId));
            }
        }
    }
}
