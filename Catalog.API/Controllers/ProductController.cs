using AutoMapper;
using Catalog.API.Mappers;
using Catalog.API.Models.Product;
using Catalog.API.Models.Product.Queries;
using Catalog.Business.Interfaces;
using Catalog.Business.Models.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers
{
    [ApiController]
    [Route("api/product-management")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService ?? throw new ArgumentNullException(nameof(productService));
        }

        [HttpGet]
        public async Task<IEnumerable<ProductViewModel>> GetAll([FromQuery] ProductQuery query)
        {
            ArgumentNullException.ThrowIfNull(query, nameof(query));

            return (await _productService.GetAllAsync(new ProductQueryEntity
            {
                CategoryId = query.CategoryId,
                Page = query.Page,
            }))
            .Select(product => product.ToView());
        }

        [HttpPost]
        public Task Add(ProductContentViewModel productContent)
        {
            ArgumentNullException.ThrowIfNull(productContent, nameof(productContent));

            var configuration = new MapperConfiguration(cfg => cfg.CreateMap<ProductContentViewModel, ProductViewModel>());
            var mapper = new Mapper(configuration);

            return _productService.AddAsync(mapper.Map<ProductViewModel>(productContent)?.ToBusiness());
        }

        [HttpDelete]
        public Task Delete(int id) => _productService.DeleteAsync(id);

        [HttpPut]
        public Task Update(ProductViewModel product)
        {
            ArgumentNullException.ThrowIfNull(product, nameof(product));

            return _productService.UpdateAsync(product.ToBusiness());
        }
    }
}
