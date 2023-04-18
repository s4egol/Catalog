using Catalog.DataAccess.DTO;
using ORM.Entities;
using System.Linq.Expressions;

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

        public static Expression<Func<Product, ProductDal>> ToDalExpression
            => product => new ProductDal
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
