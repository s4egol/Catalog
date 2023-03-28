using Catalog.DataAccess.DTO;
using ORM.Entities;

namespace Catalog.DataAccess.Mappers
{
    public static partial class Mappers
    {
        public static Product ToOrm(this ProductDal product)
            => new()
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Image = product.Image,
                Amount = product.Amount,
                Price = product.Price,
                CategoryId = product.CategoryId,
            };

        public static ProductDal ToDal(this Product product)
            => new()
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Image = product.Image,
                Amount = product.Amount,
                Price = product.Price,
                CategoryId = product.CategoryId,
                Category = product.Category?.ToDal()
            };
    }
}
