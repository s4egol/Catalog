using AutoMapper;
using Catalog.API.Mappers;
using Catalog.API.Models.Category;
using Catalog.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers
{
    [ApiController]
    [Route("api/category-management")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));
        }

        [HttpGet]
        public async Task<IEnumerable<CategoryViewModel>> GetAll()
            => (await _categoryService.GetAllAsync())
                    .Select(category => category.ToView());

        [HttpPost]
        public Task Add(CategoryContentViewModel categoryContent)
        {
            ArgumentNullException.ThrowIfNull(categoryContent, nameof(categoryContent));

            var configuration = new MapperConfiguration(cfg => cfg.CreateMap<CategoryContentViewModel, CategoryViewModel>());
            var mapper = new Mapper(configuration);

            return _categoryService.AddAsync(mapper.Map<CategoryViewModel>(categoryContent)?.ToBusiness());
        }

        [HttpPut]
        public Task Update(CategoryViewModel category)
        {
            ArgumentNullException.ThrowIfNull(category, nameof(category));

            return _categoryService.UpdateAsync(category.ToBusiness());
        }

        [HttpDelete]
        public Task Delete(int id) => _categoryService.DeleteAsync(id);
    }
}
