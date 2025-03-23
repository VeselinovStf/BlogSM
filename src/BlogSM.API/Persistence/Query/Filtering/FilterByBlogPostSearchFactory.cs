using BlogSM.API.Domain;
using BlogSM.API.Persistence.Query.Abstraction;

namespace BlogSM.API.Persistence.Query.Filtering;

public class FilterByBlogPostSearchFactory : IFilterBySearchFactory<BlogPost, FilterByBlogPostSearchDecorator>
{
    private readonly IList<Func<string, IFilteringStrategy<BlogPost>>> _strategies;

    public FilterByBlogPostSearchFactory(IList<Func<string, IFilteringStrategy<BlogPost>>> strategies)
    {
        _strategies = strategies;
    }
    public FilterByBlogPostSearchDecorator Create(string search)
    {
        var filters = _strategies.Select(factory => factory(search)).ToList();
        
        return new FilterByBlogPostSearchDecorator(filters);
    }
}
