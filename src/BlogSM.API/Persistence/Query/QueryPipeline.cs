using System;

using BlogSM.API.Persistence.Query.Abstraction;

namespace BlogSM.API.Persistence.Query;

public class QueryPipeline<T> where T : class
{
    private readonly List<IFilteringStrategy<T>> _filters = new();
    private readonly List<ISortingStrategy<T>> _sorters = new();
    private readonly List<IPagingStrategy<T>> _pagers = new();

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
        _pagers.Add(pager);

        return this;
    }

    public IQueryable<T> Apply(IQueryable<T> query)
    {
        foreach (var filter in _filters)
        {
            query = filter.Apply(query);
        }

        foreach (var sorter in _sorters)
        {
            query = sorter.Apply(query);
        }

        foreach (var pager in _pagers)
        {
            query = pager.Apply(query);
        }

        return query; 
    }
}
