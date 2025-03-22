using System;

using BlogSM.API.Persistence.Query.Abstraction;

namespace BlogSM.API.Persistence.Query;

public class QueryPipeline<T> where T : class
{
    private readonly List<IFilteringStrategy<T>> _filters = new();
    private readonly List<ISortingStrategy<T>> _sorters = new();
    private IPagingStrategy<T> _pager;

    public QueryPipeline<T> AddFilter(IFilteringStrategy<T> filter)
    {
        _filters.Add(filter);

        return this;
    }

    public QueryPipeline<T> AddSorter(ISortingStrategy<T> sort)
    {
        _sorters.Add(sort);

        return this;
    }

    public QueryPipeline<T> AddPaging(IPagingStrategy<T> pager)
    {
        _pager = pager;
        return this;
    }

    public IQueryable<T> Apply(IQueryable<T> query)
    {
        foreach (var filter in _filters)
        {
            filter.Apply(query);
        }

        foreach (var sorter in _sorters)
        {
            sorter.Apply(query);
        }

        return _pager != null ? _pager.Apply(query) : query; 
    }
}
