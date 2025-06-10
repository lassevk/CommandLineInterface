namespace CommandLineInterface;

internal class CommandLineArgumentsContext
{
    private readonly Dictionary<string, IArgumentHandler> _optionHandlers;
    private readonly List<IArgumentHandler> _positionalHandlers;

    private IArgumentHandler? _currentHandler;
    private string? _currentOption;

    public CommandLineArgumentsContext(Dictionary<string, IArgumentHandler> optionHandlers, List<IArgumentHandler> positionalHandlers)
    {
        _optionHandlers = optionHandlers ?? throw new ArgumentNullException(nameof(optionHandlers));
        _positionalHandlers = positionalHandlers ?? throw new ArgumentNullException(nameof(positionalHandlers));
    }

    public List<string> Errors { get; } = [];

    public void Process(string argument)
    {
        if (argument.StartsWith('-') || argument.StartsWith('/'))
        {
            ProcessOption(argument);
        }
        else
        {
            ProcessValue(argument);
        }
    }

    private void ProcessOption(string option)
    {
        EndPreviousOption();

        string trimmedOption = option.TrimStart('-', '/');
        if (!_optionHandlers.TryGetValue(trimmedOption, out IArgumentHandler? handler))
        {
            Errors.Add($"No command line option {option} found");
            return;
        }

        _currentOption = option;
        _currentHandler = handler;
    }

    private void EndPreviousOption()
    {
        if (_currentHandler == null)
        {
            return;
        }

        switch (_currentHandler.Finish())
        {
            case ArgumentHandlerFinishResponse.Finished:
                _currentHandler = null;
                _currentOption = null;
                break;

            case ArgumentHandlerFinishResponse.MissingValue:
                Errors.Add($"Missing value for option {_currentOption}");
                break;
        }
    }

    private void ProcessValue(string value)
    {
        if (_currentHandler != null)
        {
            ProcessValueForOption(value);
            return;
        }

        if (_positionalHandlers.Count > 0)
        {
            ProcessValueWithFirstPositional(value);
            return;
        }

        Errors.Add($"Don't know what to do with command line argument {value}");
    }

    private void ProcessValueWithFirstPositional(string value)
    {
        switch (_positionalHandlers[0].Accept(value))
        {
            case ArgumentHandlerAcceptResponse.ContinueAccepting:
                break;

            case ArgumentHandlerAcceptResponse.Finished:
                _positionalHandlers.RemoveAt(0);
                break;

            case ArgumentHandlerAcceptResponse.InvalidValue:
                Errors.Add($"Invalid value for {_positionalHandlers[0].Name}: {value}");
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void ProcessValueForOption(string value)
    {
        Assume.That(_currentHandler != null);

        switch (_currentHandler.Accept(value))
        {
            case ArgumentHandlerAcceptResponse.ContinueAccepting:
                break;

            case ArgumentHandlerAcceptResponse.Finished:
                _currentHandler = null;
                _currentOption = null;
                break;

            case ArgumentHandlerAcceptResponse.InvalidValue:
                Errors.Add($"Invalid value for option {_currentOption}: {value}");
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void Finish()
    {
        EndPreviousOption();

        foreach (IArgumentHandler handler in _positionalHandlers)
        {
            if (handler.Finish() == ArgumentHandlerFinishResponse.MissingValue)
            {
                Errors.Add($"Missing value for argument {handler.Name}");
            }
        }
    }
}