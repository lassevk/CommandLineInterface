namespace CommandLineInterface.Handlers;

internal interface IArgumentHandler
{
    string Name { get; }

    ArgumentHandlerAcceptResponse Accept(string argument);
    ArgumentHandlerFinishResponse Finish();
}