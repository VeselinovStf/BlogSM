using BlogSM.API.Domain;

namespace BlogSM.API.Persistence.Repositories.Abstraction;

public interface ICategoryRepository : IRepository<Category>
{
    Task<IEnumerable<Category>> GetCategoriesByIdsAsync(IEnumerable<Guid> ids);
}
