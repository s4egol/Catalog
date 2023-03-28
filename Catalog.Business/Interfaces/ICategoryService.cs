using Catalog.Business.Models;

namespace Catalog.Business.Interfaces
{
    public interface ICategoryService
    {
        CategoryEntity Details(int id);
        void Update(CategoryEntity entity);
        void Add(CategoryEntity entity);
        void Delete(int id);
        IEnumerable<CategoryEntity> GetAll();
    }
}
