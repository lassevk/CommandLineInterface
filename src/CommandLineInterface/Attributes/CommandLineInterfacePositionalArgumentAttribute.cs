namespace CommandLineInterface.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class CommandLineInterfacePositionalArgumentAttribute : Attribute
{
    public CommandLineInterfacePositionalArgumentAttribute(int position)
    {
        Position = position;
    }

    public int Position { get; }
}