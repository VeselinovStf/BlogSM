using System;

using BlogSM.API.Domain;
using BlogSM.API.Persistence.Query.Abstraction;

namespace BlogSM.API.Persistence.Query.Filtering;

public class FilterByBlogPostSearchDecorator : IFilteringStrategy<BlogPost>
{
    private readonly IEnumerable<IFilteringStrategy<BlogPost>> _filteringStrategies;

    public FilterByBlogPostSearchDecorator(IEnumerable<IFilteringStrategy<BlogPost>> filteringStrategies)
    {
        _filteringStrategies = filteringStrategies;
    }

    public IQueryable<BlogPost> Apply(IQueryable<BlogPost> query)
    {
        return _filteringStrategies
            .Select(filter => filter.Apply(query))
            .Aggregate((current, next) => current.Union(next)); // Combine results
    }
}
