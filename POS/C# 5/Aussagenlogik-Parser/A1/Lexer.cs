using System;
using System.Collections.Generic;

namespace A1
{
    public class Tokenizer
    {
        public static List<Token> Tokenize(string raw)
        {
            List<Token> tokens = new List<Token>();

            if (raw == null)
            {
                raw = string.Empty;
            }

            for (int index = 0; index < raw.Length; index++)
            {
                char current = raw[index];

                if (char.IsWhiteSpace(current))
                {
                    continue;
                }

                if (char.IsLetter(current))
                {
                    tokens.Add(new Token(TokenType.Variable, char.ToLowerInvariant(current).ToString()));
                    continue;
                }

                switch (current)
                {
                    case '!':
                    case '¬':
                    case '~':
                        tokens.Add(new Token(TokenType.Not, current.ToString()));
                        break;
                    case '&':
                    case '∧':
                        tokens.Add(new Token(TokenType.And, current.ToString()));
                        break;
                    case '|':
                    case '⋁':
                    case '∨':
                        tokens.Add(new Token(TokenType.Or, current.ToString()));
                        break;
                    case '=':
                    case '↔':
                        tokens.Add(new Token(TokenType.Equivalence, current.ToString()));
                        break;
                    case '>':
                    case '→':
                        tokens.Add(new Token(TokenType.Implication, current.ToString()));
                        break;
                    case '-':
                        if (index + 1 < raw.Length && raw[index + 1] == '>')
                        {
                            tokens.Add(new Token(TokenType.Implication, "->"));
                            index++;
                            break;
                        }

                        throw new Exception("Unbekanntes Zeichen '-'");
                    case '(':
                        tokens.Add(new Token(TokenType.LeftParen, current.ToString()));
                        break;
                    case ')':
                        tokens.Add(new Token(TokenType.RightParen, current.ToString()));
                        break;
                    default:
                        throw new Exception(string.Format("Unbekanntes Zeichen '{0}'", current));
                }
            }

            tokens.Add(new Token(TokenType.End, string.Empty));

            return tokens;
        }
    }
}
