using Microsoft.Extensions.DependencyInjection;

namespace CommandLineInterface.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCommandLineArguments<T>(this IServiceCollection services) => AddCommandLineArguments(services, typeof(T));

    public static IServiceCollection AddCommandLineArguments(this IServiceCollection services, Type commandLineArgumentsType)
    {
        throw new NotImplementedException();
    }
}