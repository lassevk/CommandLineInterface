namespace CommandLineInterface;

public sealed class CommandLineArguments
{
    private readonly Dictionary<Type, object> _arguments;

    public CommandLineArguments(Dictionary<Type, object> arguments)
    {
        _arguments = arguments ?? throw new ArgumentNullException(nameof(arguments));
    }

    public T GetArguments<T>() => (T)GetArguments(typeof(T));

    public object GetArguments(Type type) => _arguments[type];
}