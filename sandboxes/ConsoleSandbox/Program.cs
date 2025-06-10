using CommandLineInterface;

using ConsoleSandbox;

var builder = new CommandLineArgumentsBuilder();
builder.AddArguments(args);
builder.AddCommandLineArgumentsType<ConsoleApplicationArguments>();
CommandLineArguments arguments = builder.Build();

var application = new ConsoleApplication(arguments.GetArguments<ConsoleApplicationArguments>());
try
{
    await application.RunAsync(CancellationToken.None);
}
catch (CommandLineOptionException ex)
{
    Console.Error.WriteLine(ex.Message);
    Environment.ExitCode = 1;
}