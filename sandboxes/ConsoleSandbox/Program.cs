using CommandLineInterface;

using ConsoleSandbox;

var builder = new CommandLineArgumentsBuilder();
builder.AddArguments(args);
builder.AddCommandLineArgumentsType<ConsoleApplicationArguments>();
CommandLineArguments arguments = builder.Build();

var application = new ConsoleApplication(arguments.GetArguments<ConsoleApplicationArguments>());
await application.RunAsync(CancellationToken.None);