namespace CalculatorWPF.AST;

/// <summary>
/// Terminal Expression - Represents a variable reference.
/// Looks up the variable value in the context.
/// </summary>
public class VariableExpression : Expression
{
    public string Name { get; }

    public VariableExpression(string name)
    {
        Name = name;
    }

    public override double Interpret(EvaluationContext context)
    {
        return context.GetVariable(Name);
    }

    public override string ToTreeString(int indent = 0)
    {
        return $"{GetIndent(indent)}Variable: {Name}";
    }
}
