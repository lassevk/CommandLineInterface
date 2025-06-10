using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CommandLineInterface.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCommandLineArguments<T>(this IServiceCollection services)
        where T : class
    {
        services.TryAddScoped<DependencyInjectionCommandLineArgumentsBuilder>();
        services.TryAddScoped<CommandLineArgumentsRepository>(serviceProvider => serviceProvider.GetRequiredService<DependencyInjectionCommandLineArgumentsBuilder>().Build());
        services.TryAddScoped<ICommandLineArguments<T>>(serviceProvider
            => new CommandLineArguments<T>(() => serviceProvider.GetRequiredService<CommandLineArgumentsRepository>().GetArguments<T>().Value));

        services.AddSingleton(new CommandLineArgumentsType(typeof(T)));

        return services;
    }
}