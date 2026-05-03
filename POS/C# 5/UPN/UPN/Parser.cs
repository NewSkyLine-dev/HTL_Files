using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPN;

public abstract record AstNode;
public sealed record NumberNode(double Value) : AstNode;
public sealed record BinaryNode(string Op, AstNode Left, AstNode Right) : AstNode;

public sealed class Parser
{
    public static IExpression Build(List<Token> tokens)
    {
        var stack = new Stack<IExpression>();

        foreach (var token in tokens)
        {
            switch (token.Type)
            {
                case TokenType.EOF:
                    break;

                case TokenType.Number:
                    double value = double.Parse(token.Value, CultureInfo.InvariantCulture);
                    stack.Push(new NumberExpression(value));
                    break;

                case TokenType.Operator:
                    if (stack.Count < 2)
                        throw new Exception(
                            $"Zu wenige Operanden für '{token.Value}' an Position {token.Position}.");

                    // UPN: rechter Operand liegt oben auf dem Stack
                    var right = stack.Pop();
                    var left = stack.Pop();

                    IExpression expr = token.Value switch
                    {
                        "+" => new AddExpression(left, right),
                        "-" => new SubtractExpression(left, right),
                        "*" => new MultiplyExpression(left, right),
                        "/" => new DivideExpression(left, right),
                        "^" => new PowerExpression(left, right),
                        _ => throw new Exception($"Unbekannter Operator: {token.Value}")
                    };
                    stack.Push(expr);
                    break;
            }
        }

        if (stack.Count != 1)
            throw new Exception(
                $"Ungültiger Ausdruck: {stack.Count} Wert(e) übrig (erwartet: 1).");

        return stack.Pop();
    }

    /// <summary>Wertet einen fertigen IExpression-Baum aus.</summary>
    public static double Evaluate(IExpression expression)
    {
        var context = new Context();
        expression.Interpret(context);

        if (context.Count != 1)
            throw new Exception("Auswertungsfehler: Stack-Inkonsistenz nach Interpret().");

        return context.Pop();
    }
}
