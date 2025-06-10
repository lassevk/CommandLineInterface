namespace CommandLineInterface;

public sealed class CommandLineArgumentsCollection
{
    private readonly Dictionary<Type, object> _arguments;

    public CommandLineArgumentsCollection(List<object> arguments)
    {
        _arguments = (arguments ?? throw new ArgumentNullException(nameof(arguments))).ToDictionary(x => x.GetType());
    }

    public ICommandLineArguments<T> GetArguments<T>() => new CommandLineArguments<T>(() => (T)_arguments[typeof(T)]);
}