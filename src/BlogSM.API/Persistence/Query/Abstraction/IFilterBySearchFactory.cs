namespace BlogSM.API.Persistence.Query.Abstraction;

public interface IFilterBySearchFactory<in T, out D>
{
    D Create(string search);
}
