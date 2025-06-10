using System.Reflection;

namespace CommandLineInterface.Handlers;

internal class StringListArgumentHandler : IArgumentHandler
{
    private readonly List<string> _list;

    private StringListArgumentHandler(List<string> list)
    {
        _list = list;
    }

    public ArgumentHandlerAcceptResponse Accept(string argument)
    {
        _list.Add(argument);
        return ArgumentHandlerAcceptResponse.ContinueAccepting;
    }

    public ArgumentHandlerFinishResponse Finish() => ArgumentHandlerFinishResponse.Finished;

    public static IArgumentHandler Factory(PropertyInfo property, object instance) => new StringListArgumentHandler((List<string>)instance);
}