namespace UPN;

public static class Evaluator
{
    public static double Evaluate(AstNode node) => node switch
    {
        NumberNode(var v) => v,
        BinaryNode("+", var l, var r) => Evaluate(l) + Evaluate(r),
        BinaryNode("-", var l, var r) => Evaluate(l) - Evaluate(r),
        BinaryNode("*", var l, var r) => Evaluate(l) * Evaluate(r),
        BinaryNode("/", var l, var r) => Evaluate(r) == 0
                                            ? throw new DivideByZeroException()
                                            : Evaluate(l) / Evaluate(r),
        BinaryNode("^", var l, var r) => Math.Pow(Evaluate(l), Evaluate(r)),
        _ => throw new Exception("Unbekannter AST-Knoten.")
    };

    /// Gibt den AST leserlich als Infix-Ausdruck aus (mit Klammern).
    public static string ToInfix(AstNode node) => node switch
    {
        NumberNode(var v) => v.ToString(System.Globalization.CultureInfo.InvariantCulture),
        BinaryNode(var op, var l, var r) => $"({ToInfix(l)} {op} {ToInfix(r)})",
        _ => throw new Exception("Unbekannter AST-Knoten.")
    };
}