using System;

using BlogSM.API.Domain;
using BlogSM.API.Persistence.Repositories.Abstraction;

namespace BlogSM.API.Persistence.Repositories;

public class PostTargetRepository : Repository<PostTarget>, IPostTargetRepository
{
    public PostTargetRepository(BlogSMDbContext blogSMDbContext) : base(blogSMDbContext)
    {
    }
}
