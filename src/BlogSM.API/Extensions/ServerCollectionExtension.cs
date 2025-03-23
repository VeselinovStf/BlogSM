using System.Reflection;

using BlogSM.API.Domain;
using BlogSM.API.Persistence.Query.Abstraction;
using BlogSM.API.Persistence.Query.Filtering;
using BlogSM.API.Persistence.Query.Paging;
using BlogSM.API.Persistence.Query.Sorting;

namespace BlogSM.API.Extensions;

public static class ServerCollectionExtension
{
    public static void AddRepositories(this IServiceCollection services, Assembly assembly)
    {
        // Find all the repository types that end with "Repository"
        var repositoryTypes = assembly.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && t.Name.EndsWith("Repository"))
            .ToList();

        foreach (var implementationType in repositoryTypes)
        {
            // Find the corresponding interface for each repository
            var interfaceType = implementationType.GetInterfaces()
                .FirstOrDefault(i => i.Name == $"I{implementationType.Name}");

            if (interfaceType != null)
            {
                // Register the repository interface and its implementation
                services.AddScoped(interfaceType, implementationType);
            }
        }
    }

    public static void AddServices(this IServiceCollection services, Assembly assembly)
    {
        // Find all the service types that end with "Service"
        var serviceTypes = assembly.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && t.Name.EndsWith("Service"))
            .ToList();

        foreach (var implementationType in serviceTypes)
        {
            // Find the corresponding interface for each service
            var interfaceType = implementationType.GetInterfaces()
                .FirstOrDefault(i => i.Name == $"I{implementationType.Name}");

            if (interfaceType != null)
            {
                // Register the service interface and its implementation
                services.AddScoped(interfaceType, implementationType);
            }
        }
    }

    /// <summary>
    /// Not using reflection so far -
    /// Adding Query Stategies to service collection
    /// </summary>
    /// <param name="services">Collection of service descriptors</param>
    /// <param name="assembly">Assembly</param>
    public static void AddQueryStrategies(this IServiceCollection services, Assembly assembly)
    {
        services.AddScoped<ISortingStrategy<BlogPost>, SortBlogPostByTitleAscendingSortingStrategy>();
        services.AddScoped<ISortingStrategy<BlogPost>, SortBlogPostByTitleDescendingSortingStrategy>();
        services.AddScoped<ISortingStrategy<BlogPost>, SortBlogPostByDateAscendingSortingStrategy>();
        services.AddScoped<ISortingStrategy<BlogPost>, SortBlogPostByDateDescendingSortingStrategy>();
    }

    /// <summary>
    /// Not using reflection so far -
    /// Adding Query Stategies Factories to service collection
    /// </summary>
    /// <param name="services">Collection of service descriptors</param>
    /// <param name="assembly">Assembly</param>
    public static void AddQueryStrategyFactories(this IServiceCollection services, Assembly assembly)
    {
        services.AddScoped<ISortingStrategyFactory<BlogPost>, BlogPostSortingStrategyFactory>();
        services.AddScoped<IFilterBySearchFactory<BlogPost, FilterByBlogPostSearchDecorator>, FilterByBlogPostSearchFactory>();
        services.AddScoped<IFilteringStrategyFactory<BlogPost>, BlogPostFilteringStrategyFactory>();
        services.AddScoped<IPagingStrategyFactory<BlogPost>, BlogPostPagingStrategyFactory>();
    }
}
