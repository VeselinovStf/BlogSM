using System;

using BlogSM.API.Domain;
using BlogSM.API.Persistence.Repositories.Abstraction;

namespace BlogSM.API.Persistence.Repositories;

public class PageTypeRepository : Repository<PageType>, IPageTypeRepository
{
    public PageTypeRepository(BlogSMDbContext blogSMDbContext) : base(blogSMDbContext)
    {
    }
}
