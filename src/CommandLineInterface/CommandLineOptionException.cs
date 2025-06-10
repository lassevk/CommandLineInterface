namespace CommandLineInterface;

public class CommandLineOptionException : InvalidOperationException
{
    public CommandLineOptionException(string? message)
        : base(message) { }

}