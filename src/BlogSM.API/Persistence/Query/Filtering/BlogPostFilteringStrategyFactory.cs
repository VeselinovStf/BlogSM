using BlogSM.API.Domain;
using BlogSM.API.Persistence.Query.Abstraction;

namespace BlogSM.API.Persistence.Query.Filtering;

public class BlogPostFilteringStrategyFactory : IFilteringStrategyFactory<BlogPost>
{
    private readonly Dictionary<FilterType, Func<Guid, IFilteringStrategy<BlogPost>>> _filterMappings;

    public BlogPostFilteringStrategyFactory()
    {
        _filterMappings = new Dictionary<FilterType, Func<Guid, IFilteringStrategy<BlogPost>>>
        {
            { FilterType.CategoryId, categoryId => new FilterByBlogPostCategoryIdStrategy(categoryId) },
            { FilterType.TagId, tagId => new FilterByBlogPostTagIdStrategy(tagId) },
            { FilterType.AuthorId, authorId => new FilterByBlogPostAuthorIdStrategy(authorId) }
        };
    }

    public IFilteringStrategy<BlogPost> GetFilter(FilterType category, Guid value)
    {
        return _filterMappings[category](value);  
    }
}
