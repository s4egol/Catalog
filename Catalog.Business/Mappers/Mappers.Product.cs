using Catalog.Business.Models;
using Catalog.DataAccess.DTO;

namespace Catalog.Business.Mappers
{
    public static partial class Mappers
    {
        public static ProductEntity ToBusiness(this ProductDal product)
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

        public static ProductDal ToDal(this ProductEntity product)
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
