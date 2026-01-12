using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taschenrechner.Token;

public class Token(TokenType type, string value, int position)
{
    public TokenType Type { get; } = type;
    public string Value { get; } = value;
    public int Position { get; } = position;

    public override string ToString()
    {
        return $"{Type} '{Value}' @ {Position}";
    }
}
