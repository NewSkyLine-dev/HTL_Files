namespace Taschenrechner.AST;

/// <summary>
/// Repräsentiert einen binären Operator im Syntaxbaum.
/// </summary>
public class BinaryExpression : IExpression
{
    public IExpression Left { get; }
    public IExpression Right { get; }
    public BinaryOperator Operator { get; }

    public BinaryExpression(IExpression left, BinaryOperator op, IExpression right)
    {
        Left = left;
        Operator = op;
        Right = right;
    }

    public double Evaluate(EvaluationContext context)
    {
        var leftValue = Left.Evaluate(context);
        var rightValue = Right.Evaluate(context);

        return Operator switch
        {
            BinaryOperator.Add => leftValue + rightValue,
            BinaryOperator.Subtract => leftValue - rightValue,
            BinaryOperator.Multiply => leftValue * rightValue,
            BinaryOperator.Divide => rightValue != 0 
                ? leftValue / rightValue 
                : throw new DivideByZeroException("Division durch Null ist nicht erlaubt."),
            BinaryOperator.Power => Math.Pow(leftValue, rightValue),
            _ => throw new InvalidOperationException($"Unbekannter Operator: {Operator}")
        };
    }

    public ISet<char> GetVariables()
    {
        var variables = new HashSet<char>(Left.GetVariables());
        variables.UnionWith(Right.GetVariables());
        return variables;
    }

    public override string ToString()
    {
        var opSymbol = Operator switch
        {
            BinaryOperator.Add => "+",
            BinaryOperator.Subtract => "-",
            BinaryOperator.Multiply => "*",
            BinaryOperator.Divide => "/",
            BinaryOperator.Power => "^",
            _ => "?"
        };
        return $"({Left} {opSymbol} {Right})";
    }
}

public enum BinaryOperator
{
    Add,
    Subtract,
    Multiply,
    Divide,
    Power
}
