using Catalog.Business.Interfaces;
using Catalog.Mappers;
using Catalog.Models;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ILogger<CategoryController> _logger;
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService,
            ILogger<CategoryController> logger)
        {
            _categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        public IActionResult Index()
        {
            var categories = _categoryService.GetAll()
                .Select(category => category.ToMvc())
                .ToArray();

            return View(categories);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CategoryViewModel category)
        {
            _categoryService.Update(category?.ToBusiness());

            return RedirectToAction("Index", "Category");
        }

        public IActionResult Delete(int id)
        {
            _categoryService.Delete(id);

            return RedirectToAction("Index", "Category");
        }

        public IActionResult Details(int id)
        {
            var category = _categoryService.Details(id)
                .ToMvc();

            return View(category);
        }
    }
}
