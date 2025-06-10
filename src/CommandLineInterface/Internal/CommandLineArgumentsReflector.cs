using System.Numerics;
using System.Reflection;

using CommandLineInterface.Attributes;
using CommandLineInterface.Handlers;

namespace CommandLineInterface.Internal;

internal static class CommandLineArgumentsReflector
{
    private static readonly Dictionary<Type, Func<PropertyInfo, object, string, IArgumentHandler?>> _argumentHandlers = [];

    static CommandLineArgumentsReflector()
    {
        _argumentHandlers.Add(typeof(List<string>), StringListArgumentHandler.Factory);
        _argumentHandlers.Add(typeof(bool), BooleanArgumentHandler.Factory);
        _argumentHandlers.Add(typeof(bool?), BooleanArgumentHandler.Factory);

        _argumentHandlers.Add(typeof(string), StringArgumentHandler.Factory);
        _argumentHandlers.Add(typeof(Guid), GuidArgumentHandler.Factory);

        addNumberHandler<byte>();
        addNumberHandler<sbyte>();
        addNumberHandler<ushort>();
        addNumberHandler<short>();
        addNumberHandler<uint>();
        addNumberHandler<int>();
        addNumberHandler<ulong>();
        addNumberHandler<long>();
        addNumberHandler<UInt128>();
        addNumberHandler<Int128>();
        addNumberHandler<Half>();
        addNumberHandler<float>();
        addNumberHandler<double>();
        addNumberHandler<decimal>();

        addNumberHandler<BigInteger>();

        // DateTime?
        // DateOnly?
        // TimeOnly?
        // TimeSpan?
        // Char?

        void addNumberHandler<T>()
            where T : struct, INumber<T>
        {
            _argumentHandlers.Add(typeof(T), NumericArgumentHandler<T>.Factory);
            _argumentHandlers.Add(typeof(T?), NumericArgumentHandler<T>.Factory);
        }
    }

    private static IEnumerable<PropertyInfo> GetEligibleProperties(object instance)
    {
        Dictionary<string, IArgumentHandler> result = [];

        foreach (PropertyInfo property in instance.GetType().GetProperties())
        {
            if (property is not { CanRead: true })
            {
                continue;
            }

            if (property.GetIndexParameters() is not [])
            {
                continue;
            }

            yield return property;
        }
    }

    public static Dictionary<string, IArgumentHandler> ReflectOptionProperties(object instance)
    {
        Dictionary<string, IArgumentHandler> result = [];

        foreach (PropertyInfo property in GetEligibleProperties(instance))
        {
            if (!_argumentHandlers.TryGetValue(property.PropertyType, out Func<PropertyInfo, object, string, IArgumentHandler?>? handlerFactory))
            {
                continue;
            }

            var optionAttributes = property.GetCustomAttributes<CommandLineInterfaceOptionAttribute>().ToList();
            if (optionAttributes.Count == 0)
            {
                continue;
            }

            string name = property.GetCustomAttribute<CommandLineInterfaceArgumentNameAttribute>()?.Name ?? property.Name;

            IArgumentHandler? handler = handlerFactory(property, instance, name);
            if (handler is null)
            {
                continue;
            }

            foreach (string option in optionAttributes.Select(x => x.Option))
            {
                result.Add(option, handler);
            }
        }

        return result;
    }

    public static List<PositionalArgumentHandler> ReflectPositionalProperties(object instance)
    {
        List<PositionalArgumentHandler> result = [];

        foreach (PropertyInfo property in GetEligibleProperties(instance))
        {
            if (!_argumentHandlers.TryGetValue(property.PropertyType, out Func<PropertyInfo, object, string, IArgumentHandler?>? handlerFactory))
            {
                continue;
            }

            CommandLineInterfacePositionalArgumentAttribute? positionalAttribute = property.GetCustomAttribute<CommandLineInterfacePositionalArgumentAttribute>();
            CommandLineInterfaceRestArgumentsAttribute? restAttribute = property.GetCustomAttribute<CommandLineInterfaceRestArgumentsAttribute>();
            if (positionalAttribute == null && restAttribute == null)
            {
                continue;
            }

            string name = property.GetCustomAttribute<CommandLineInterfaceArgumentNameAttribute>()?.Name ?? property.Name;

            IArgumentHandler? handler = handlerFactory(property, instance, name);
            if (handler is null)
            {
                continue;
            }

            if (positionalAttribute != null)
            {
                result.Add(new PositionalArgumentHandler(positionalAttribute.Position, handler));
            }
            else if (restAttribute != null)
            {
                result.Add(new PositionalArgumentHandler(int.MaxValue, handler));
            }
        }

        return result;
    }
}