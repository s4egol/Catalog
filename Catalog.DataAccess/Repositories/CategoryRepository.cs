﻿using Catalog.DataAccess.DTO;
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

        public async Task AddAsync(CategoryDal entity)
        {
            ArgumentNullException.ThrowIfNull(entity);

            await _dbContext.Categories.AddAsync(entity.ToOrm());
        }

        public async Task DeleteAsync(int entityId)
        {
            var category = await _dbContext.Categories
                .Where(category => category.Id == entityId)
                .SingleAsync();

            _dbContext.Categories.Remove(category);
        }

        public async Task<IEnumerable<CategoryDal>> GetAllAsync()
        {
            var categories = await _dbContext.Categories
                .Include(category => category.Parent)
                .ToArrayAsync();

            return categories.Select(category => category.ToDal());
        }

        public async Task<CategoryDal> GetByIdAsync(int id)
            => (await GetEntitiesByIdsQuery(new[] { id }).SingleOrDefaultAsync())?.ToDal();

        public async Task<CategoryDal[]> GetByIdAsync(int[] ids)
            => (await GetEntitiesByIdsQuery(ids)
                .ToArrayAsync())
                .Select(category => category.ToDal())
                .ToArray();

        public async Task<CategoryDal[]> GetChildrenAsync(int id)
            => (await _dbContext.Categories
                .Where(category => category.ParentId.HasValue && category.ParentId.Value == id)
                .ToArrayAsync())
                .Select(category => category.ToDal())
                .ToArray();

        public Task<bool> IsExistsAsync(int id)
            => _dbContext.Categories.AnyAsync(category => category.Id == id);

        public async Task UpdateAsync(CategoryDal entity)
        {
            ArgumentNullException.ThrowIfNull(entity, nameof(entity));

            var category = await GetEntitiesByIdsQuery(new[] { entity.Id })
                .SingleOrDefaultAsync();

            category.Name = entity.Name;
            category.Image = entity.Image;
            category.ParentId = entity.ParentId;

            _dbContext.Categories.Update(category);
        }

        private IQueryable<Category> GetEntitiesByIdsQuery(int[] ids)
            => _dbContext.Categories
                .Include(category => category.Parent)
                .Where(category => ids.Contains(category.Id));
    }
}
