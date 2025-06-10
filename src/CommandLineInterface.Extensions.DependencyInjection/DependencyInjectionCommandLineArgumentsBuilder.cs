using Microsoft.Extensions.DependencyInjection;

namespace CommandLineInterface.Extensions.DependencyInjection;

internal class DependencyInjectionCommandLineArgumentsBuilder
{
    private readonly IEnumerable<CommandLineArgumentsType> _commandLineArgumentsTypes;
    private readonly IServiceProvider _serviceProvider;

    public DependencyInjectionCommandLineArgumentsBuilder(IEnumerable<CommandLineArgumentsType> commandLineArgumentsWrappers, IServiceProvider serviceProvider)
    {
        _commandLineArgumentsTypes = commandLineArgumentsWrappers ?? throw new ArgumentNullException(nameof(commandLineArgumentsWrappers));
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    }

    public CommandLineArguments Build()
    {
        var builder = new CommandLineArgumentsBuilder();
        builder.AddHostArguments();

        foreach (CommandLineArgumentsType commandLineArgumentsType in _commandLineArgumentsTypes)
        {
            builder.AddCommandLineArgumentsType(commandLineArgumentsType.Value);
        }

        return builder.Build();
    }
}