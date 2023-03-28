using Catalog.Business.Models;
using Catalog.DataAccess.DTO;

namespace Catalog.Business.Mappers
{
    public static partial class Mappers
    {
        public static CategoryEntity ToBusiness(this CategoryDal category)
            => new()
            {
                Id = category.Id,
                Name = category.Name,
                Image = category.Image,
                ParentId = category.ParentId,
                Parent = category.Parent?.ToBusiness(),
            };

        public static CategoryDal ToDal(this CategoryEntity category)
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
