using System;

using BlogSM.API.Domain;
using BlogSM.API.Persistence.Repositories.Abstraction;

namespace BlogSM.API.Persistence.Repositories;

public class TagRepository : Repository<Tag>, ITagRepository
{
    public TagRepository(BlogSMDbContext blogSMDbContext) : base(blogSMDbContext)
    {
    }

    public IQueryable<Tag> GetTagsByIds(IEnumerable<Guid> ids)
    {
        return _dbSet.Where(t => ids.Contains(t.Id));
    }
}
