using CommandLineInterface.Handlers;
using CommandLineInterface.Internal;

namespace CommandLineInterface;

public class CommandLineArgumentsBuilder
{
    private readonly HashSet<Type> _commandLineArgumentsTypes = [];
    private readonly List<string[]> _arguments = [];

    public void AddCommandLineArgumentsType<T>()
        where T : class, new()
        => AddCommandLineArgumentsType(typeof(T));

    public void AddCommandLineArgumentsType(Type type)
    {
        _commandLineArgumentsTypes.Add(type);
    }

    public CommandLineArgumentsBuilder AddHostArguments() => AddArguments(Environment.GetCommandLineArgs().Skip(1).ToArray());

    public CommandLineArgumentsBuilder AddArguments(string[] arguments)
    {
        _arguments.Add(arguments ?? throw new ArgumentNullException(nameof(arguments)));
        return this;
    }

    public CommandLineArgumentsRepository Build()
    {
        List<object> instances = CreateCommandLineArgumentInstances();
        ParseAndInjectAllArguments(instances);

        return new CommandLineArgumentsRepository(instances);
    }

    private void ParseAndInjectAllArguments(List<object> instances)
    {
        Dictionary<string, IArgumentHandler> handlers = GetArgumentHandlers(instances);
        List<IArgumentHandler> positionalHandlers = GetPositionalArgumentHandlers(instances);

        var context = new CommandLineArgumentsContext(handlers, positionalHandlers);
        foreach (string argument in _arguments.SelectMany(x => x))
        {
            context.Process(argument);
        }

        context.Finish();

        if (context.Errors.Count > 0)
        {
            throw new CommandLineArgumentsBuilderException(context.Errors);
        }
    }

    private Dictionary<string, IArgumentHandler> GetArgumentHandlers(List<object> instances)
    {
        Dictionary<string, IArgumentHandler> handlers = [];
        List<PositionalArgumentHandler> positionalHandlers = [];

        foreach (object instance in instances)
        {
            Dictionary<string, IArgumentHandler> handlersForInstance = CommandLineArgumentsReflector.ReflectOptionProperties(instance);
            foreach (KeyValuePair<string, IArgumentHandler> kvp in handlersForInstance)
            {
                handlers.Add(kvp.Key, kvp.Value);
            }
        }

        return handlers;
    }

    private List<IArgumentHandler> GetPositionalArgumentHandlers(List<object> instances)
    {
        List<PositionalArgumentHandler> positionalHandlers = [];

        foreach (object instance in instances)
        {
            List<PositionalArgumentHandler> positionalHandlersForInstance = CommandLineArgumentsReflector.ReflectPositionalProperties(instance);
            positionalHandlers.AddRange(positionalHandlersForInstance);
        }

        positionalHandlers.Sort((x, y) => x.Position.CompareTo(y.Position));

        return positionalHandlers.Select(pah => pah.Handler).ToList();
    }

    private List<object> CreateCommandLineArgumentInstances()
        => _commandLineArgumentsTypes.Select(type => Activator.CreateInstance(type) ?? throw new InvalidOperationException($"Failed to create instance of type {type.FullName}")).ToList();
}