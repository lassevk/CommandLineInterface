using System.Reflection;

namespace CommandLineInterface.Handlers;

internal class BooleanArgumentHandler : IArgumentHandler
{
    private readonly PropertyInfo _property;
    private readonly object _instance;

    private bool _valueWasSet;

    private BooleanArgumentHandler(PropertyInfo property, object instance)
    {
        _property = property ?? throw new ArgumentNullException(nameof(property));
        _instance = instance ?? throw new ArgumentNullException(nameof(instance));
    }

    public ArgumentHandlerAcceptResponse Accept(string argument)
    {
        bool? value = argument.ToLowerInvariant() switch
        {
            "yes"  => true,
            "1"    => true,
            "on"   => true,
            "true" => true,

            "no"    => false,
            "0"     => false,
            "off"   => false,
            "false" => false,

            _ => null,
        };

        if (value is null)
        {
            return ArgumentHandlerAcceptResponse.InvalidValue;
        }

        _property.SetValue(_instance, value);
        _valueWasSet = true;
        return ArgumentHandlerAcceptResponse.Finished;
    }

    public ArgumentHandlerFinishResponse Finish()
    {
        if (!_valueWasSet)
        {
            _property.SetValue(_instance, true);
        }

        return ArgumentHandlerFinishResponse.Finished;
    }

    public static IArgumentHandler Factory(PropertyInfo property, object instance) => new BooleanArgumentHandler(property, instance);
}