using CommandLineInterface;
using CommandLineInterface.Attributes;

namespace ConsoleSandbox;

public record ConsoleApplicationArguments
{
    [CommandLineInterfaceOption("bp1")]
    public bool? BooleanProperty1 { get; init; }

    [CommandLineInterfaceOption("i1")]
    public int Int32Property1 { get; init; }

    [CommandLineInterfaceOption("f1")]
    public float FloatProperty1 { get; init; }

    [CommandLineInterfaceOption("g1")]
    public Guid GuidProperty { get; init; }

    [CommandLineInterfacePositionalArgument(1)]
    [CommandLineInterfaceArgumentName("firstname")]
    public string StringProperty1 { get; init; } = "";

    [CommandLineInterfacePositionalArgument(2)]
    [CommandLineInterfaceArgumentName("lastname")]
    public string StringProperty2 { get; init; } = "";

    [CommandLineInterfaceRestArguments]
    public List<string> RestArguments { get; init; } = [];
}