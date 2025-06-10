using CommandLineInterface;
using CommandLineInterface.Extensions.Hosting;

namespace ConsoleSandboxDependencyInjection;

public class ConsoleApplication : IConsoleApplication
{
    private readonly ICommandLineArguments<ConsoleApplicationArguments> _arguments;

    public ConsoleApplication(ICommandLineArguments<ConsoleApplicationArguments> arguments)
    {
        _arguments = arguments ?? throw new ArgumentNullException(nameof(arguments));
    }

    public Task<int> RunAsync(CancellationToken stoppingToken)
    {
        Console.WriteLine("Hello World!");
        Console.WriteLine(_arguments.Value.ToString());
        return Task.FromResult(0);
    }
}