using System.Reflection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace WebApi.Features.Abstractions.Endpoints;

public static class EndpointExtensions
{
    public static IServiceCollection AddEndpoints(
        this IServiceCollection services,
        Assembly assembly
    )
    {
        ServiceDescriptor[] serviceDescriptors = assembly
            .DefinedTypes.Where(type =>
                type is { IsAbstract: false, IsInterface: false }
                && type.IsAssignableTo(typeof(IEndpoint))
            )
            .Select(type => ServiceDescriptor.Transient(typeof(IEndpoint), type))
            .ToArray();

        services.TryAddEnumerable(serviceDescriptors);

        return services;
    }

    public static IApplicationBuilder MapEndpoints(
        this WebApplication app,
        RouteGroupBuilder? routeGroupBuilder = null
    )
    {
        IEnumerable<IEndpoint> endpoints = app.Services.GetRequiredService<
            IEnumerable<IEndpoint>
        >();

        IEndpointRouteBuilder rootBuilder = routeGroupBuilder is null ? app : routeGroupBuilder;

        var groupCache = new Dictionary<Type, RouteGroupBuilder>();

        foreach (IEndpoint endpoint in endpoints)
        {
            Type? groupInterface = endpoint
                .GetType()
                .GetInterfaces()
                .FirstOrDefault(i =>
                    i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEndpoint<>)
                );

            if (groupInterface is null)
            {
                endpoint.MapEndpoint(rootBuilder);
                continue;
            }

            Type groupType = groupInterface.GetGenericArguments()[0];

            if (!groupCache.TryGetValue(groupType, out RouteGroupBuilder? groupBuilder))
            {
                var group = (IEndpointGroup)Activator.CreateInstance(groupType)!;
                groupBuilder = group.Map(rootBuilder);
                groupCache[groupType] = groupBuilder;
            }

            endpoint.MapEndpoint(groupBuilder);
        }

        return app;
    }
}
