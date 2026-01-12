using RoboterWPF.Token;

namespace RoboterWPF.Parser;

public class ParseException(string message, int line = 0, int column = 0)
{
    public string Message { get; set; } = message;
    public int Line { get; set; } = line;
    public int Column { get; set; } = column;

    public override string ToString()
    {
        return $"Parse Error at line {Line}, column {Column}: {Message}";
    }
}

public class Parser(List<Token.Token> tokens)
{
    private readonly List<Token.Token> _tokens = tokens;
    private int _currentIndex = 0;
    private readonly List<ParseException> _errors = [];

    public ProgramExpression Parse()
    {
        var program = new ProgramExpression();
        _errors.Clear();

        while (!IsAtEnd())
        {
            try
            {
                var stmt = ParseStatement();
                if (stmt != null)
                {
                    program.AddStatement(stmt);
                }
            }
            catch (Exception)
            {
                Synchronize();
            }
        }

        return program;
    }

    public List<ParseException> GetErrors() => _errors;
    public bool HasErrors() => _errors.Count > 0;

    private Token.Token Current()
    {
        if (_currentIndex >= _tokens.Count)
        {
            return new Token.Token(TokenType.Error, "EOF", 0, 0);
        }
        return _tokens[_currentIndex];
    }

    private Token.Token Peek(int offset = 1)
    {
        int index = _currentIndex + offset;
        if (index >= _tokens.Count)
        {
            return new Token.Token(TokenType.Error, "EOF", 0, 0);
        }
        return _tokens[index];
    }

    private bool IsAtEnd() => _currentIndex >= _tokens.Count;

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
        return _tokens[_currentIndex].Type == type;
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

    private void Expect(TokenType type, string message)
    {
        if (!Check(type))
        {
            AddError(message);
            throw new InvalidOperationException(message);
        }
        Advance();
    }

    private void AddError(string message)
    {
        _errors.Add(new ParseException(message, Current().Line, Current().Column));
    }

    private void Synchronize()
    {
        Advance();

        while (!IsAtEnd())
        {
            // Try to find the next statement start
            if (Check(TokenType.Keyword))
            {
                string keyword = Current().Text;
                if (keyword == "REPEAT" || keyword == "MOVE" || keyword == "COLLECT" ||
                    keyword == "UNTIL" || keyword == "IF")
                {
                    return;
                }
            }
            Advance();
        }
    }

    private StatementExpression ParseStatement()
    {
        if (!Check(TokenType.Keyword))
        {
            AddError("Expected a statement keyword (REPEAT, MOVE, COLLECT, UNTIL, or IF)");
            throw new InvalidOperationException("Expected statement");
        }

        string keyword = Current().Text;

        return keyword switch
        {
            "REPEAT" => ParseRepeat(),
            "MOVE" => ParseMove(),
            "COLLECT" => ParseCollect(),
            "UNTIL" => ParseUntil(),
            "IF" => ParseIf(),
            _ => throw new InvalidOperationException($"Unknown keyword: {keyword}")
        };
    }

    private RepeatExpression ParseRepeat()
    {
        Expect(TokenType.Keyword, "Expected 'REPEAT'");

        if (!Check(TokenType.Number))
        {
            AddError("Expected number after REPEAT");
            throw new InvalidOperationException("Expected number");
        }

        int count = int.Parse(Current().Text);
        Advance();

        Expect(TokenType.Brackets, "Expected '{' after REPEAT count");

        var repeatExpression = new RepeatExpression(count);

        // Parse statements inside the block
        while (!Check(TokenType.Brackets) && !IsAtEnd())
        {
            var stmt = ParseStatement();
            if (stmt != null)
            {
                repeatExpression.AddStatement(stmt);
            }
        }

        if (!Check(TokenType.Brackets) || Current().Text != "}")
        {
            AddError("Expected '}' to close REPEAT block");
            throw new InvalidOperationException("Expected '}'");
        }
        Advance();

        return repeatExpression;
    }

