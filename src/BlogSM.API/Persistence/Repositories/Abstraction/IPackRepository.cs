using System;

using BlogSM.API.Domain;

namespace BlogSM.API.Persistence.Repositories.Abstraction;

public interface IPackRepository : IRepository<Pack>
{
    Task<IEnumerable<Pack>> GetAllByIdsAsync(IEnumerable<Guid> ids);
}
