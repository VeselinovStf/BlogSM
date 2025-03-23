using BlogSM.API.Domain;
using BlogSM.API.Persistence.Query.Abstraction;

namespace BlogSM.API.Persistence.Query.Sorting;

public class BlogPostSortingStrategyFactory : ISortingStrategyFactory<BlogPost>
{
    private readonly Dictionary<(SortField, SortDirection), ISortingStrategy<BlogPost>> _strategies;

    public BlogPostSortingStrategyFactory(Dictionary<(SortField, SortDirection), ISortingStrategy<BlogPost>> strategies)
    {
        _strategies = strategies;
    }

    public ISortingStrategy<BlogPost> GetSortingStrategy(SortField sortField, SortDirection sortDirection)
    {
        return _strategies.TryGetValue((sortField, sortDirection), out var strategy)
            ? strategy
            : throw new ArgumentException("Invalid sorting combination");
    }
}
