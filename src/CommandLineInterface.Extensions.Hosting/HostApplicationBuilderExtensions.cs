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
}