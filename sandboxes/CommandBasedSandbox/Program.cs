using CommandBasedSandbox;

using CommandLineInterface.Extensions.DependencyInjection;
using CommandLineInterface.Extensions.Hosting;

using Microsoft.Extensions.Hosting;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
builder.Services.AddCommandLineArguments<GlobalCommandLineArguments>();

builder.AddConsoleCommand<ListCommand>("list");

IHost host = builder.Build();

await host.RunAsync();