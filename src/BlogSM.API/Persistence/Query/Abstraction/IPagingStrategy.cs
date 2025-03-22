using System;

namespace BlogSM.API.Persistence.Query.Abstraction;
public interface IPagingStrategy<T>
{
    IQueryable<T> Apply(IQueryable<T> query);
}
