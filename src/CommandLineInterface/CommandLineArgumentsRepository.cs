namespace CommandLineInterface;

public sealed class CommandLineArgumentsRepository
{
    private readonly Dictionary<Type, object> _arguments;

    public CommandLineArgumentsRepository(List<object> arguments)
    {
        _arguments = (arguments ?? throw new ArgumentNullException(nameof(arguments))).ToDictionary(x => x.GetType());
    }

    public ICommandLineArguments<T> GetArguments<T>() => new CommandLineArguments<T>(() => (T)_arguments[typeof(T)]);
}