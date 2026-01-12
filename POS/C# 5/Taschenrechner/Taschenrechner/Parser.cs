using Taschenrechner.AST;
using Taschenrechner.Token;

namespace Taschenrechner;

/// <summary>
/// Parser für mathematische Ausdrücke basierend auf der EBNF-Grammatik.
/// Verwendet rekursiven Abstieg (Recursive Descent Parser).
/// </summary>
public class Parser
{
    private readonly List<Token.Token> _tokens;
    private int _position = 0;
    private readonly List<ParseError> _errors = new();

    public Parser(List<Token.Token> tokens)
    {
        _tokens = tokens;
    }

    public IReadOnlyList<ParseError> Errors => _errors;
    public bool HasErrors => _errors.Count > 0;

    private Token.Token Current => _position < _tokens.Count
        ? _tokens[_position]
        : _tokens[^1];

    private Token.Token Peek(int offset = 0)
    {
        var index = _position + offset;
        return index < _tokens.Count ? _tokens[index] : _tokens[^1];
    }

    private Token.Token Advance()
    {
        var current = Current;
        if (_position < _tokens.Count - 1)
            _position++;
        return current;
    }

    private bool Check(TokenType type)
    {
        return Current.Type == type;
    }

    private bool Match(params TokenType[] types)
    {
        foreach (var type in types)
        {
            if (Check(type))
            {
                Advance();
                return true;
            }
        }
        return false;
    }

    private Token.Token Consume(TokenType type, string errorMessage)
    {
        if (Check(type))
            return Advance();

        _errors.Add(new ParseError(errorMessage, Current.Position));
        throw new ParseException(errorMessage, Current.Position);
    }

    /// <summary>
    /// Parst den gesamten Ausdruck.
    /// </summary>
    public IExpression Parse()
    {
        try
        {
            var expression = ParseExpression();

            if (!Check(TokenType.EndOfFile))
            {
                _errors.Add(new ParseError(
                    $"Unerwartetes Token '{Current.Value}' nach dem Ausdruck",
                    Current.Position
                ));
            }

            return expression;
        }
        catch (ParseException)
        {
            // Fehler wurde bereits zur Liste hinzugefügt
            throw;
        }
    }

    /// <summary>
    /// expression ::= term { ("+" | "-") term }
    /// </summary>
    private IExpression ParseExpression()
    {
        var left = ParseTerm();

        while (Check(TokenType.Plus) || Check(TokenType.Minus))
        {
            var op = Advance();
            var right = ParseTerm();
            var binaryOp = op.Type == TokenType.Plus
                ? BinaryOperator.Add
                : BinaryOperator.Subtract;
            left = new BinaryExpression(left, binaryOp, right);
        }

        return left;
    }

    /// <summary>
    /// term ::= power { ("*" | "/") power }
    /// </summary>
    private IExpression ParseTerm()
    {
        var left = ParsePower();

        while (Check(TokenType.Star) || Check(TokenType.Slash))
        {
            var op = Advance();
            var right = ParsePower();
            var binaryOp = op.Type == TokenType.Star
                ? BinaryOperator.Multiply
                : BinaryOperator.Divide;
            left = new BinaryExpression(left, binaryOp, right);
        }

        return left;
    }

    /// <summary>
    /// power ::= unary [ "^" power ]  (rechtsassoziativ)
    /// </summary>
    private IExpression ParsePower()
    {
        var left = ParseUnary();

        if (Check(TokenType.Caret))
        {
            Advance();
            var right = ParsePower(); // Rechtsassoziativ durch Rekursion
            return new BinaryExpression(left, BinaryOperator.Power, right);
        }

        return left;
    }

    /// <summary>
    /// unary ::= [ "-" ] factor
    /// </summary>
    private IExpression ParseUnary()
    {
        if (Check(TokenType.Minus))
        {
            Advance();
            var operand = ParseFactor();
            return new UnaryExpression(UnaryOperator.Negate, operand);
        }

        return ParseFactor();
    }

    /// <summary>
    /// factor ::= number | variable | "(" expression ")"
    /// </summary>
    private IExpression ParseFactor()
    {
        // Zahl
        if (Check(TokenType.Number))
        {
            var token = Advance();
            // Ersetze Komma durch Punkt für das Parsen
            var valueStr = token.Value.Replace(',', '.');
            if (double.TryParse(valueStr, System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.InvariantCulture, out var value))
            {
                return new NumberExpression(value);
            }
            _errors.Add(new ParseError($"Ungültige Zahl: {token.Value}", token.Position));
            throw new ParseException($"Ungültige Zahl: {token.Value}", token.Position);
        }

        // Variable
        if (Check(TokenType.Variable))
        {
            var token = Advance();
            return new VariableExpression(token.Value[0]);
        }

        // Geklammerter Ausdruck
        if (Check(TokenType.LeftParen))
        {
            Advance();
            var expression = ParseExpression();
            Consume(TokenType.RightParen, "Schließende Klammer ')' erwartet");
            return expression;
        }

        // Fehlerfall
        var errorToken = Current;
        var errorMsg = errorToken.Type == TokenType.EndOfFile
            ? "Unerwartetes Ende des Ausdrucks"
            : $"Unerwartetes Token '{errorToken.Value}'";

        _errors.Add(new ParseError(errorMsg, errorToken.Position));
        throw new ParseException(errorMsg, errorToken.Position);
    }
}

/// <summary>
/// Repräsentiert einen Parse-Fehler.
/// </summary>
public class ParseError
{
    public string Message { get; }
    public int Position { get; }

    public ParseError(string message, int position)
    {
        Message = message;
        Position = position;
    }
}

/// <summary>
/// Exception für Parse-Fehler.
/// </summary>
public class ParseException : Exception
{
    public int Position { get; }

    public ParseException(string message, int position) : base(message)
    {
        Position = position;
    }
}
