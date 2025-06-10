using CommandLineInterface.Extensions.DependencyInjection;
using CommandLineInterface.Extensions.Hosting;

using ConsoleSandboxDependencyInjection;

using Microsoft.Extensions.Hosting;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
builder.Services.AddCommandLineArguments<ConsoleApplicationArguments>();
builder.AddConsoleApplication<ConsoleApplication>();

IHost host = builder.Build();

await host.RunAsync();
