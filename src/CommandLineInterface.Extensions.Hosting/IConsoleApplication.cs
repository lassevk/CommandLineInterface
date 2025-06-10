namespace CommandLineInterface.Extensions.Hosting;

public interface IConsoleApplication
{
    Task<int> RunAsync(CancellationToken stoppingToken);
}