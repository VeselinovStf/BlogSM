using System;

using BlogSM.API.Domain;

namespace BlogSM.API.Persistence.Repositories.Abstraction;

public interface ITagRepository : IRepository<Tag>
{
    Task<IEnumerable<Tag>> GetTagsByIdsAsync(IEnumerable<Guid> ids);
}
