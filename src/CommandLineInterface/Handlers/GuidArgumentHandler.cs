using System.Reflection;

namespace CommandLineInterface.Handlers;

internal class GuidArgumentHandler : IArgumentHandler
{
    private readonly PropertyInfo _property;
    private readonly object _instance;

    private bool _valueWasSet;

    private GuidArgumentHandler(PropertyInfo property, object instance)
    {
        _property = property ?? throw new ArgumentNullException(nameof(property));
        _instance = instance ?? throw new ArgumentNullException(nameof(instance));
    }

    public ArgumentHandlerAcceptResponse Accept(string argument)
    {
        if (!Guid.TryParse(argument, out Guid value))
        {
            return ArgumentHandlerAcceptResponse.InvalidValue;
        }

        _property.SetValue(_instance, value);
        _valueWasSet = true;
        return ArgumentHandlerAcceptResponse.Finished;
    }

    public ArgumentHandlerFinishResponse Finish() => !_valueWasSet ? ArgumentHandlerFinishResponse.MissingValue : ArgumentHandlerFinishResponse.Finished;

    public static IArgumentHandler Factory(PropertyInfo property, object instance) => new GuidArgumentHandler(property, instance);
}