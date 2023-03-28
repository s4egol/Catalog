using Catalog.Business.Interfaces;
using Catalog.Business.Mappers;
using Catalog.Business.Models;
using Catalog.DataAccess.Interfaces;

namespace Catalog.Business.Implementation
{
    public class CatalogService : ICatalogService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CatalogService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public void Add(ProductEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            _unitOfWork.ProductRepository.Add(entity.ToDal());
            _unitOfWork.Commit();
        }

        public void Delete(ProductEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            _unitOfWork.ProductRepository.Delete(entity.ToDal());
            _unitOfWork.Commit();
        }

        public void Update(ProductEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            _unitOfWork.ProductRepository.Update(entity.ToDal());
            _unitOfWork.Commit();
        }

        public ProductEntity Get(int id)
            => _unitOfWork.ProductRepository.GetById(id)?.ToBusiness() ?? throw new KeyNotFoundException(nameof(id));

        public IEnumerable<ProductEntity> GetAll()
            => _unitOfWork.ProductRepository
                .GetAll()
                .Select(productDal => productDal.ToBusiness());
    }
}
