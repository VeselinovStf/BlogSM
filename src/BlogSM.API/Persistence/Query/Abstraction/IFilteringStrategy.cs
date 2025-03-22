using System;

namespace BlogSM.API.Persistence.Query.Abstraction;

public interface IFilteringStrategy<T>
{
    IQueryable<T> Apply(IQueryable<T> query);
}
