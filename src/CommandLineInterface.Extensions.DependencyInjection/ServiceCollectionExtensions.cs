using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CommandLineInterface.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCommandLineArguments<T>(this IServiceCollection services)
        where T : class
    {
        services.TryAddScoped<DependencyInjectionCommandLineArgumentsBuilder>();
        services.TryAddScoped<CommandLineArguments>(serviceProvider => serviceProvider.GetRequiredService<DependencyInjectionCommandLineArgumentsBuilder>().Build());
        services.TryAddScoped<T>(serviceProvider => serviceProvider.GetRequiredService<CommandLineArguments>().GetArguments<T>());
        services.TryAddSingleton(new CommandLineArgumentsType(typeof(T)));

        return services;
    }
}