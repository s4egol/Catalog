using Catalog.Business.Interfaces;
using Catalog.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Controllers
{
    public class ProductController : Controller
    {
        private readonly ILogger<ProductController> _logger;
        private readonly ICatalogService _catalogService;

        public ProductController(
            ICatalogService catalogService,
            ILogger<ProductController> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _catalogService = catalogService ?? throw new ArgumentNullException(nameof(catalogService));
        }

        [HttpGet]
        public IActionResult Get(int id)
        {
            var product = _catalogService.Get(id).ToMvc();

            return View(product);
        }

        public IActionResult Index()
        {
            var products = _catalogService.GetAll()
                .Select(product => product.ToMvc())
                .ToArray();

            return View(products);
        }
    }
}
