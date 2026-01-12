namespace Taschenrechner.AST;

/// <summary>
/// Repräsentiert einen unären Operator im Syntaxbaum (z.B. Negation).
/// </summary>
public class UnaryExpression : IExpression
{
    public IExpression Operand { get; }
    public UnaryOperator Operator { get; }

    public UnaryExpression(UnaryOperator op, IExpression operand)
    {
        Operator = op;
        Operand = operand;
    }

    public double Evaluate(EvaluationContext context)
    {
        var value = Operand.Evaluate(context);

        return Operator switch
        {
            UnaryOperator.Negate => -value,
            _ => throw new InvalidOperationException($"Unbekannter Operator: {Operator}")
        };
    }

    public ISet<char> GetVariables()
    {
        return Operand.GetVariables();
    }

    public override string ToString()
    {
        return Operator switch
        {
            UnaryOperator.Negate => $"(-{Operand})",
            _ => Operand.ToString() ?? ""
        };
    }
}

public enum UnaryOperator
{
    Negate
}
