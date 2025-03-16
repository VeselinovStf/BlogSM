using BlogSM.API.Domain;
using BlogSM.API.Persistence.Repositories.Abstraction;

using Microsoft.EntityFrameworkCore;

namespace BlogSM.API.Persistence.Repositories;

public class PackRepository : Repository<Pack>, IPackRepository
{
    public PackRepository(BlogSMDbContext blogSMDbContext) : base(blogSMDbContext)
    {
    }

    public async Task<IEnumerable<Pack>> GetAllByIdsAsync(IEnumerable<Guid> ids)
    {
        return await _dbSet.Where(p => ids.Contains(p.Id)).ToListAsync();
    }
}
