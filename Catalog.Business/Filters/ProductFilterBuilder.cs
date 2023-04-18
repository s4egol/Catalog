using Catalog.Business.Filters.Interfaces;
using Catalog.DataAccess.DTO;
using System.Linq.Expressions;

namespace Catalog.Business.Filters
{
    public sealed class ProductFilterBuilder : IProductFilterBuilder
    {
        public Expression<Func<ProductDal, bool>> Filter { get; private set; } = product => true;

        public IProductFilterBuilder WhereCategoryId(int? categoryId)
        {
            if (categoryId.HasValue)
            {
                Expression<Func<ProductDal, bool>> hasCategory = product =>
                    product.CategoryId.HasValue && product.CategoryId.Value == categoryId.Value;

                var invokedExpr = Expression.Invoke(hasCategory, Filter.Parameters.Cast<Expression>());

                Filter = Expression.Lambda<Func<ProductDal, bool>>(
                    Expression.AndAlso(Filter.Body, invokedExpr),
                    Filter.Parameters);
            }

            return this;
        }
    }
}
