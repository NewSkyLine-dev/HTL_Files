namespace Taschenrechner.AST;

/// <summary>
/// Repräsentiert eine Variable im Syntaxbaum (Terminal).
/// </summary>
public class VariableExpression : IExpression
{
    public char Name { get; }

    public VariableExpression(char name)
    {
        Name = name;
    }

    public double Evaluate(EvaluationContext context)
    {
        return context.GetVariable(Name);
    }

    public ISet<char> GetVariables()
    {
        return new HashSet<char> { Name };
    }

    public override string ToString()
    {
        return Name.ToString();
    }
}
