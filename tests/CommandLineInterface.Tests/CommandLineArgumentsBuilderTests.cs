namespace CommandLineInterface.Tests;

public class CommandLineArgumentsBuilderTests
{
    [Test]
    public void Build_WithBooleanPropertyArgument_SetsProperty()
    {
        var builder = new CommandLineArgumentsBuilder();
        builder.AddCommandLineArgumentsType<TestArguments>();
        builder.AddArguments(["-bp1"]);

        CommandLineArguments arguments = builder.Build();

        TestArguments args1 = arguments.GetArguments<TestArguments>();

        Assert.That(args1.BooleanProperty1, Is.True);
    }
}