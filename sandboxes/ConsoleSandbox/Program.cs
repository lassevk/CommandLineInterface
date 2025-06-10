using CommandLineInterface;

using ConsoleSandbox;

var builder = new CommandLineArgumentsBuilder();
builder.AddArguments(args);
builder.AddCommandLineArgumentsType<ConsoleApplicationArguments>();

try
{
    CommandLineArguments arguments = builder.Build();
    var application = new ConsoleApplication(arguments.GetArguments<ConsoleApplicationArguments>());
    await application.RunAsync(CancellationToken.None);
}
catch (CommandLineArgumentsBuilderException ex)
{
    foreach (string message in ex.Messages)
    {
        Console.Error.WriteLine(message);
    }

    Environment.ExitCode = 1;
}