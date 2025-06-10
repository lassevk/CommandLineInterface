namespace CommandLineInterface;

public interface ICommandLineArguments<out T>
{
    public T Value { get; }
}