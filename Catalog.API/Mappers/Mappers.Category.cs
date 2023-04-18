using Catalog.API.Models.Category;
using Catalog.Business.Models;

namespace Catalog.API.Mappers
{
    public static partial class Mappers
    {
        public static CategoryViewModel ToView(this CategoryEntity category)
            => new()
            {
                Id = category.Id,
                Name = category.Name,
                Image = category.Image,
                ParentId = category.ParentId,
                Parent = category.Parent?.ToView()
            };

        public static CategoryEntity ToBusiness(this CategoryViewModel category)
            => new()
            {
                Id = category.Id,
                Name = category.Name,
                Image = category.Image,
                ParentId = category.ParentId,
                Parent = category.Parent?.ToBusiness()
            };
    }
}
