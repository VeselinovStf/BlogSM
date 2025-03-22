using System;

using BlogSM.API.Domain;
using BlogSM.API.Persistence.Query.Abstraction;

namespace BlogSM.API.Persistence.Query.Sorting;

public class SortBlogPostByTitleDescendingSortingStrategy : ISortingStrategy<BlogPost>
{
    public IQueryable<BlogPost> Apply(IQueryable<BlogPost> query)
    {
        return query.OrderByDescending(p => p.Title);
    }
}
