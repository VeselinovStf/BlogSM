using System;

using BlogSM.API.Domain;
using BlogSM.API.Persistence.Query.Abstraction;

namespace BlogSM.API.Persistence.Query.Sorting;

public class BlogPostSortingStrategyFactory : ISortingStrategyFactory<BlogPost>
{
    private readonly Dictionary<(SortField, SortDirection), ISortingStrategy<BlogPost>> _strategies;

    public BlogPostSortingStrategyFactory(IEnumerable<ISortingStrategy<BlogPost>> strategies)
    {
        _strategies = strategies.ToDictionary(
            strategy => strategy switch
            {
                SortBlogPostByTitleAscendingSortingStrategy => (SortField.Title, SortDirection.Ascending),
                SortBlogPostByTitleDescendingSortingStrategy => (SortField.Title, SortDirection.Descending),
                SortBlogPostByDateAscendingSortingStrategy => (SortField.Date, SortDirection.Ascending),
                SortBlogPostByDateDescendingSortingStrategy => (SortField.Date, SortDirection.Descending),
                _ => throw new ArgumentException("Unsupported sorting strategy")
            },
            strategy => strategy
        );
    }

    public ISortingStrategy<BlogPost> GetSortingStrategy(SortField sortField, SortDirection sortDirection)
    {
        return _strategies.TryGetValue((sortField, sortDirection), out var strategy)
            ? strategy
            : throw new ArgumentException("Invalid sorting combination");
    }
}
