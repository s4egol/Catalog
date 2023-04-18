using Catalog.API.Models.Product;
using Catalog.Business.Models;

namespace Catalog.API.Mappers
{
    public static partial class Mappers
    {
        public static ProductViewModel ToView(this ProductEntity product)
            => new()
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Image = product.Image,
                Price = product.Price,
                Amount = product.Amount,
                CategoryId = product.CategoryId,
                Category = product.Category?.ToView()
            };

        public static ProductEntity ToBusiness(this ProductViewModel product)
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
    }
}
