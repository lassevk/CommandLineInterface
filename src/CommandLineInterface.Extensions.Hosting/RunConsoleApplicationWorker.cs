using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CommandLineInterface.Extensions.Hosting;

internal class RunConsoleApplicationWorker : BackgroundService
{
    private readonly ILogger<RunConsoleApplicationWorker> _logger;
    private readonly IHostApplicationLifetime _hostApplicationLifetime;
    private readonly IServiceProvider _services;

    public RunConsoleApplicationWorker(ILogger<RunConsoleApplicationWorker> logger, IHostApplicationLifetime hostApplicationLifetime, IServiceProvider services)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _hostApplicationLifetime = hostApplicationLifetime ?? throw new ArgumentNullException(nameof(hostApplicationLifetime));
        _services = services ?? throw new ArgumentNullException(nameof(services));
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            if (await TryRunConsoleApplicationAsync(stoppingToken))
            {
                return;
            }

            if (await TryRunConsoleCommandAsync(stoppingToken))
            {
                return;
            }

            throw new InvalidOperationException("No console application or console command has been registered");
        }
        catch (TaskCanceledException)
        {
            _logger.LogDebug("Console application was terminated by the user");
            throw;
        }
        catch (CommandLineArgumentsBuilderException ex)
        {
            _logger.LogDebug(ex, "Error running console application: {Message}", ex.Message);
            foreach (string message in ex.Messages)
            {
                await Console.Error.WriteLineAsync(message);
            }
            Environment.ExitCode = 1;
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

    private async Task<bool> TryRunConsoleCommandAsync(CancellationToken stoppingToken)
    {
        CommandLineCommandsCollection? commandsCollection = _services.GetService<CommandLineCommandsCollection>();
        if (commandsCollection is null)
        {
            return false;
        }

        string? commandName = _services.GetRequiredService<ICommandLineArguments<CommandLineCommandNameArguments>>().Value.CommandName;
        if (commandName is null)
        {
            await Console.Error.WriteLineAsync("No command specified");
            Environment.ExitCode = 1;
            return true;
        }

        _logger.LogDebug("Resolving console command {Name}", commandName);
        if (!commandsCollection.Commands.TryGetValue(commandName, out Type? commandType))
        {
            _logger.LogDebug("No command with the name {Name} has been registered", commandName);
            await Console.Error.WriteLineAsync($"No command with the name {commandName}");
            Environment.ExitCode = 1;
            return true;
        }

        var instance = (IConsoleApplication)_services.GetRequiredService(commandType);

        int exitCode = await instance.RunAsync(_hostApplicationLifetime.ApplicationStopping);
        Environment.ExitCode = exitCode;
        return true;
    }

    private async Task<bool> TryRunConsoleApplicationAsync(CancellationToken stoppingToken)
    {
        IConsoleApplication? application = _services.GetService<IConsoleApplication>();
        if (application is null)
        {
            return false;
        }

        Environment.ExitCode = await application.RunAsync(stoppingToken);
        return true;
    }
}