using System;

using BlogSM.API.Persistence.Query.Paging;

namespace BlogSM.API.Persistence.Query.Abstraction;

public interface IPagingStrategyFactory<T>
{
    IPagingStrategy<T> GetPager(int page, int pageSize, PagingType pagingType = PagingType.Default);
}
