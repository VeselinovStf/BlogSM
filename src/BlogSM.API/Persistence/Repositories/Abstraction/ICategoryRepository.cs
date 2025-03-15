using BlogSM.API.Domain;

namespace BlogSM.API.Persistence.Repositories.Abstraction;

public interface ICategoryRepository : IRepository<Category>
{
    IQueryable<Category> GetCategoriesByIds(IEnumerable<Guid> ids);
}
