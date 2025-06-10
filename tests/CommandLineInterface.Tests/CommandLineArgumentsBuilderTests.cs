namespace CommandLineInterface.Tests;

public class CommandLineArgumentsBuilderTests
{
    [Test]
    public void Build_WithBooleanPropertyArgument_SetsProperty()
    {
        var builder = new CommandLineArgumentsBuilder();
        builder.AddCommandLineArgumentsType<TestArguments>();
        builder.AddArguments(["-bp1"]);

        CommandLineArgumentsRepository argumentsRepository = builder.Build();

        TestArguments args1 = argumentsRepository.GetArguments<TestArguments>().Value;

        Assert.That(args1.BooleanProperty1, Is.True);
    }

    [Test]
    public void Build_WithBooleanPropertyWithValue_SetsPropertyToValue()
    {
        var builder = new CommandLineArgumentsBuilder();
        builder.AddCommandLineArgumentsType<TestArguments>();
        builder.AddArguments(["-bp1", "on"]);

        CommandLineArgumentsRepository argumentsRepository = builder.Build();

        TestArguments args1 = argumentsRepository.GetArguments<TestArguments>().Value;

        Assert.That(args1.BooleanProperty1, Is.True);
    }

    [Test]
    public void Build_WithBooleanPropertySetTwise_SetsPropertyToLastValue()
    {
        var builder = new CommandLineArgumentsBuilder();
        builder.AddCommandLineArgumentsType<TestArguments>();
        builder.AddArguments(["-bp1", "on", "-bp1", "0"]);

        CommandLineArgumentsRepository argumentsRepository = builder.Build();

        TestArguments args1 = argumentsRepository.GetArguments<TestArguments>().Value;

        Assert.That(args1.BooleanProperty1, Is.False);
    }
}