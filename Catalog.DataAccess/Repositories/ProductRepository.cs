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

        public async Task AddAsync(ProductDal entity)
        {
            ArgumentNullException.ThrowIfNull(entity);

            await _dbContext.Products.AddAsync(entity.ToOrm());
        }

        public async Task DeleteAsync(int entityId)
        {
            var product = await GetDbEntityByIdAsync(entityId);

            _dbContext.Products.Remove(product);
        }

        public async Task<IEnumerable<ProductDal>> GetAllAsync()
            => (await _dbContext.Products
                .ToArrayAsync())
                .Select(product => product.ToDal());

        public IQueryable<ProductDal> GetAllQuery()
            => _dbContext.Products
                .Select(Mappers.Mappers.ToDalExpression)
                .AsQueryable();

        public async Task<ProductDal[]> GetByCategoryIdAsync(int categoryId)
            => (await _dbContext.Products
                .Where(product => product.CategoryId.HasValue && product.CategoryId.Value == categoryId)
                .ToArrayAsync())
                .Select(product => product.ToDal())
                .ToArray();

        public async Task<ProductDal> GetByIdAsync(int id)
            => (await GetDbEntityByIdAsync(id))?.ToDal();

        public Task<bool> IsExistsAsync(int id) =>
            _dbContext.Products.AnyAsync(product => product.Id == id);

        public async Task UpdateAsync(ProductDal entity)
        {
            ArgumentNullException.ThrowIfNull(entity, nameof(entity));

            var product = await GetDbEntityByIdAsync(entity.Id);

            product.Name = entity.Name;
            product.Description = entity.Description;
            product.Image = entity.Image;
            product.Price = entity.Price;
            product.Amount = entity.Amount;
            product.CategoryId = entity.CategoryId;
        }

        private Task<Product> GetDbEntityByIdAsync(int id)
            => _dbContext.Products
                .SingleOrDefaultAsync(product => product.Id == id);
    }
}
