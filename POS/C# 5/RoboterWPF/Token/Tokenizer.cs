using System.Text.RegularExpressions;

namespace RoboterWPF.Token;

public class Tokenizer
{
    private readonly List<TokenPattern> _patterns = new();

    private class TokenPattern
    {
        public Regex Regex { get; set; } = null!;
        public TokenType Type { get; set; }
        public string Keyword { get; set; } = "";
    }

    public Tokenizer()
    {
        InitializePatterns();
    }

    private void InitializePatterns()
    {
        // Order matters: more specific patterns should come first

        // Keywords
        _patterns.Add(new TokenPattern { Regex = new Regex(@"\bREPEAT\b"), Type = TokenType.Keyword, Keyword = "REPEAT" });
        _patterns.Add(new TokenPattern { Regex = new Regex(@"\bMOVE\b"), Type = TokenType.Keyword, Keyword = "MOVE" });
        _patterns.Add(new TokenPattern { Regex = new Regex(@"\bCOLLECT\b"), Type = TokenType.Keyword, Keyword = "COLLECT" });
        _patterns.Add(new TokenPattern { Regex = new Regex(@"\bUNTIL\b"), Type = TokenType.Keyword, Keyword = "UNTIL" });
        _patterns.Add(new TokenPattern { Regex = new Regex(@"\bIF\b"), Type = TokenType.Keyword, Keyword = "IF" });

        // Condition keyword
        _patterns.Add(new TokenPattern { Regex = new Regex(@"\bIS-A\b"), Type = TokenType.Condition, Keyword = "IS-A" });

        // Target types (OBSTACLE must come before single letters)
        _patterns.Add(new TokenPattern { Regex = new Regex(@"\bOBSTACLE\b"), Type = TokenType.Target, Keyword = "OBSTACLE" });

        // Directions
        _patterns.Add(new TokenPattern { Regex = new Regex(@"\b(UP|DOWN|LEFT|RIGHT)\b"), Type = TokenType.Direction, Keyword = "" });

        // Numbers
        _patterns.Add(new TokenPattern { Regex = new Regex(@"\b\d+\b"), Type = TokenType.Number, Keyword = "" });

        // Single letters (for IS-A A, IS-A B, etc.) - must come after other keywords
        _patterns.Add(new TokenPattern { Regex = new Regex(@"\b[A-Z]\b"), Type = TokenType.Letter, Keyword = "" });

        // Braces
        _patterns.Add(new TokenPattern { Regex = new Regex(@"[\{\}]"), Type = TokenType.Brackets, Keyword = "" });
    }

    public List<Token> Tokenize(string input)
    {
        var tokens = new List<Token>();
        int line = 1;
        int column = 1;
        int position = 0;

        while (position < input.Length)
        {
            char currentChar = input[position];

            // Skip whitespace (except newlines for line tracking)
            if (char.IsWhiteSpace(currentChar))
            {
                if (currentChar == '\n')
                {
                    line++;
                    column = 1;
                }
                else
                {
                    column++;
                }
                position++;
                continue;
            }

            // Try to match patterns
            bool matched = false;

            foreach (var pattern in _patterns)
            {
                var match = pattern.Regex.Match(input, position);

                if (match.Success && match.Index == position)
                {
                    string value = match.Value;
                    var token = new Token(pattern.Type, value, line, column);
                    tokens.Add(token);

                    position += value.Length;
                    column += value.Length;
                    matched = true;
                    break;
                }
            }

            if (!matched)
            {
                // Unknown character, create an Error token
                string unknownChar = input[position].ToString();
                var token = new Token(TokenType.Error, unknownChar, line, column);
                tokens.Add(token);
                position++;
                column++;
            }
        }

        return tokens;
    }
}
