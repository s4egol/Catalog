using Catalog.Business.Models;
using ORM.Entities;

namespace Catalog.Business.Mappers
{
    public static partial class Mappers
    {
        public static ProductEntity ToBusiness(this Product product)
            => new()
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Image = product.Image,
                Price = product.Price,
                Amount = product.Amount,
                CategoryId = product.CategoryId,
                Category = product.Category?.ToBusiness()
            };

        public static Product ToDal(this ProductEntity product)
            => new()
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Image = product.Image,
                Price = product.Price,
                Amount = product.Amount,
                CategoryId = product.CategoryId,
                Category = product.Category?.ToDal()
            };
    }
}
