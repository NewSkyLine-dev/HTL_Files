namespace CalculatorWPF.AST;

/// <summary>
/// Context class for the Interpreter Pattern.
/// Holds the variable values and provides operations for evaluation.
/// </summary>
public class EvaluationContext
{
    private readonly Dictionary<string, double> _variables = new();
    private readonly List<string> _errors = new();
    
    /// <summary>
    /// Callback function to request a variable value from the user.
    /// </summary>
    public Func<string, double?>? VariableRequestCallback { get; set; }

    /// <summary>
    /// Gets or sets a variable value.
    /// </summary>
    public double this[string name]
    {
        get => GetVariable(name);
        set => SetVariable(name, value);
    }

    /// <summary>
    /// Gets all defined variable names.
    /// </summary>
    public IEnumerable<string> VariableNames => _variables.Keys;

    /// <summary>
    /// Gets all variables as a dictionary.
    /// </summary>
    public IReadOnlyDictionary<string, double> Variables => _variables;

    /// <summary>
    /// Gets errors encountered during evaluation.
    /// </summary>
    public IReadOnlyList<string> Errors => _errors;

    /// <summary>
    /// Checks if there are any errors.
    /// </summary>
    public bool HasErrors => _errors.Count > 0;

    /// <summary>
    /// Sets the value of a variable.
    /// </summary>
    public void SetVariable(string name, double value)
    {
        _variables[name.ToLower()] = value;
    }

    /// <summary>
    /// Gets the value of a variable.
    /// If not defined, tries to request it via callback.
    /// Throws if the variable cannot be resolved.
    /// </summary>
    public double GetVariable(string name)
    {
        string key = name.ToLower();
        if (_variables.TryGetValue(key, out double value))
        {
            return value;
        }

        // Try to get the variable value from the user
        if (VariableRequestCallback != null)
        {
            double? requestedValue = VariableRequestCallback(name);
            if (requestedValue.HasValue)
            {
                _variables[key] = requestedValue.Value;
                return requestedValue.Value;
            }
        }

        string error = $"Undefined variable: '{name}'";
        _errors.Add(error);
        throw new InvalidOperationException(error);
    }

    /// <summary>
    /// Checks if a variable is defined.
    /// </summary>
    public bool IsDefined(string name)
    {
        return _variables.ContainsKey(name.ToLower());
    }

    /// <summary>
    /// Clears all variables and errors.
    /// </summary>
    public void Clear()
    {
        _variables.Clear();
        _errors.Clear();
    }

    /// <summary>
    /// Clears only the errors.
    /// </summary>
    public void ClearErrors()
    {
        _errors.Clear();
    }

    /// <summary>
    /// Adds an error message.
    /// </summary>
    public void AddError(string message)
    {
        _errors.Add(message);
    }
}
