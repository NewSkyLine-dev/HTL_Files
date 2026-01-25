using PA2_5A_2026;
using Painter;

namespace PA2;

public class ForExpression : Expression
{
    private int _count;
    private readonly List<Expression> _expressions = [];

    public override void Parse(List<Token> tokenlist)
    {
        var token = tokenlist[_position];
        if (token == null || token.Type != TokenType.FOR)
        {
            throw new Exception("Expected 'for' token");
        }
        _position++;

        var countToken = tokenlist[_position];
        if (countToken == null || countToken.Type != TokenType.NUMBER)
        {
            throw new Exception("Expected number token after 'for'");
        }
        _count = int.Parse(countToken.Value);
        _position++;

        var lparen = tokenlist[_position];
        if (lparen == null || lparen.Type != TokenType.LPAREN)
        {
            throw new Exception("Expected '(' token after 'for' count");
        }
        _position++;
        token = tokenlist[_position];

        while (tokenlist[_position] != null && tokenlist[_position].Type != TokenType.RPAREN)
        {
            try
            {
                Expression? stmt;
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
                _errors.Add(e.Message);
            }
            token = tokenlist[_position];
        }
        _position++;
    }

    public override void Run(PainterControl painter)
    {
        for (int i = 0; i < _count; i++)
        {
            foreach (var expression in _expressions)
            {
                expression.Run(painter);
            }
        }
    }
}