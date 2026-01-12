namespace CalculatorWPF.Parser;

/// <summary>
/// Represents a parsing error with position information.
/// </summary>
public class ParseException : Exception
{
    public int Position { get; }

    public ParseException(string message, int position = 0)
        : base(message)
    {
        Position = position;
    }

    public override string ToString()
    {
        return $"Parse Error at position {Position}: {Message}";
    }
}
