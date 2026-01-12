namespace CalculatorWPF.Token;

/// <summary>
/// Represents a single token in the calculator expression.
/// </summary>
public class Token
{
    public TokenType Type { get; set; }
    public string Text { get; set; }
    public int Position { get; set; }
    public double? NumericValue { get; set; }

    public Token(TokenType type, string text, int position, double? numericValue = null)
    {
        Type = type;
        Text = text;
        Position = position;
        NumericValue = numericValue;
    }

    public string TypeToString()
    {
        return Type switch
        {
            TokenType.Number => "Number",
            TokenType.Variable => "Variable",
            TokenType.Plus => "Plus",
            TokenType.Minus => "Minus",
            TokenType.Multiply => "Multiply",
            TokenType.Divide => "Divide",
            TokenType.Power => "Power",
            TokenType.LeftParen => "LeftParen",
            TokenType.RightParen => "RightParen",
            TokenType.Equals => "Equals",
            TokenType.EOF => "EOF",
            TokenType.Error => "Error",
            _ => "Unknown"
        };
    }

    public override string ToString()
    {
        return $"Position {Position} {TypeToString()}: '{Text}'";
    }
}
