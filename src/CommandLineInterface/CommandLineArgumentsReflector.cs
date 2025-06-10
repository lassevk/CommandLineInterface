using System.Numerics;
using System.Reflection;

using CommandLineInterface.Handlers;

namespace CommandLineInterface;

internal static class CommandLineArgumentsReflector
{
    private static readonly Dictionary<Type, Func<PropertyInfo, object, IArgumentHandler?>> _argumentHandlers = [];

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
        addNumberHandler<decimal>();
        addNumberHandler<double>();
        addNumberHandler<float>();

        addNumberHandler<BigInteger>();

        void addNumberHandler<T>()
            where T : struct, INumber<T>
        {
            _argumentHandlers.Add(typeof(T), NumericArgumentHandler<T>.Factory);
            _argumentHandlers.Add(typeof(T?), NumericArgumentHandler<T>.Factory);
        }
    }

    public static Dictionary<string, IArgumentHandler> Reflect(object instance)
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

            if (!_argumentHandlers.TryGetValue(property.PropertyType, out Func<PropertyInfo, object, IArgumentHandler?>? handlerFactory))
            {
                continue;
            }

            var optionAttributes = property.GetCustomAttributes<CommandLineOptionAttribute>().ToList();
            if (optionAttributes.Count == 0)
            {
                continue;
            }

            IArgumentHandler? handler = handlerFactory(property, instance);
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
}