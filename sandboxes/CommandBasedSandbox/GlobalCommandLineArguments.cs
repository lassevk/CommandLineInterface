using CommandLineInterface.Attributes;

namespace CommandBasedSandbox;

public class GlobalCommandLineArguments
{
    [CommandLineInterfaceOption("verbose")]
    [CommandLineInterfaceOption("v")]
    public bool Verbose { get; set; }
}