using AutoMapper;
using Catalog.API.Models.Product;
using Catalog.API.Models.Product.Queries;
using Catalog.Business.Interfaces;
using Catalog.Business.Models;
using Catalog.Business.Models.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers
{
    [ApiController]
    [Route("api/product-management")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public ProductController(IProductService productService, IMapper mapper)
        {
            _productService = productService ?? throw new ArgumentNullException(nameof(productService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public async Task<IEnumerable<ProductViewModel>> GetAll([FromQuery] ProductQuery query)
        {
            ArgumentNullException.ThrowIfNull(query, nameof(query));

            var products = (await _productService.GetAllAsync(_mapper.Map<ProductQueryEntity>(query)))
                .Select(_mapper.Map<ProductViewModel>);

            return products;
        }

        [HttpPost]
        public Task Add(ProductContentViewModel productContent)
        {
            ArgumentNullException.ThrowIfNull(productContent, nameof(productContent));

            return _productService.AddAsync(_mapper.Map<ProductEntity>(productContent));
        }

        [HttpDelete]
        public Task Delete(int id) => _productService.DeleteAsync(id);

        [HttpPut]
        public Task Update(ProductViewModel product)
        {
            ArgumentNullException.ThrowIfNull(product, nameof(product));

            return _productService.UpdateAsync(_mapper.Map<ProductEntity>(product));
        }
    }
}
