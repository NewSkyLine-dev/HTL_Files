using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace Taschenrechner
{
    public class Token(string value)
    {
        public string Value { get; } = value;
    }

    public class Lexer
    {
        private static Regex MathRegex = new(@"(?<NUMBER>[0-9]+)" + 
                                @"|(?<VARIABLE>[a-z])" + 
                                @"|(?<LPAREN>\()" + 
                                @"|(?<RPAREN>\))" + 
                                @"|(?<POTENZ>\^)" + 
                                @"|(?<STRICH>\+|\-)" + 
                                @"|(?<PUNKT>\*|\/)" + 
                                @"|(?<WHITESPACE>[ \t]+)" + 
                                @"|(?<UNKNOWN>\S+)");

        public static List<Token> Tokenize(string raw)
        {
            List<Token> tokens = [];

            foreach (Match match in MathRegex.Matches(raw))
            {
                if (match.Groups["UNKNOWN"].Success)
                    throw new Exception("Unbekannt");

                if (match.Groups["WHITESPACE"].Success)
                    continue;

                if (match.Groups["VARIABLE"].Success)
                {
                    Variable dlg = new();
                    dlg.ShowDialog();

                    if (dlg.DialogResult == true)
                        Context.Variables[match.Value] = (int)dlg.Result;
                }

                tokens.Add(new(match.Value));
            }

            return tokens;
        }
    }
}
