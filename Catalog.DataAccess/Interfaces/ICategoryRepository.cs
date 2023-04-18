using Catalog.DataAccess.DTO;

namespace Catalog.DataAccess.Interfaces
{
    public interface ICategoryRepository : IRepository<CategoryDal>
    {
        Task<bool> IsExistsAsync(int id);
        Task<CategoryDal[]> GetByIdAsync(int[] ids);
        Task<CategoryDal[]> GetChildrenAsync(int id);
    }
}
