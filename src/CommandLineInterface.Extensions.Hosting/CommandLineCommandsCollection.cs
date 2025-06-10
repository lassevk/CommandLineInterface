using CommandLineInterface.Extensions.DependencyInjection;

namespace CommandLineInterface.Extensions.Hosting;

public class CommandLineCommandsCollection
{
    public Dictionary<string, Type> Commands { get; } = new();
}