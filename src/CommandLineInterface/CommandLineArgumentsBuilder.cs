using System.Reflection;

using CommandLineInterface.Handlers;

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

    public CommandLineArguments Build()
    {
        List<object> instances = CreateCommandLineArgumentInstances();
        ParseAndInjectAllArguments(instances);

        return new CommandLineArguments(instances);
    }

    private void ParseAndInjectAllArguments(List<object> instances)
    {
        Dictionary<string, IArgumentHandler> handlers = GetArgumentHandlers(instances);

        string? currentOption = null;
        IArgumentHandler? currentHandler = null;
        foreach (string argument in _arguments.SelectMany(x => x))
        {
            if (argument.StartsWith('-') || argument.StartsWith('/'))
            {
                if (currentHandler != null)
                {
                    switch (currentHandler.Finish())
                    {
                        case ArgumentHandlerFinishResponse.Finished:
                            currentHandler = null;
                            break;

                        case ArgumentHandlerFinishResponse.MissingValue:
                            throw new CommandLineOptionMissingValueException($"Missing value for option {currentOption}");
                    }
                    currentOption = null;
                }

                string option = argument.TrimStart('/', '-');
                if (handlers.TryGetValue(option, out IArgumentHandler? handler))
                {
                    currentHandler = handler;
                }
                else
                {
                    throw new InvalidOperationException($"No command line option {argument} found");
                }
            }
            else
            {
                if (currentHandler is null)
                {
                    throw new InvalidOperationException($"No command line option found for argument {argument}");
                }

                switch (currentHandler.Accept(argument))
                {
                    case ArgumentHandlerAcceptResponse.ContinueAccepting:
                        break;

                    case ArgumentHandlerAcceptResponse.Finished:
                        currentHandler = null;
                        break;

                    case ArgumentHandlerAcceptResponse.InvalidValue:
                        throw new CommandLineOptionInvalidValueException($"Invalid value for option {currentOption}: {argument}");

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        if (currentHandler != null)
        {
            switch (currentHandler.Finish())
            {
                case ArgumentHandlerFinishResponse.Finished:
                    break;

                case ArgumentHandlerFinishResponse.MissingValue:
                    throw new CommandLineOptionMissingValueException($"Missing value for option {currentOption}");
            }
        }
    }

    private Dictionary<string, IArgumentHandler> GetArgumentHandlers(List<object> instances)
    {
        Dictionary<string, IArgumentHandler> handlers = [];

        foreach (object instance in instances)
        {
            Dictionary<string, IArgumentHandler> handlersForInstance = CommandLineArgumentsReflector.Reflect(instance);
            foreach (KeyValuePair<string, IArgumentHandler> kvp in handlersForInstance)
            {
                handlers.Add(kvp.Key, kvp.Value);
            }
        }

        return handlers;
    }

    private List<object> CreateCommandLineArgumentInstances()
        => _commandLineArgumentsTypes.Select(type => Activator.CreateInstance(type) ?? throw new InvalidOperationException($"Failed to create instance of type {type.FullName}")).ToList();
}