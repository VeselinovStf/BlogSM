using System;

using BlogSM.API.Domain;
using BlogSM.API.Persistence.Query.Abstraction;

namespace BlogSM.API.Persistence.Query.Filtering;

public class FilterByBlogPostTitleStrategy : IFilteringStrategy<BlogPost>
{
    private readonly string _search;

    public FilterByBlogPostTitleStrategy(string search)
    {
        _search = search;
    }
    public IQueryable<BlogPost> Apply(IQueryable<BlogPost> query)
    {
        return query.Where(p => p.Title.ToLower().Contains(_search));
    }
}
