using Catalog.DataAccess.Interfaces;
using ORM.Context;

namespace Catalog.DataAccess.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CatalogContext _dbContext;
        private ICategoryRepository _categoryRepository;
        private IProductRepository _productRepository;

        public UnitOfWork(CatalogContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public ICategoryRepository CategoryRepository
        {
            get
            {
                if (_categoryRepository == null)
                {
                    _categoryRepository = new CategoryRepository(_dbContext);
                }

                return _categoryRepository;
            }
        }

        public IProductRepository ProductRepository
        {
            get
            {
                if (_productRepository == null)
                {
                    _productRepository = new ProductRepository(_dbContext);
                }

                return _productRepository;
            }
        }

        public void Commit() => _dbContext.SaveChanges();
    }
}
