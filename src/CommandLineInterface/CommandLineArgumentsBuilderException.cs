namespace CommandLineInterface;

public class CommandLineArgumentsBuilderException : InvalidOperationException
{
    public CommandLineArgumentsBuilderException(List<string> messages)
        : base(messages.Count == 1 ? messages[0] : "There were errors when processing the command line arguments")
    {
        Messages = messages;
    }

    public List<string> Messages { get; }
}