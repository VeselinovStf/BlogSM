using System;

using BlogSM.API.Persistence.Query.Sorting;

namespace BlogSM.API.Persistence.Query.Abstraction;

public interface ISortingStrategyFactory<T>
{
    ISortingStrategy<T> GetSortingStrategy(SortField sortField, SortDirection sortDirection);
}
