namespace CommandLineInterface;

public class CommandLineArgumentsBuilder
{
    private readonly HashSet<Type> _argumentTypes = new();

    public void AddInjectableType<T>() => AddInjectableType(typeof(T));

    public void AddInjectableType(Type type)
    {
        _argumentTypes.Add(type);
    }

    public CommandLineArguments Build(Func<Type, object>? factory = null)
    {
        factory ??= type => Activator.CreateInstance(type) ?? throw new InvalidOperationException($"Unable to create instance of type '{type.FullName}'");

        // TODO: Add command object, if present
        // TODO: Parse and handle all command line arguments, and inject property values

        throw new NotImplementedException();
    }
}