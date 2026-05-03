using AbcRobotCore;

namespace Robotersteuerung;

public class Parser(string source)
{
    private readonly List<Token> _tokens = Lexer.Tokenize(source);
    private int _pos;
    private Token? Current => _pos < _tokens.Count ? _tokens[_pos] : null;

    private Token Consume(string? expected = null)
    {
        if (Current == null)
            throw new Exception("Unerwartetes Ende des Programs!");

        if (expected != null && Current.Value != expected)
            throw new Exception($"Erwartet '{expected}', gefunden '{Current.Value}'");

        return _tokens[_pos++];
    }

    public List<IExpression> Parse()
    {
        var stmts = new List<IExpression>();
        while (_pos < _tokens.Count && Current?.Value != "}")
            stmts.Add(ParseStatement());
        return stmts;
    }

    private IExpression ParseStatement()
    {
        var tok = Current ?? throw new Exception("Unerwartetes Ende!");

        return tok.Value switch
        {
            "MOVE" => ParseMove(),
            "COLLECT" => ParseCollect(),
            "REPEAT" => ParseRepeat(),
            "IF" => ParseIf(),
            "UNTIL" => ParseUntil(),
            _ => throw new Exception($"Unbekanntes Schlüsselwort '{tok.Value}'")
        };
    }

    private IExpression ParseUntil()
    {
        Consume("UNTIL");
        var dirTok = Consume();
        Consume("IS-A");
        var condTok = Consume();
        var body = ParseBlock();
        return new UntilExpression(ParseDirection(dirTok), condTok.Value, body);
    }

    private IExpression ParseIf()
    {
        Consume("IF");
        var dirTok = Consume();
        Consume("IS-A");
        var condTok = Consume();
        var body = ParseBlock();
        return new IfExpression(ParseDirection(dirTok), condTok.Value, body);
    }

    private IExpression ParseRepeat()
    {
        Consume("REPEAT");
        var countTok = Consume();
        if (!int.TryParse(countTok.Value, out int count))
            throw new Exception($"Zahl erwartet, gefunden '{countTok.Value}'");

        var body = ParseBlock();
        return new RepeatExpression(count, body);
    }

    private BlockExpression ParseBlock()
    {
        Consume("{");
        var stmts = Parse();
        Consume("}");
        return new BlockExpression(stmts);
    }

    private IExpression ParseCollect()
    {
        Consume("COLLECT");
        return new CollectExpression();
    }

    private IExpression ParseMove()
    {
        Consume("MOVE");
        var dirToken = Consume();
        return new MoveExpression(ParseDirection(dirToken));
    }

    private RobotField.Direction ParseDirection(Token dirToken)
    {
        return dirToken.Value switch
        {
            "LEFT" => RobotField.Direction.Left,
            "RIGHT" => RobotField.Direction.Right,
            "UP" => RobotField.Direction.Up,
            "DOWN" => RobotField.Direction.Down,
            _ => throw new Exception($"Ungültige Richtung '${dirToken.Value}'")
        };
    }
}
