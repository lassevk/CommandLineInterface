using CommandLineInterface;

namespace ConsoleSandbox;

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
}