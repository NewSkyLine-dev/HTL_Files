namespace CalculatorWPF.AST;

/// <summary>
/// Defines the binary operators supported by the calculator.
/// </summary>
public enum BinaryOperator
{
    Add,        // +
    Subtract,   // -
    Multiply,   // *
    Divide,     // /
    Power       // ^
}

/// <summary>
/// Non-Terminal Expression - Represents a binary operation.
/// Contains two child expressions and forwards interpret requests to them.
/// </summary>
public class BinaryExpression : Expression
{
    public Expression Left { get; }
    public BinaryOperator Operator { get; }
    public Expression Right { get; }

    public BinaryExpression(Expression left, BinaryOperator op, Expression right)
    {
        Left = left;
        Operator = op;
        Right = right;
    }

    public override double Interpret(EvaluationContext context)
    {
        double leftValue = Left.Interpret(context);
        double rightValue = Right.Interpret(context);

        return Operator switch
        {
            BinaryOperator.Add => leftValue + rightValue,
            BinaryOperator.Subtract => leftValue - rightValue,
            BinaryOperator.Multiply => leftValue * rightValue,
            BinaryOperator.Divide => rightValue != 0 
                ? leftValue / rightValue 
                : throw new DivideByZeroException("Division by zero"),
            BinaryOperator.Power => Math.Pow(leftValue, rightValue),
            _ => throw new InvalidOperationException($"Unknown operator: {Operator}")
        };
    }

    public string OperatorToString()
    {
        return Operator switch
        {
            BinaryOperator.Add => "+",
            BinaryOperator.Subtract => "-",
            BinaryOperator.Multiply => "*",
            BinaryOperator.Divide => "/",
            BinaryOperator.Power => "^",
            _ => "?"
        };
    }

    public override string ToTreeString(int indent = 0)
    {
        var sb = new System.Text.StringBuilder();
        sb.AppendLine($"{GetIndent(indent)}BinaryOp: {OperatorToString()}");
        sb.AppendLine(Left.ToTreeString(indent + 1));
        sb.Append(Right.ToTreeString(indent + 1));
        return sb.ToString();
    }
}
