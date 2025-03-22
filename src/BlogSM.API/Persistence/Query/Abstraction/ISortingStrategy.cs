using System;

namespace BlogSM.API.Persistence.Query.Abstraction;

public interface ISortingStrategy<T>
{
    IQueryable<T> Apply(IQueryable<T> query);
}
