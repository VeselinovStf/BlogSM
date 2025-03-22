using BlogSM.API.Domain;
using BlogSM.API.Persistence.Query;

namespace BlogSM.API.Persistence.Repositories.Abstraction;

public interface IBlogPostRepository : IRepository<BlogPost>
{
    Task<BlogPost?> GetPostWithRelationIdsAsync(Guid id);
    Task<BlogPost?> GetPostWithIncludesAsync(Guid id);
    Task<IEnumerable<BlogPost>> GetAllAsync(QueryPipeline<BlogPost> queryPipeline);
}
