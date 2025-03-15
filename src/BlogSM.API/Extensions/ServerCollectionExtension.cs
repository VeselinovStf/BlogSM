using System.Reflection;

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
}
