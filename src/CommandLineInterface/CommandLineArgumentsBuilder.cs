namespace CommandLineInterface;

public class CommandLineArgumentsBuilder
{
    private readonly HashSet<Type> _commandLineArgumentsTypes = new();
    private readonly List<string[]> _arguments = [];

    public void AddCommandLineArgumentsType<T>()
        where T : class, new()
        => AddCommandLineArgumentsType(typeof(T));

    public void AddCommandLineArgumentsType(Type type)
    {
        _commandLineArgumentsTypes.Add(type);
    }

    public CommandLineArgumentsBuilder AddHostArguments() => AddArguments(Environment.GetCommandLineArgs());

    public CommandLineArgumentsBuilder AddArguments(string[] arguments)
    {
        _arguments.Add(arguments ?? throw new ArgumentNullException(nameof(arguments)));
        return this;
    }

    public CommandLineArguments Build()
    {
        var result = new Dictionary<Type, object>();

        foreach (Type type in _commandLineArgumentsTypes)
        {
            object argumentInstance = Activator.CreateInstance(type) ?? throw new InvalidOperationException($"Failed to create instance of type {type.FullName}");
            result.Add(type, argumentInstance);
        }

        foreach (string argument in _arguments.SelectMany(s => s))
        {
            Console.WriteLine("argument: " + argument);
        }

        // TODO: Parse and handle all command line arguments, and inject property values

        return new CommandLineArguments(result);
    }
}