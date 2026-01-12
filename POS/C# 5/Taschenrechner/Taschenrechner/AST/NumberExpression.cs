namespace Taschenrechner.AST;

/// <summary>
/// Repräsentiert eine Zahl im Syntaxbaum (Terminal).
/// </summary>
public class NumberExpression : IExpression
{
    public double Value { get; }

    public NumberExpression(double value)
    {
        Value = value;
    }

    public double Evaluate(EvaluationContext context)
    {
        return Value;
    }

    public ISet<char> GetVariables()
    {
        return new HashSet<char>();
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}
