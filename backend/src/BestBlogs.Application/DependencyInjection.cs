using BestBlogs.Application.Common.CQRS;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BestBlogs.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Register CQRS dispatcher
        services.AddScoped<IDispatcher, Dispatcher>();

        // Register all command and query handlers from this assembly
        var assembly = Assembly.GetExecutingAssembly();

        // This will be used in Phase 3 when we add actual handlers
        // RegisterHandlers(services, assembly);

        // Register FluentValidation validators
        services.AddValidatorsFromAssembly(assembly);

        // Register AutoMapper profiles
        services.AddAutoMapper(assembly);

        return services;
    }

    // Uncomment when handlers are added in Phase 3
    // private static void RegisterHandlers(IServiceCollection services, Assembly assembly)
    // {
    //     // Register command handlers
    //     var commandHandlerTypes = assembly.GetTypes()
    //         .Where(t => t.GetInterfaces()
    //             .Any(i => i.IsGenericType &&
    //                 (i.GetGenericTypeDefinition() == typeof(ICommandHandler<>) ||
    //                  i.GetGenericTypeDefinition() == typeof(ICommandHandler<,>))))
    //         .ToList();
    //
    //     foreach (var handlerType in commandHandlerTypes)
    //     {
    //         var interfaceType = handlerType.GetInterfaces()
    //             .First(i => i.IsGenericType &&
    //                 (i.GetGenericTypeDefinition() == typeof(ICommandHandler<>) ||
    //                  i.GetGenericTypeDefinition() == typeof(ICommandHandler<,>)));
    //         services.AddScoped(interfaceType, handlerType);
    //     }
    //
    //     // Register query handlers
    //     var queryHandlerTypes = assembly.GetTypes()
    //         .Where(t => t.GetInterfaces()
    //             .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IQueryHandler<,>)))
    //         .ToList();
    //
    //     foreach (var handlerType in queryHandlerTypes)
    //     {
    //         var interfaceType = handlerType.GetInterfaces()
    //             .First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IQueryHandler<,>));
    //         services.AddScoped(interfaceType, handlerType);
    //     }
    // }
}
