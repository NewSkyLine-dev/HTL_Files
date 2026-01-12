using System.Text;

namespace CalculatorWPF.Token;

/// <summary>
/// Lexer/Tokenizer for calculator expressions.
/// Converts input string into a list of tokens.
/// </summary>
public class Tokenizer
{
    private readonly string _input;
    private int _position;
    private readonly List<Token> _tokens;

    public Tokenizer(string input)
    {
        _input = input;
        _position = 0;
        _tokens = new List<Token>();
    }

    private char Current => _position < _input.Length ? _input[_position] : '\0';
    private bool IsAtEnd => _position >= _input.Length;

    private void Advance()
    {
        _position++;
    }

    private char Peek(int offset = 1)
    {
        int pos = _position + offset;
        return pos < _input.Length ? _input[pos] : '\0';
    }

    public List<Token> Tokenize()
    {
        _tokens.Clear();
        _position = 0;

        while (!IsAtEnd)
        {
            // Skip whitespace
            if (char.IsWhiteSpace(Current))
            {
                Advance();
                continue;
            }

            // Try to match token
            Token? token = MatchToken();
            if (token != null)
            {
                _tokens.Add(token);
            }
        }

        // Add EOF token
        _tokens.Add(new Token(TokenType.EOF, "", _position));

        return _tokens;
    }

    private Token? MatchToken()
    {
        int startPos = _position;

        // Numbers (integers and decimals)
        if (char.IsDigit(Current))
        {
            return ReadNumber();
        }

        // Variables (single lowercase letters)
        if (char.IsLower(Current))
        {
            string varName = Current.ToString();
            Advance();
            return new Token(TokenType.Variable, varName, startPos);
        }

        // Operators and parentheses
        switch (Current)
        {
            case '+':
                Advance();
                return new Token(TokenType.Plus, "+", startPos);

            case '-':
                Advance();
                return new Token(TokenType.Minus, "-", startPos);

            case '*':
                Advance();
                return new Token(TokenType.Multiply, "*", startPos);

            case '/':
                Advance();
                return new Token(TokenType.Divide, "/", startPos);

            case '^':
                Advance();
                return new Token(TokenType.Power, "^", startPos);

            case '(':
                Advance();
                return new Token(TokenType.LeftParen, "(", startPos);

            case ')':
                Advance();
                return new Token(TokenType.RightParen, ")", startPos);

            case '=':
                Advance();
                return new Token(TokenType.Equals, "=", startPos);

            default:
                // Unknown character - create error token
                string errorChar = Current.ToString();
                Advance();
                return new Token(TokenType.Error, errorChar, startPos);
        }
    }

    private Token ReadNumber()
    {
        int startPos = _position;
        var sb = new StringBuilder();

        // Read integer part
        while (!IsAtEnd && char.IsDigit(Current))
        {
            sb.Append(Current);
            Advance();
        }

        // Check for decimal point
        if (!IsAtEnd && Current == '.' && char.IsDigit(Peek()))
        {
            sb.Append(Current);
            Advance();

            // Read decimal part
            while (!IsAtEnd && char.IsDigit(Current))
            {
                sb.Append(Current);
                Advance();
            }
        }

        string text = sb.ToString();
        double value = double.Parse(text, System.Globalization.CultureInfo.InvariantCulture);
        return new Token(TokenType.Number, text, startPos, value);
    }
}