    private MoveExpression ParseMove()
    {
        Expect(TokenType.Keyword, "Expected 'MOVE'");

        if (!Check(TokenType.Direction))
        {
            AddError("Expected direction (UP, DOWN, LEFT, RIGHT) after MOVE");
            throw new InvalidOperationException("Expected direction");
        }

        Direction dir = DirectionExtensions.FromString(Current().Text);
        Advance();

        return new MoveExpression(dir);
    }

    private CollectExpression ParseCollect()
    {
        Expect(TokenType.Keyword, "Expected 'COLLECT'");
        return new CollectExpression();
    }

    private ConditionExpression ParseCondition()
    {
        // Parse: DIRECTION IS-A TARGET
        // e.g., "DOWN IS-A OBSTACLE" or "UP IS-A A"

        if (!Check(TokenType.Direction))
        {
            AddError("Expected direction (UP, DOWN, LEFT, RIGHT) in condition");
            throw new InvalidOperationException("Expected direction");
        }

        Direction dir = DirectionExtensions.FromString(Current().Text);
        Advance();

        if (!Check(TokenType.Condition) || Current().Text != "IS-A")
        {
            AddError("Expected 'IS-A' after direction in condition");
            throw new InvalidOperationException("Expected IS-A");
        }
        Advance();

        // Check for OBSTACLE or a Letter
        string target;
        bool isObstacle;

        if (Check(TokenType.Target) && Current().Text == "OBSTACLE")
        {
            target = "OBSTACLE";
            isObstacle = true;
            Advance();
        }
        else if (Check(TokenType.Letter))
        {
            target = Current().Text;
            isObstacle = false;
            Advance();
        }
        else
        {
            AddError("Expected 'OBSTACLE' or a letter (A, B, etc.) after IS-A");
            throw new InvalidOperationException("Expected target");
        }

        return new ConditionExpression(dir, target, isObstacle);
    }

    private UntilExpression ParseUntil()
    {
        Expect(TokenType.Keyword, "Expected 'UNTIL'");

        // Parse condition
        var condition = ParseCondition();

        // Expect opening brace
        if (!Check(TokenType.Brackets) || Current().Text != "{")
        {
            AddError("Expected '{' after UNTIL condition");
            throw new InvalidOperationException("Expected '{'");
        }
        Advance();

        var untilExpression = new UntilExpression(condition);

        // Parse statements inside the block
        while (!IsAtEnd() && !(Check(TokenType.Brackets) && Current().Text == "}"))
        {
            var stmt = ParseStatement();
            if (stmt != null)
            {
                untilExpression.AddStatement(stmt);
            }
        }

        if (!Check(TokenType.Brackets) || Current().Text != "}")
        {
            AddError("Expected '}' to close UNTIL block");
            throw new InvalidOperationException("Expected '}'");
        }
        Advance();

        return untilExpression;
    }

    private IfExpression ParseIf()
    {
        Expect(TokenType.Keyword, "Expected 'IF'");

        // Parse condition
        var condition = ParseCondition();

        // Expect opening brace
        if (!Check(TokenType.Brackets) || Current().Text != "{")
        {
            AddError("Expected '{' after IF condition");
            throw new InvalidOperationException("Expected '{'");
        }
        Advance();

        var ifExpression = new IfExpression(condition);

        // Parse statements inside the block
        while (!IsAtEnd() && !(Check(TokenType.Brackets) && Current().Text == "}"))
        {
            var stmt = ParseStatement();
            if (stmt != null)
            {
                ifExpression.AddStatement(stmt);
            }
        }

        if (!Check(TokenType.Brackets) || Current().Text != "}")
        {
            AddError("Expected '}' to close IF block");
            throw new InvalidOperationException("Expected '}'");
        }
        Advance();

        return ifExpression;
    }
}
