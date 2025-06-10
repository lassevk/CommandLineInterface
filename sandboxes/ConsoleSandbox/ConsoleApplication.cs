using CommandLineInterface.Extensions.Hosting;

namespace ConsoleSandbox;

public class ConsoleApplication : IConsoleApplication
{
    public async Task<int> RunAsync(CancellationToken stoppingToken)
    {
        Console.WriteLine("Hello World!");
        await Task.Delay(20000, stoppingToken);
        return 0;
    }
}