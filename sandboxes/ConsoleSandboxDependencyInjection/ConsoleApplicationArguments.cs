using CommandLineInterface;

namespace ConsoleSandboxDependencyInjection;

public record ConsoleApplicationArguments
{
    [CommandLineOption("bp1")]
    public bool? BooleanProperty1 { get; init; }

    [CommandLineOption("i1")]
    public int Int32Property1 { get; init; }

    [CommandLineOption("f1")]
    public float FloatProperty1 { get; init; }

    [CommandLineOption("g1")]
    public Guid GuidProperty { get; init; }

    [CommandLinePositionalArgument(1)]
    [CommandLineArgumentName("firstname")]
    public string StringProperty1 { get; init; } = "";

    [CommandLinePositionalArgument(2)]
    [CommandLineArgumentName("lastname")]
    public string StringProperty2 { get; init; } = "";

    [CommandLineRestArguments]
    public List<string> RestArguments { get; init; } = [];
}