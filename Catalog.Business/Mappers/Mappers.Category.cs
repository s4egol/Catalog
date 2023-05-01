using Catalog.Business.Models;
using ORM.Entities;

namespace Catalog.Business.Mappers
{
    public static partial class Mappers
    {
        public static CategoryEntity ToBusiness(this Category category)
            => new()
            {
                Id = category.Id,
                Name = category.Name,
                Image = category.Image,
                ParentId = category.ParentId,
                Parent = category.Parent?.ToBusiness(),
            };

        public static Category ToDal(this CategoryEntity category)
            => new()
            {
                Id = category.Id,
                Name = category.Name,
                Image = category.Image,
                ParentId = category.ParentId,
                Parent = category.Parent?.ToDal(),
            };
    }
}
