using Microsoft.Extensions.DependencyInjection;

namespace CommandLineInterface.Extensions.DependencyInjection;

public static class CommandLineArgumentsBuilderExtensions
{
    public static CommandLineArguments Build(this CommandLineArgumentsBuilder builder, IServiceProvider services) => builder.Build(services.GetRequiredService);
}