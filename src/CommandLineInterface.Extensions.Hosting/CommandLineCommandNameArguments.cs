using CommandLineInterface.Attributes;

namespace CommandLineInterface.Extensions.Hosting;

internal class CommandLineCommandNameArguments
{
    [CommandLineInterfaceArgumentName("command")]
    [CommandLineInterfacePositionalArgument(int.MinValue)]
    public string? CommandName { get; set; }
}