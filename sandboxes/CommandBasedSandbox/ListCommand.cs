using CommandLineInterface;
using CommandLineInterface.Extensions.Hosting;

namespace CommandBasedSandbox;

public class ListCommand : IConsoleApplication
{
    private readonly ICommandLineArguments<GlobalCommandLineArguments> _globalArguments;

    public ListCommand(ICommandLineArguments<GlobalCommandLineArguments> globalArguments)
    {
        _globalArguments = globalArguments ?? throw new ArgumentNullException(nameof(globalArguments));
    }

    public Task<int> RunAsync(CancellationToken stoppingToken)
    {
        Console.WriteLine("list");

        if (_globalArguments.Value.Verbose)
        {
            Console.WriteLine("verbose");
        }

        return Task.FromResult(0);
    }
}