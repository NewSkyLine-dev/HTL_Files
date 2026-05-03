using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace UPN;

public enum TokenType
{
    Number,
    Operator,
    EOF
}

public sealed record Token(TokenType Type, string Value, int Position);

public sealed class Lexer
{
    // Each tuple: (TokenType, Regex-Pattern)
    private static readonly (TokenType Type, Regex Pattern)[] Rules =
    [
        (TokenType.Number,   new Regex(@"\G-?\d+(\.\d+)?",  RegexOptions.Compiled)),
        (TokenType.Operator, new Regex(@"\G[+\-*/^]",        RegexOptions.Compiled)),
    ];

    private static readonly Regex Whitespace = new(@"\G\s+", RegexOptions.Compiled);

    public static List<Token> Tokenize(string input)
    {
        var tokens = new List<Token>();
        int pos = 0;

        while (pos < input.Length)
        {
            // Skip whitespace
            var wsMatch = Whitespace.Match(input, pos);
            if (wsMatch.Success) { pos += wsMatch.Length; continue; }

            bool matched = false;
            foreach (var (type, pattern) in Rules)
            {
                var m = pattern.Match(input, pos);
                if (m.Success)
                {
                    tokens.Add(new Token(type, m.Value, pos));
                    pos += m.Length;
                    matched = true;
                    break;
                }
            }

            if (!matched)
                throw new Exception($"Unbekanntes Zeichen '{input[pos]}' an Position {pos}.");
        }

        tokens.Add(new Token(TokenType.EOF, "", pos));
        return tokens;
    }
}