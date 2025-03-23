using System;

using BlogSM.API.Persistence.Query.Filtering;

namespace BlogSM.API.Persistence.Query.Abstraction;

public interface IFilteringStrategyFactory<T> where T : class
{
    IFilteringStrategy<T> GetFilter(FilterType category, Guid value);
}
