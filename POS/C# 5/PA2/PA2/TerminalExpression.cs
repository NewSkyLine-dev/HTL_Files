using PA2_5A_2026;
using Painter;

namespace PA2;

public class DrawExpression : Expression
{
    private int _length;

    public override void Parse(List<Token> tokenlist)
    {
        var token = tokenlist[_position];
        if (token == null || token.Type != TokenType.DRAW)
        {
            throw new Exception("Expected DRAW token");
        }
        _position++;

        var length = tokenlist[_position];
        if (length == null || length.Type != TokenType.NUMBER)
        {
            _position--;
            throw new Exception("Expected NUMBER token after DRAW");
        }
        _length = int.Parse(length.Value);
        _position++;
    }

    public override void Run(PainterControl painter)
    {
        painter.Draw(_length);
    }
}

public class TurnExpression : Expression
{
    enum Direction
    {
        Left,
        Right
    }
    private Direction _direction;
    private int _degree;

    public override void Parse(List<Token> tokenlist)
    {
        var token = tokenlist[_position];
        if (token == null || token.Type != TokenType.TURN)
        {
            throw new Exception("Expected TURN token");
        }
        _position++;
        var direction = tokenlist[_position];
        if (direction == null || direction.Type != TokenType.DIRECTION)
        {
            _position--;
            throw new Exception("Expected DIRECTION token after TURN");
        }
        _direction = direction.Value.Equals("left", StringComparison.CurrentCultureIgnoreCase) ? Direction.Left : Direction.Right;
        _position++;
        var degree = tokenlist[_position];
        if (degree == null || degree.Type != TokenType.NUMBER)
        {
            _position--;
            throw new Exception("Expected NUMBER token after DIRECTION");
        }
        _degree = int.Parse(degree.Value);
        _position++;
    }
    public override void Run(PainterControl painter)
    {
        painter.Rotate(_direction == Direction.Left ? -_degree : _degree);
    }
}

public class ColorExpression : Expression
{
    private string? _color;

    public override void Parse(List<Token> tokenlist)
    {
        var token = tokenlist[_position];
        if (token == null || token.Type != TokenType.COLOR)
        {
            throw new Exception("Expected COLOR token");
        }
        _position++;

        token = tokenlist[_position];
        if (token == null || (token.Type != TokenType.COLORNAME && token.Type != TokenType.COLORHEX))
        {
            _position--;
            throw new Exception("Expected COLORNAME or COLORHEX token after COLOR");
        }
        _color = token.Value;
        _position++;
    }
    public override void Run(PainterControl painter)
    {
        if (_color != null)
            painter.ChangeColor(_color);
    }
}