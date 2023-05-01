using ORM.Entities;

namespace Catalog.DataAccess.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<Product[]> GetByCategoryIdAsync(int categoryId);
        Task<bool> IsExistsAsync(int id);
        IQueryable<Product> GetAllQuery();
    }
}
