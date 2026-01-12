namespace CalculatorWPF.AST;

/// <summary>
/// Abstract Expression - Base class for all AST nodes in the Interpreter Pattern.
/// Each node knows how to evaluate itself given a context.
/// </summary>
public abstract class Expression
{
    /// <summary>
    /// Interprets/evaluates this expression within the given context.
    /// This is the core method of the Interpreter Pattern.
    /// </summary>
    public abstract double Interpret(EvaluationContext context);

    /// <summary>
    /// Returns a string representation of this expression for debugging.
    /// </summary>
    public abstract string ToTreeString(int indent = 0);

    protected static string GetIndent(int level)
    {
        return new string(' ', level * 2);
    }
}
