using ORM.Entities;
using System.Linq.Expressions;

namespace Catalog.Business.Filters.Interfaces
{
    public interface IProductFilterBuilder
    {
        Expression<Func<Product, bool>> Filter { get; }

        IProductFilterBuilder WhereCategoryId(int? categoryId);
    }
}
