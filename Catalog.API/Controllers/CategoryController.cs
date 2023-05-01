using AutoMapper;
using Catalog.API.Models.Category;
using Catalog.Business.Interfaces;
using Catalog.Business.Models;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers
{
    [ApiController]
    [Route("api/category-management")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryService categoryService, IMapper mapper)
        {
            _categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public async Task<IEnumerable<CategoryViewModel>> GetAll()
        {
            var categories = (await _categoryService.GetAllAsync())
                .Select(_mapper.Map<CategoryViewModel>);

            return categories;
        }

        [HttpPost]
        public Task Add(CategoryContentViewModel categoryContent)
        {
            ArgumentNullException.ThrowIfNull(categoryContent, nameof(categoryContent));

            return _categoryService.AddAsync(_mapper.Map<CategoryEntity>(categoryContent));
        }

        [HttpPut]
        public Task Update(CategoryViewModel category)
        {
            ArgumentNullException.ThrowIfNull(category, nameof(category));

            return _categoryService.UpdateAsync(_mapper.Map<CategoryEntity>(category));
        }

        [HttpDelete]
        public Task Delete(int id) => _categoryService.DeleteAsync(id);
    }
}
