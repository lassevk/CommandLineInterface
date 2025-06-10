using CommandLineInterface.Extensions.Hosting;

using ConsoleSandbox;

using Microsoft.Extensions.Hosting;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
builder.AddConsoleApplication<ConsoleApplication>();

IHost host = builder.Build();

await host.RunAsync();