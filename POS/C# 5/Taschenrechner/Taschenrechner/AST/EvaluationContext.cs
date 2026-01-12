namespace Taschenrechner.AST;

/// <summary>
/// Kontext für die Auswertung von Ausdrücken.
/// Enthält die Werte der Variablen.
/// </summary>
public class EvaluationContext
{
    private readonly Dictionary<char, double> _variables = new();

    /// <summary>
    /// Setzt den Wert einer Variable.
    /// </summary>
    public void SetVariable(char name, double value)
    {
        _variables[name] = value;
    }

    /// <summary>
    /// Gibt den Wert einer Variable zurück.
    /// </summary>
    public double GetVariable(char name)
    {
        if (!_variables.TryGetValue(name, out var value))
        {
            throw new InvalidOperationException($"Variable '{name}' ist nicht definiert.");
        }
        return value;
    }

    /// <summary>
    /// Prüft, ob eine Variable definiert ist.
    /// </summary>
    public bool HasVariable(char name)
    {
        return _variables.ContainsKey(name);
    }
}
