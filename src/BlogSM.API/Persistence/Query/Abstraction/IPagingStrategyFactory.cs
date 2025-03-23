using System;

namespace BlogSM.API.Persistence.Query.Abstraction;

public interface IPagingStrategyFactory<T>
{
    IPagingStrategy<T> GetPager(int page, int pageSize);
}
