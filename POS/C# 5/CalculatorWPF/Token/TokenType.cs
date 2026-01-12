namespace CalculatorWPF.Token;

/// <summary>
/// Defines all token types for the calculator lexer.
/// </summary>
public enum TokenType
{
    Number,         // Numeric literals (e.g., 42, 3.14)
    Variable,       // Single letter variables (a-z)
    Plus,           // +
    Minus,          // -
    Multiply,       // *
    Divide,         // /
    Power,          // ^
    LeftParen,      // (
    RightParen,     // )
    Equals,         // = (for assignment)
    EOF,            // End of input
    Error           // Invalid token
}
