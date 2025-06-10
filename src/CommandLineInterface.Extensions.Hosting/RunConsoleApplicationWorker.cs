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

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        IConsoleApplication? application = _services.GetService<IConsoleApplication>();
        if (application is not null)
        {
            _hostApplicationLifetime.ApplicationStarted.Register((_, _) => _ = ApplicationStarted(application), null);
            return Task.CompletedTask;
        }

        CommandLineCommandsCollection? commandsCollection = _services.GetService<CommandLineCommandsCollection>();
        if (commandsCollection is not null)
        {
            _hostApplicationLifetime.ApplicationStarted.Register((_, _) => _ = CommandStarted(commandsCollection), null);
        }

        return Task.CompletedTask;
    }

    private async Task CommandStarted(CommandLineCommandsCollection commandsCollection)
    {
        _logger.LogDebug("Calling console command");
        try
        {
            string? commandName = _services.GetRequiredService<ICommandLineArguments<CommandLineCommandNameArguments>>().Value.CommandName;
            if (commandName is null)
            {
                await Console.Error.WriteLineAsync("No command specified");
                Environment.ExitCode = 1;
                return;
            }

            _logger.LogDebug("Resolving console command {Name}", commandName);
            if (!commandsCollection.Commands.TryGetValue(commandName, out Type? commandType))
            {
                _logger.LogDebug("No command with the name {Name} has been registered", commandName);
                await Console.Error.WriteLineAsync($"No command with the name {commandName}");
                Environment.ExitCode = 1;
                return;
            }

            var instance = (IConsoleApplication)_services.GetRequiredService(commandType);

            int exitCode = await instance.RunAsync(_hostApplicationLifetime.ApplicationStopping);
            Environment.ExitCode = exitCode;
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

    private async Task ApplicationStarted(IConsoleApplication application)
    {
        _logger.LogDebug("Calling console application");
        try
        {

            int exitCode = await application.RunAsync(_hostApplicationLifetime.ApplicationStopping);
            Environment.ExitCode = exitCode;
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
}