namespace CommandLineInterface;

internal interface IArgumentHandler
{
    ArgumentHandlerAcceptResponse Accept(string argument);
    ArgumentHandlerFinishResponse Finish();
}