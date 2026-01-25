using PA2_5A_2026;
using Painter;

namespace PA2;

public class ProgramExpression : Expression
{
    private List<Expression> _expressions = [];

    public override void Parse(List<Token> tokenlist)
    {
        Expression? stmt = null;
        var token = tokenlist[_position];
        while (token != null && _position < tokenlist.Count && token.Type != TokenType.EOF)
        { 
            try { 
                switch (token.Type)
                {
                    case TokenType.FOR:
                        stmt = new ForExpression();
                        stmt.Parse(tokenlist);
                        _expressions.Add(stmt);
                        break;
                    case TokenType.EOF:
                        _expressions.Add(null!);
                        break;
                    case TokenType.COLOR:
                        stmt = new ColorExpression();
                        stmt.Parse(tokenlist);
                        _expressions.Add(stmt);
                        break;
                    case TokenType.TURN:
                        stmt = new TurnExpression();
                        stmt.Parse(tokenlist);
                        _expressions.Add(stmt);
                        break;
                    case TokenType.DRAW:
                        stmt = new DrawExpression();
                        stmt.Parse(tokenlist);
                        _expressions.Add(stmt);
                        break;
                    case TokenType.INVALID:
                    default:
                        throw new Exception($"Unexpected token: {token.Value}");
                }
            }
            catch (Exception e) 
            {
                _position++;
                _errors.Add(e.Message);
            }
            token = tokenlist[_position];
        }
    }

    public static bool HasErrors() => _errors.Count > 0;

    public static List<string> GetErrors() => _errors;

    public override void Run(PainterControl painter)
    {
        foreach (Expression expr in _expressions)
        {
            expr.Run(painter);
        }
    }
}
