namespace CommandLineInterface.Handlers;

internal record struct PositionalArgumentHandler(int Position, IArgumentHandler Handler);