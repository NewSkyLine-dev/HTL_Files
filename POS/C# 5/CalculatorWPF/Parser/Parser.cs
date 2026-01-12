using CalculatorWPF.AST;
using CalculatorWPF.Token;

namespace CalculatorWPF.Parser;

/// <summary>
/// Recursive descent parser for calculator expressions.
/// Implements the grammar defined in ABNF.txt.
/// 
/// Grammar (operator precedence from lowest to highest):
/// statement  = assignment / expression
/// assignment = variable "=" expression
/// expression = term *( ("+" / "-") term )
/// term       = power *( ("*" / "/") power )
/// power      = unary ["^" power]         ; right-associative
/// unary      = ["-"] factor
/// factor     = number / variable / "(" expression ")"
/// </summary>
public class Parser
{
    private readonly List<Token.Token> _tokens;
    private int _currentIndex;
    private readonly List<ParseException> _errors;

    public Parser(List<Token.Token> tokens)
    {
        _tokens = tokens;
        _currentIndex = 0;
        _errors = new List<ParseException>();
    }

    public List<ParseException> GetErrors() => _errors;
    public bool HasErrors => _errors.Count > 0;

    /// <summary>
    /// Parses the tokens and returns the AST root.
    /// </summary>
    public Expression Parse()
    {
        _errors.Clear();
        _currentIndex = 0;

        try
        {
            Expression result = ParseStatement();

            // Check for unconsumed tokens
            if (!IsAtEnd() && Current().Type != TokenType.EOF)
            {
                AddError($"Unexpected token: '{Current().Text}'");
            }

            return result;
        }
        catch (ParseException ex)
        {
            _errors.Add(ex);
            throw;
        }
    }

    #region Token Access

    private Token.Token Current()
    {
        if (_currentIndex >= _tokens.Count)
        {
            return new Token.Token(TokenType.EOF, "", _tokens.Count > 0 ? _tokens[^1].Position : 0);
        }
        return _tokens[_currentIndex];
    }

    private Token.Token Peek(int offset = 1)
    {
        int index = _currentIndex + offset;
        if (index >= _tokens.Count)
        {
            return new Token.Token(TokenType.EOF, "", _tokens.Count > 0 ? _tokens[^1].Position : 0);
        }
        return _tokens[index];
    }

    private bool IsAtEnd() => _currentIndex >= _tokens.Count || Current().Type == TokenType.EOF;

    private void Advance()
    {
        if (!IsAtEnd())
        {
            _currentIndex++;
        }
    }

    private bool Check(TokenType type)
    {
        if (IsAtEnd()) return false;
        return Current().Type == type;
    }

    private bool Match(TokenType type)
    {
        if (Check(type))
        {
            Advance();
            return true;
        }
        return false;
    }

    private Token.Token Expect(TokenType type, string message)
    {
        if (!Check(type))
        {
            throw new ParseException(message, Current().Position);
        }
        var token = Current();
        Advance();
        return token;
    }

    private void AddError(string message)
    {
        _errors.Add(new ParseException(message, Current().Position));
    }

    #endregion

    #region Grammar Rules

    /// <summary>
    /// statement = assignment / expression
    /// assignment = variable "=" expression
    /// </summary>
    private Expression ParseStatement()
    {
        // Check if this is an assignment (variable followed by =)
        if (Check(TokenType.Variable) && Peek().Type == TokenType.Equals)
        {
            return ParseAssignment();
        }

        return ParseExpression();
    }

    /// <summary>
    /// assignment = variable "=" expression
    /// </summary>
    private Expression ParseAssignment()
    {
        var varToken = Current();
        Advance(); // consume variable
        Advance(); // consume =

        Expression value = ParseExpression();
        return new AssignmentExpression(varToken.Text, value);
    }

    /// <summary>
    /// expression = term *( ("+" / "-") term )
    /// </summary>
    private Expression ParseExpression()
    {
        Expression left = ParseTerm();

        while (Check(TokenType.Plus) || Check(TokenType.Minus))
        {
            BinaryOperator op = Current().Type == TokenType.Plus
                ? BinaryOperator.Add
                : BinaryOperator.Subtract;
            Advance();

            Expression right = ParseTerm();
            left = new BinaryExpression(left, op, right);
        }

        return left;
    }

    /// <summary>
    /// term = power *( ("*" / "/") power )
    /// </summary>
    private Expression ParseTerm()
    {
        Expression left = ParsePower();

        while (Check(TokenType.Multiply) || Check(TokenType.Divide))
        {
            BinaryOperator op = Current().Type == TokenType.Multiply
                ? BinaryOperator.Multiply
                : BinaryOperator.Divide;
            Advance();

            Expression right = ParsePower();
            left = new BinaryExpression(left, op, right);
        }

        return left;
    }

    /// <summary>
    /// power = unary ["^" power]
    /// Right-associative: 2^3^4 = 2^(3^4)
    /// </summary>
    private Expression ParsePower()
    {
        Expression left = ParseUnary();

        if (Check(TokenType.Power))
        {
            Advance();
            Expression right = ParsePower(); // Recursive for right-associativity
            return new BinaryExpression(left, BinaryOperator.Power, right);
        }

        return left;
    }

    /// <summary>
    /// unary = ["-"] factor
    /// </summary>
    private Expression ParseUnary()
    {
        if (Check(TokenType.Minus))
        {
            Advance();
            Expression operand = ParseFactor();
            return new UnaryExpression(UnaryOperator.Negate, operand);
        }

        return ParseFactor();
    }

    /// <summary>
    /// factor = number / variable / "(" expression ")"
    /// </summary>
    private Expression ParseFactor()
    {
        // Number
        if (Check(TokenType.Number))
        {
            var token = Current();
            Advance();
            return new NumberExpression(token.NumericValue ?? 0);
        }

        // Variable
        if (Check(TokenType.Variable))
        {
            var token = Current();
            Advance();
            return new VariableExpression(token.Text);
        }

        // Parenthesized expression
        if (Check(TokenType.LeftParen))
        {
            Advance();
            Expression expr = ParseExpression();
            Expect(TokenType.RightParen, "Expected ')' after expression");
            return expr;
        }

        // Error
        if (Check(TokenType.Error))
        {
            throw new ParseException($"Invalid character: '{Current().Text}'", Current().Position);
        }

        throw new ParseException($"Expected number, variable, or '(' but found '{Current().Text}'", Current().Position);
    }

    #endregion
}
