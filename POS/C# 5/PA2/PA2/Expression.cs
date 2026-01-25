using PA2;
using Painter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PA2_5A_2026;

public abstract class Expression
{
    public abstract void Parse(List<Token> tokenlist);
    public abstract void Run(PainterControl painter);
    protected static int _position = 0;
    protected static List<String> _errors = [];
}
