using System;

using BlogSM.API.Domain;
using BlogSM.API.Persistence.Repositories.Abstraction;

using Microsoft.EntityFrameworkCore;

namespace BlogSM.API.Persistence.Repositories;

public class CategoryRepository : Repository<Category>, ICategoryRepository
{

    public CategoryRepository(BlogSMDbContext blogSMDbContext) : base(blogSMDbContext)
    {
    }

    public IQueryable<Category> GetCategoriesByIds(IEnumerable<Guid> ids)
    {
        return _dbSet.Where(c => ids.Contains(c.Id));
    }
}
