namespace CalculatorWPF.AST;

/// <summary>
/// Defines unary operators.
/// </summary>
public enum UnaryOperator
{
    Negate  // -
}

/// <summary>
/// Non-Terminal Expression - Represents a unary operation.
/// Contains one child expression and forwards interpret request to it.
/// </summary>
public class UnaryExpression : Expression
{
    public UnaryOperator Operator { get; }
    public Expression Operand { get; }

    public UnaryExpression(UnaryOperator op, Expression operand)
    {
        Operator = op;
        Operand = operand;
    }

    public override double Interpret(EvaluationContext context)
    {
        double value = Operand.Interpret(context);

        return Operator switch
        {
            UnaryOperator.Negate => -value,
            _ => throw new InvalidOperationException($"Unknown unary operator: {Operator}")
        };
    }

    public string OperatorToString()
    {
        return Operator switch
        {
            UnaryOperator.Negate => "-",
            _ => "?"
        };
    }

    public override string ToTreeString(int indent = 0)
    {
        var sb = new System.Text.StringBuilder();
        sb.AppendLine($"{GetIndent(indent)}UnaryOp: {OperatorToString()}");
        sb.Append(Operand.ToTreeString(indent + 1));
        return sb.ToString();
    }
}
