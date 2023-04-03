using Catalog.DataAccess.DTO;
using Catalog.DataAccess.Interfaces;
using Catalog.DataAccess.Mappers;
using Microsoft.EntityFrameworkCore;
using ORM.Context;
using ORM.Entities;

namespace Catalog.DataAccess.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly CatalogContext _dbContext;

        public CategoryRepository(CatalogContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public void Add(CategoryDal entity)
        {
            if (entity == null)
            { 
                throw new ArgumentNullException(nameof(entity));
            }

            _dbContext.Categories.Add(entity.ToOrm());
        }

        public void Delete(CategoryDal entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            _dbContext.Categories
                .Where(category => category.Id == entity.Id)
                .ExecuteDelete();
        }

        public IEnumerable<CategoryDal> GetAll()
        {
            var categories = _dbContext.Categories
                .Include(category => category.Parent)
                .ToArray();

            return categories.Select(category => category.ToDal());
        }

        public CategoryDal GetById(int id)
            => GetDbEntityById(id)?.ToDal();

        public void Update(CategoryDal entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var category = GetDbEntityById(entity.Id);

            category.Name = entity.Name;
            category.Image = entity.Image;
            category.ParentId = entity.ParentId;

            _dbContext.Categories.Update(category);
        }

        private Category GetDbEntityById(int id)
            => _dbContext.Categories
                .Include(category => category.Parent)
                .SingleOrDefault(category => category.Id == id);
    }
}
