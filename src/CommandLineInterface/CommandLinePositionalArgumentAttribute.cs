namespace CommandLineInterface;

[AttributeUsage(AttributeTargets.Property)]
public class CommandLinePositionalArgumentAttribute : Attribute
{
    public CommandLinePositionalArgumentAttribute(int position)
    {
        Position = position;
    }

    public int Position { get; }
}