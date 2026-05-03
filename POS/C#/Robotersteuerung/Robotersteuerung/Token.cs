using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Robotersteuerung;

public class Token
{
    public string Value { get; }
    public int Line { get; }
    public Token(string value, int line) { Value = value; Line = line; }
}

public class Lexer
{
    private static readonly Regex TokenPatterns = new(
        @"(?<KEYWORD>MOVE|COLLECT|REPEAT|UNTIL|IF|IS-A|OBSTACLE)" + 
        @"|(?<DIRECTION>LEFT|RIGHT|UP|DOWN)" + 
        @"|(?<LBRACE>\{)" +
        @"|(?<RBRACE>\})" +
        @"|(?<NUMBER>[0-9]+)" +
        @"|(?<LETTER>[A-Z])" +
        @"|(?<WHITESPACE>[ \t]+)" +
        @"|(?<NEWLINE>\r?\n)" +
        @"|(?<UNKNOWN>\S+)", RegexOptions.Compiled);

    public static List<Token> Tokenize(string source)
    {
        var tokens = new List<Token>();
        int lineNum = 1;

        foreach (Match match in TokenPatterns.Matches(source))
        {
            if (match.Groups["NEWLINE"].Success)
            {
                lineNum++;
                continue;
            }

            if (match.Groups["WHITESPACE"].Success)
            {
                continue;
            }

            if (match.Groups["UNKNOWN"].Success)
            {
                throw new Exception("Unbekanntes Zeichen oder Token: " + match.Value + ' ' + lineNum);
            }

            tokens.Add(new Token(match.Value, lineNum));
        }

        return tokens;
    }
}