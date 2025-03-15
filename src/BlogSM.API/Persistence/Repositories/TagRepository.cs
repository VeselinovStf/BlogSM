using System;

using BlogSM.API.Domain;
using BlogSM.API.Persistence.Repositories.Abstraction;

using Microsoft.EntityFrameworkCore;

namespace BlogSM.API.Persistence.Repositories;

public class TagRepository : Repository<Tag>, ITagRepository
{
    public TagRepository(BlogSMDbContext blogSMDbContext) : base(blogSMDbContext)
    {
    }

    public async Task<IEnumerable<Tag>> GetTagsByIdsAsync(IEnumerable<Guid> ids)
    {
        return await _dbSet.Where(t => ids.Contains(t.Id)).ToListAsync();
    }
}
