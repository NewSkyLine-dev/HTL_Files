using System.Text.RegularExpressions;
using Taschenrechner.Token;

namespace Taschenrechner;

/// <summary>
/// Lexer für mathematische Ausdrücke.
/// Wandelt einen Eingabestring in eine Sequenz von Tokens um.
/// </summary>
public partial class Lexer
{
    private static readonly Regex TokenRegex = MathRegex();

    private readonly string _input;
    private int _position = 0;
    private readonly List<Token.Token> _tokens = new();
    private readonly List<LexerError> _errors = new();

    public Lexer(string input)
    {
        _input = input;
    }

    public IReadOnlyList<LexerError> Errors => _errors;
    public bool HasErrors => _errors.Count > 0;

    public List<Token.Token> Tokenize()
    {
        _tokens.Clear();
        _errors.Clear();
        _position = 0;

        while (_position < _input.Length)
        {
            var match = TokenRegex.Match(_input, _position);

            if (!match.Success || match.Index != _position)
            {
                // Unbekanntes Zeichen gefunden
                var errorChar = _input[_position];
                _errors.Add(new LexerError(
                    $"Unerwartetes Zeichen '{errorChar}'",
                    _position,
                    1
                ));
                _tokens.Add(new Token.Token(TokenType.Error, errorChar.ToString(), _position));
                _position++;
                continue;
            }

            if (match.Groups["WHITESPACE"].Success)
            {
                _position += match.Length;
                continue;
            }

            Token.Token? token = null;

            if (match.Groups["NUMBER"].Success)
            {
                token = new Token.Token(TokenType.Number, match.Value, _position);
            }
            else if (match.Groups["VARIABLE"].Success)
            {
                token = new Token.Token(TokenType.Variable, match.Value, _position);
            }
            else if (match.Groups["PLUS"].Success)
            {
                token = new Token.Token(TokenType.Plus, match.Value, _position);
            }
            else if (match.Groups["MINUS"].Success)
            {
                token = new Token.Token(TokenType.Minus, match.Value, _position);
            }
            else if (match.Groups["STAR"].Success)
            {
                token = new Token.Token(TokenType.Star, match.Value, _position);
            }
            else if (match.Groups["SLASH"].Success)
            {
                token = new Token.Token(TokenType.Slash, match.Value, _position);
            }
            else if (match.Groups["CARET"].Success)
            {
                token = new Token.Token(TokenType.Caret, match.Value, _position);
            }
            else if (match.Groups["LPAREN"].Success)
            {
                token = new Token.Token(TokenType.LeftParen, match.Value, _position);
            }
            else if (match.Groups["RPAREN"].Success)
            {
                token = new Token.Token(TokenType.RightParen, match.Value, _position);
            }
            else if (match.Groups["ERROR"].Success)
            {
                _errors.Add(new LexerError(
                    $"Ungültiges Zeichen '{match.Value}'",
                    _position,
                    match.Length
                ));
                token = new Token.Token(TokenType.Error, match.Value, _position);
            }

            if (token != null)
            {
                _tokens.Add(token);
            }

            _position += match.Length;
        }

        _tokens.Add(new Token.Token(TokenType.EndOfFile, string.Empty, _position));
        return _tokens;
    }

    [GeneratedRegex(@"\G(?:
            (?<WHITESPACE>\s+) |
            (?<NUMBER>\d+([.,]\d+)?) |
            (?<VARIABLE>[a-z]) |
            (?<PLUS>\+) |
            (?<MINUS>-) |
            (?<STAR>\*) |
            (?<SLASH>/) |
            (?<CARET>\^) |
            (?<LPAREN>\() |
            (?<RPAREN>\)) |
            (?<ERROR>.)
        )", RegexOptions.IgnorePatternWhitespace)]
    private static partial Regex MathRegex();
}

/// <summary>
/// Repräsentiert einen Lexer-Fehler.
/// </summary>
public class LexerError
{
    public string Message { get; }
    public int Position { get; }
    public int Length { get; }

    public LexerError(string message, int position, int length)
    {
        Message = message;
        Position = position;
        Length = length;
    }
}
