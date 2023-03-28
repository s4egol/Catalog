using Catalog.Business.Implementation;
using Catalog.Business.Interfaces;
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
            services.AddScoped<ICatalogService, CatalogService>();

            return services;
        }

        public static IServiceCollection ConfigureRepositories(this IServiceCollection services)
        {
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
