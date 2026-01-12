namespace CalculatorWPF.AST;

/// <summary>
/// Non-Terminal Expression - Represents a variable assignment.
/// Evaluates the right-hand side and stores it in the context.
/// Example: x = 5 + 3
/// </summary>
public class AssignmentExpression : Expression
{
    public string VariableName { get; }
    public Expression Value { get; }

    public AssignmentExpression(string variableName, Expression value)
    {
        VariableName = variableName;
        Value = value;
    }

    public override double Interpret(EvaluationContext context)
    {
        double result = Value.Interpret(context);
        context.SetVariable(VariableName, result);
        return result;
    }

    public override string ToTreeString(int indent = 0)
    {
        var sb = new System.Text.StringBuilder();
        sb.AppendLine($"{GetIndent(indent)}Assignment: {VariableName} =");
        sb.Append(Value.ToTreeString(indent + 1));
        return sb.ToString();
    }
}
