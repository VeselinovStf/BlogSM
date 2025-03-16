using System;

using BlogSM.API.Domain;
using BlogSM.API.Persistence.Repositories.Abstraction;

namespace BlogSM.API.Persistence.Repositories;

public class LayoutRepository : Repository<Layout>, ILayoutRepository
{
    public LayoutRepository(BlogSMDbContext blogSMDbContext) : base(blogSMDbContext)
    {
    }
}
