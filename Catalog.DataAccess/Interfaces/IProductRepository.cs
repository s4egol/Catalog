using Catalog.DataAccess.DTO;

namespace Catalog.DataAccess.Interfaces
{
    public interface IProductRepository : IRepository<ProductDal>
    {
        Task<ProductDal[]> GetByCategoryIdAsync(int categoryId);
        Task<bool> IsExistsAsync(int id);
        IQueryable<ProductDal> GetAllQuery();
    }
}
