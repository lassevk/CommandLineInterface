namespace CommandLineInterface;

[AttributeUsage(AttributeTargets.Property)]
public sealed class CommandLineOptionAttribute : Attribute
{
    public CommandLineOptionAttribute(string option)
    {
        if (option.StartsWith('-') || option.StartsWith('/'))
        {
            throw new ArgumentException("Option attributes should exclude the leading - or / to denote a command line option", nameof(option));
        }

        Option = option;
    }

    public string Option { get; }
}