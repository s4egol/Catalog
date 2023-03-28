using Catalog.DataAccess.DTO;
using Catalog.DataAccess.Interfaces;
using Catalog.DataAccess.Mappers;
using Microsoft.EntityFrameworkCore;
using ORM.Context;
using ORM.Entities;

namespace Catalog.DataAccess.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly CatalogContext _dbContext;

        public ProductRepository(CatalogContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public void Add(ProductDal entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            _dbContext.Products.Add(entity.ToOrm());
        }

        public void Delete(ProductDal entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            _dbContext.Products
                .Where(product => product.Id == entity.Id)
                .ExecuteDelete();
        }

        public IEnumerable<ProductDal> GetAll()
            => _dbContext.Products
                .ToArray()
                .Select(product => product.ToDal());

        public ProductDal GetById(int id)
            => GetDbEntityById(id)?.ToDal();

        public void Update(ProductDal entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var product = GetDbEntityById(entity.Id);

            product.Name = entity.Name;
            product.Description = entity.Description;
            product.Image = entity.Image;
            product.Price = entity.Price;
            product.Amount = entity.Amount;
            product.CategoryId = entity.CategoryId;
        }

        private Product GetDbEntityById(int id)
            => _dbContext.Products.SingleOrDefault(product => product.Id == id);
    }
}
