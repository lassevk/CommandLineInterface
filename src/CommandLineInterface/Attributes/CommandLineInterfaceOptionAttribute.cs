namespace CommandLineInterface.Attributes;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
public sealed class CommandLineInterfaceOptionAttribute : Attribute
{
    public CommandLineInterfaceOptionAttribute(string option)
    {
        if (option.StartsWith('-') || option.StartsWith('/'))
        {
            throw new ArgumentException("Option attributes should exclude the leading - or / to denote a command line option", nameof(option));
        }

        Option = option;
    }

    public string Option { get; }
}