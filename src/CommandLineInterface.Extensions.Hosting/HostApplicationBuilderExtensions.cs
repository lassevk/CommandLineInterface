using CommandLineInterface.Extensions.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

namespace CommandLineInterface.Extensions.Hosting;

public static class HostApplicationBuilderExtensions
{
    public static IHostApplicationBuilder AddConsoleApplication<T>(this IHostApplicationBuilder builder)
        where T : IConsoleApplication
        => AddConsoleApplication(builder, typeof(T));

    public static IHostApplicationBuilder AddConsoleApplication(this IHostApplicationBuilder builder, Type consoleApplicationType)
    {
        builder.Services.TryAddTransient(typeof(IConsoleApplication), consoleApplicationType);
        builder.Services.AddHostedService<RunConsoleApplicationWorker>();

        return builder;
    }

    public static IHostApplicationBuilder AddConsoleCommand<T>(this IHostApplicationBuilder builder, string commandName)
        where T : class, IConsoleApplication
    {
        if (builder.Services.FirstOrDefault(sd => sd.Lifetime == ServiceLifetime.Singleton && sd.ServiceType == typeof(CommandLineCommandsCollection))?.ImplementationInstance is not
            CommandLineCommandsCollection commandsCollection)
        {
            commandsCollection = new CommandLineCommandsCollection();
            builder.Services.AddSingleton(commandsCollection);

            builder.Services.AddHostedService<RunConsoleApplicationWorker>();
            builder.Services.AddCommandLineArguments<CommandLineCommandNameArguments>();
        }

        commandsCollection.Commands.Add(commandName, typeof(T));
        builder.Services.TryAddScoped<T>();

        return builder;
    }
}