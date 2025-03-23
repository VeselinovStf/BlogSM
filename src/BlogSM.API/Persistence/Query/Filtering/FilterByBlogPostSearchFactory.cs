using System;

using BlogSM.API.Domain;
using BlogSM.API.Persistence.Query.Abstraction;

namespace BlogSM.API.Persistence.Query.Filtering;

public class FilterByBlogPostSearchFactory : IFilterBySearchFactory<BlogPost, FilterByBlogPostSearchDecorator>
{
    public FilterByBlogPostSearchDecorator Create(string search)
    {
        return new FilterByBlogPostSearchDecorator(new List<IFilteringStrategy<BlogPost>>(){
                    new FilterByBlogPostTitleStrategy(search),
                    new FilterByBlogPostContentStrategy(search)
        });
    }
}
