namespace CommandLineInterface.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class CommandLineInterfaceArgumentNameAttribute : Attribute
{
    public CommandLineInterfaceArgumentNameAttribute(string name)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
    }

    public string Name { get; }
}