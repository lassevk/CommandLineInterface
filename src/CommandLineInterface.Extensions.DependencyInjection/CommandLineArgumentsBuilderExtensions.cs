using Microsoft.Extensions.DependencyInjection;

namespace CommandLineInterface.Extensions.DependencyInjection;

public static class CommandLineArgumentsBuilderExtensions
{
    public static CommandLineArguments Build(this CommandLineArgumentsBuilder builder, IServiceProvider services) => Build(builder, services, Environment.GetCommandLineArgs());
    public static CommandLineArguments Build(this CommandLineArgumentsBuilder builder, IServiceProvider services, ReadOnlySpan<string> arguments) => builder.Build(arguments, services.GetRequiredService);
}