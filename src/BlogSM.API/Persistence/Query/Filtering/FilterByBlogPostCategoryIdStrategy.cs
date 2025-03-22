using System;

using BlogSM.API.Domain;
using BlogSM.API.Persistence.Query.Abstraction;

namespace BlogSM.API.Persistence.Query.Filtering;

public class FilterByBlogPostCategoryIdStrategy : IFilteringStrategy<BlogPost>
{
    private readonly Guid _id;

    public FilterByBlogPostCategoryIdStrategy(Guid id)
    {
        _id = id;
    }
    public IQueryable<BlogPost> Apply(IQueryable<BlogPost> query)
    {
        return query.Where(p => p.Categories.Any(c => c.Id == _id));
    }
}
