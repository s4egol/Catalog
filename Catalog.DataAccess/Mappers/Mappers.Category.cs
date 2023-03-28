using Catalog.DataAccess.DTO;
using ORM.Entities;

namespace Catalog.DataAccess.Mappers
{
    public static partial class Mappers
    {
        public static Category ToOrm(this CategoryDal category)
            => new()
            {
                Id = category.Id,
                Name = category.Name,
                Image = category.Image,
                ParentId = category.ParentId,
            };

        public static CategoryDal ToDal(this Category category)
            => new()
            {
                Id = category.Id,
                Name = category.Name,
                Image = category.Image,
                ParentId = category.ParentId,
                Parent = category.Parent?.ToDal()
            };
    }
}
