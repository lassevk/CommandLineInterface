namespace CommandLineInterface;

internal class CommandLineArguments<T> : ICommandLineArguments<T>
{
    private readonly Lazy<T> _valueFactory;

    public CommandLineArguments(Func<T> valueFactory)
    {
        _valueFactory = new Lazy<T>(valueFactory ?? throw new ArgumentNullException(nameof(valueFactory)));
    }

    public T Value => _valueFactory.Value;
}