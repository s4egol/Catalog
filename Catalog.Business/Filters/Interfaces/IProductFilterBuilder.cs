using Catalog.DataAccess.DTO;
using System.Linq.Expressions;

namespace Catalog.Business.Filters.Interfaces
{
    public interface IProductFilterBuilder
    {
        Expression<Func<ProductDal, bool>> Filter { get; }

        IProductFilterBuilder WhereCategoryId(int? categoryId);
    }
}
