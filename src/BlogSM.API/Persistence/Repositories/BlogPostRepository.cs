using System;

using BlogSM.API.Domain;
using BlogSM.API.Persistence.Repositories.Abstraction;

using Microsoft.EntityFrameworkCore;

namespace BlogSM.API.Persistence.Repositories;

public class BlogPostRepository : Repository<BlogPost>, IBlogPostRepository
{
    public BlogPostRepository(BlogSMDbContext blogSMDbContext) : base(blogSMDbContext)
    {
    }

    public async Task<BlogPost?> GetPostWithCategoriesAndTagsAsync(Guid id)
    {
        return await _dbSet
            .Include(bp => bp.Categories) // TODO: Take only Id's
            .Include(bp => bp.Tags)
            .FirstOrDefaultAsync(p => p.Id == id);
    }
}
