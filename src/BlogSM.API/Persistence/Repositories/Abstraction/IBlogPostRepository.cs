using System;

using BlogSM.API.Domain;

namespace BlogSM.API.Persistence.Repositories.Abstraction;

public interface IBlogPostRepository : IRepository<BlogPost>
{
    Task<BlogPost?> GetPostWithRelationIdsAsync(Guid id);

    Task<BlogPost?> GetPostWithIncludesAsync(Guid id);
}
