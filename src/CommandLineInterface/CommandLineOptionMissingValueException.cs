namespace CommandLineInterface;

public class CommandLineOptionMissingValueException : CommandLineOptionException
{
    public CommandLineOptionMissingValueException(string? message)
        : base(message) { }
}