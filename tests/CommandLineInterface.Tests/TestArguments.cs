using CommandLineInterface.Attributes;

namespace CommandLineInterface.Tests;

public class TestArguments
{
    [CommandLineInterfaceOption("bp1")]
    public bool BooleanProperty1 { get; set; }
}