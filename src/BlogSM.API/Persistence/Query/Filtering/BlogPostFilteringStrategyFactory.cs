using BlogSM.API.Domain;
using BlogSM.API.Persistence.Query.Abstraction;

namespace BlogSM.API.Persistence.Query.Filtering;

public class BlogPostFilteringStrategyFactory : IFilteringStrategyFactory<BlogPost>
{
    private readonly Dictionary<FilterType, Func<Guid, IFilteringStrategy<BlogPost>>> _filterMappings;

    public BlogPostFilteringStrategyFactory(Dictionary<FilterType, Func<Guid, IFilteringStrategy<BlogPost>>> filterMappings)
    {
        _filterMappings = filterMappings;
    }

    public IFilteringStrategy<BlogPost> GetFilter(FilterType category, Guid value)
    {
        return _filterMappings[category](value);  
    }
}
