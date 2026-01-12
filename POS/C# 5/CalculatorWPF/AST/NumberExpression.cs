namespace CalculatorWPF.AST;

/// <summary>
/// Terminal Expression - Represents a numeric literal.
/// Directly returns its value without delegating to child expressions.
/// </summary>
public class NumberExpression : Expression
{
    public double Value { get; }

    public NumberExpression(double value)
    {
        Value = value;
    }

    public override double Interpret(EvaluationContext context)
    {
        return Value;
    }

    public override string ToTreeString(int indent = 0)
    {
        return $"{GetIndent(indent)}Number: {Value}";
    }
}
