namespace CommandLineInterface;

internal record struct PositionalArgumentHandler(int Position, IArgumentHandler Handler);