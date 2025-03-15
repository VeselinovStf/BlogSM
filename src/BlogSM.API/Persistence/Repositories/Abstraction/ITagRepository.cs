using System;

using BlogSM.API.Domain;

namespace BlogSM.API.Persistence.Repositories.Abstraction;

public interface ITagRepository : IRepository<Tag>
{
    IQueryable<Tag> GetTagsByIds(IEnumerable<Guid> ids);
}
