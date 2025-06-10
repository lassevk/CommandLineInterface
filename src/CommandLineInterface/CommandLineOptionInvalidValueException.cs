namespace CommandLineInterface;

public class CommandLineOptionInvalidValueException : CommandLineOptionException
{
    public CommandLineOptionInvalidValueException(string? message)
        : base(message) { }
}