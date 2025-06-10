using CommandLineInterface.Extensions.Hosting;

namespace ConsoleSandboxDependencyInjection;

public class ConsoleApplication : IConsoleApplication
{
    private readonly ConsoleApplicationArguments _arguments;

    public ConsoleApplication(ConsoleApplicationArguments arguments)
    {
        _arguments = arguments ?? throw new ArgumentNullException(nameof(arguments));
    }

    public Task<int> RunAsync(CancellationToken stoppingToken)
    {
        Console.WriteLine("Hello World!");
        Console.WriteLine(_arguments.ToString());
        return Task.FromResult(0);
    }
}