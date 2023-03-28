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

        public CategoryEntity Details(int id)
            => _unitOfWork.CategoryRepository.GetById(id)?.ToBusiness() ?? throw new KeyNotFoundException(nameof(id));

        public void Delete(int id)
        {
            var category = _unitOfWork.CategoryRepository.GetById(id);

            if (category == null)
            {
                throw new KeyNotFoundException(nameof(id));
            }

            _unitOfWork.CategoryRepository.Delete(category);
            _unitOfWork.Commit();
        }

        public IEnumerable<CategoryEntity> GetAll()
            => _unitOfWork.CategoryRepository
                .GetAll()
                .Select(categoryDal => categoryDal.ToBusiness())
                .ToList();

        public void Update(CategoryEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            _unitOfWork.CategoryRepository.Update(entity.ToDal());
            _unitOfWork.Commit();
        }

        public void Add(CategoryEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            _unitOfWork.CategoryRepository.Add(entity.ToDal());
            _unitOfWork.Commit();
        }
    }
}
