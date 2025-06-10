namespace CommandLineInterface;

[AttributeUsage(AttributeTargets.Property)]
public class CommandLineArgumentNameAttribute : Attribute
{
    public CommandLineArgumentNameAttribute(string name)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
    }

    public string Name { get; }
}