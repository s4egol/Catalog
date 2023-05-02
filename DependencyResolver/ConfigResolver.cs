using Catalog.Business.Implementation;
using Catalog.Business.Interfaces;
using Catalog.DataAccess.Filters;
using Catalog.DataAccess.Filters.Interfaces;
using Catalog.DataAccess.Interfaces;
using Catalog.DataAccess.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace DependencyResolver
{
    public static class ConfigResolver
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services)
        {
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IProductService, ProductService>();

            return services;
        }

        public static IServiceCollection ConfigureRepositories(this IServiceCollection services)
        {
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }

        public static IServiceCollection ConfigureFilters(this IServiceCollection services)
        {
            services.AddScoped<IProductFilterBuilder, ProductFilterBuilder>();

            return services;
        }
    }
}
