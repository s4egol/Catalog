using Catalog.Business.Models;

namespace Catalog.Business.Interfaces
{
    public interface ICatalogService
    {
        ProductEntity Get(int id);
        IEnumerable<ProductEntity> GetAll();
        void Add(ProductEntity entity);
        void Update(ProductEntity entity);
        void Delete(ProductEntity entity);
    }
}
