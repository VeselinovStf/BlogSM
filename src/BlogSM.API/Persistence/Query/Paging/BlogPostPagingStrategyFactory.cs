using System;

using BlogSM.API.Domain;
using BlogSM.API.Persistence.Query.Abstraction;

namespace BlogSM.API.Persistence.Query.Paging;

public class BlogPostPagingStrategyFactory : IPagingStrategyFactory<BlogPost>
{
    public IPagingStrategy<BlogPost> GetPager(int page, int pageSize)
    {
        return new PagingStrategy<BlogPost>(page, pageSize);
    }
}
