using System;

using BlogSM.API.Domain;
using BlogSM.API.Persistence.Query.Abstraction;

namespace BlogSM.API.Persistence.Query.Filtering;

public class FilterByBlogPostAuthorIdStrategy : IFilteringStrategy<BlogPost>
{
    private readonly Guid _id;

    public FilterByBlogPostAuthorIdStrategy(Guid id)
    {
        _id = id;
    }
    public IQueryable<BlogPost> Apply(IQueryable<BlogPost> query)
    {
        return query.Where(p => p.AuthorId == _id);
    }
}
