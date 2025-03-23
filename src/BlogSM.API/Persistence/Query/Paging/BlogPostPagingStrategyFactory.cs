using System;

using BlogSM.API.Domain;
using BlogSM.API.Persistence.Query.Abstraction;

namespace BlogSM.API.Persistence.Query.Paging;

public class BlogPostPagingStrategyFactory : IPagingStrategyFactory<BlogPost>
{
    private readonly IDictionary<PagingType, Func<int, int, PagingStrategy<BlogPost>>> _pagingStrategy;

    public BlogPostPagingStrategyFactory(IDictionary<PagingType, Func<int, int, PagingStrategy<BlogPost>>> pagingStrategy)
    {
        _pagingStrategy = pagingStrategy;
    }
    public IPagingStrategy<BlogPost> GetPager(int page, int pageSize, PagingType pagingType = PagingType.Default)
    {
        return _pagingStrategy[pagingType](page, pageSize);
    }
}
