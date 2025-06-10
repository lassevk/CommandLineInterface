using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CommandLineInterface.Extensions.Hosting;

internal class RunConsoleApplicationWorker : BackgroundService
{
    private readonly IConsoleApplication _consoleApplication;
    private readonly ILogger<RunConsoleApplicationWorker> _logger;
    private readonly IHostApplicationLifetime _hostApplicationLifetime;

    public RunConsoleApplicationWorker(IConsoleApplication consoleApplication, ILogger<RunConsoleApplicationWorker> logger, IHostApplicationLifetime hostApplicationLifetime)
    {
        _consoleApplication = consoleApplication ?? throw new ArgumentNullException(nameof(consoleApplication));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _hostApplicationLifetime = hostApplicationLifetime ?? throw new ArgumentNullException(nameof(hostApplicationLifetime));
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _hostApplicationLifetime.ApplicationStarted.Register((_, _) => _ = ApplicationStarted(), null);
        return Task.CompletedTask;
    }

    private async Task ApplicationStarted()
    {
        _logger.LogDebug("Calling console application");
        try
        {

            int exitCode = await _consoleApplication.RunAsync(_hostApplicationLifetime.ApplicationStopping);
            Environment.ExitCode = exitCode;
        }
        catch (TaskCanceledException)
        {
            _logger.LogDebug("Console application was terminated by the user");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error running console application: {Message}", ex.Message);
            Environment.ExitCode = 1;
        }
        finally
        {
            _logger.LogDebug("Asking host system to terminate");
            _hostApplicationLifetime.StopApplication();
        }
    }
}