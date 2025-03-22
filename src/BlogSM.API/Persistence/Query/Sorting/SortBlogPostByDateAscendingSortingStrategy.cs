using System;

using BlogSM.API.Domain;
using BlogSM.API.Persistence.Query.Abstraction;

namespace BlogSM.API.Persistence.Query.Sorting;

public class SortBlogPostByDateAscendingSortingStrategy : ISortingStrategy<BlogPost>
{
    public IQueryable<BlogPost> Apply(IQueryable<BlogPost> query)
    {
        return query.OrderBy(p => p.Date);
    }
}
