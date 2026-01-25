using System.Text.RegularExpressions;

namespace PA2;

public enum TokenType
{
    LPAREN,
    RPAREN,
    NUMBER,
    DIRECTION,
    FOR,
    EOF,
    COLOR,
    TURN,
    DRAW,
    COLORNAME,
    COLORHEX,
    INVALID
}

public class Token(TokenType type, string value)
{
    public TokenType Type { get; set; } = type;
    public string Value { get; set; } = value;
}

public class Error
{
    public string Message { get; set; }
}

public class Tokenizer(string input)
{
    private readonly string _input = input;
    private int _position = 0;
    private List<Error> _errors = [];
    private readonly List<(Regex regex, TokenType type)> _patterns =
    [
        (new(@"^TURN\b"), TokenType.TURN),
        (new(@"^DRAW\b"), TokenType.DRAW),
        (new(@"^COLOR\b"), TokenType.COLOR),
        (new(@"^FOR\b"), TokenType.FOR),
        (new(@"^\{"), TokenType.LPAREN),
        (new(@"^\}"), TokenType.RPAREN),
        (new(@"^#([0-9a-fA-F]{6})\b"), TokenType.COLORHEX),
        (new(@"^\b(red|green|blue|white)\b", RegexOptions.IgnoreCase), TokenType.COLORNAME),
        (new(@"^\b(right|left)\b", RegexOptions.IgnoreCase), TokenType.DIRECTION),
        (new(@"^\d+\b"), TokenType.NUMBER)
    ];

    public List<Token> Tokenize()
    {
        List<Token> tokens = [];
        while (_position < _input.Length)
        {
            bool matched = false;
            foreach (var (regex, type) in _patterns)
            {
                var match = regex.Match(_input[_position..]);
                if (match.Success)
                {
                    tokens.Add(new Token(type, match.Value));
                    _position += match.Length;
                    matched = true;
                    break;
                }
            }
            if (!matched)
            {
                if (char.IsWhiteSpace(_input[_position]))
                {
                    _position++;
                }
                else
                {
                    _errors.Add(new Error { Message = $"Unbekannter token '{_input[_position]}'" });
                    _position++;
                }
            }
        }
        tokens.Add(new Token(TokenType.EOF, string.Empty));
        return tokens;
    }
}