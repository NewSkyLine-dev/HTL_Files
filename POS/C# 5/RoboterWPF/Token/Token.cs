namespace RoboterWPF.Token;

public enum TokenType
{
    Keyword,      // REPEAT, MOVE, COLLECT, UNTIL, IF
    Letter,       // General letters (A, B, C, etc.)
    Number,       // Numeric values
    Brackets,     // { }
    Direction,    // UP, DOWN, LEFT, RIGHT
    Condition,    // IS-A
    Target,       // OBSTACLE
    Newline,      // Line breaks
    Error         // Unknown tokens
}

public class Token(TokenType type = TokenType.Error, string text = "Missing Token", int line = 1, int column = 1)
{
    public string Text { get; set; } = text;
    public TokenType Type { get; set; } = type;
    public int Line { get; set; } = line;
    public int Column { get; set; } = column;

    public string TypeToString()
    {
        return Type switch
        {
            TokenType.Keyword => "Keyword",
            TokenType.Letter => "Letter",
            TokenType.Number => "Number",
            TokenType.Brackets => "Brackets",
            TokenType.Direction => "Direction",
            TokenType.Condition => "Condition",
            TokenType.Target => "Target",
            TokenType.Newline => "Newline",
            TokenType.Error => "Error",
            _ => "Unknown"
        };
    }

    public override string ToString()
    {
        return $"Line {Line} Col {Column} {TypeToString()}: {Text}";
    }
}
