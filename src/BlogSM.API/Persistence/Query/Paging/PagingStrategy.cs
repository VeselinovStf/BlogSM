using BlogSM.API.Persistence.Query.Abstraction;

namespace BlogSM.API.Persistence.Query.Paging;

public class PagingStrategy<T> : IPagingStrategy<T>
{
    private readonly int _page;
    private readonly int _pageSize;

    public PagingStrategy(int page, int pageSize)
    {
        _page = page;
        _pageSize = pageSize;
    }
    public IQueryable<T> Apply(IQueryable<T> query)
    {
        return query.Skip((_page - 1) * _pageSize).Take(_pageSize);
    }
}
