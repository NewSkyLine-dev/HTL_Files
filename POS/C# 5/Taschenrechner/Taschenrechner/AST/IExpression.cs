namespace Taschenrechner.AST;

/// <summary>
/// Interface für das Interpreter-Pattern.
/// Alle Ausdrucksknoten im Syntaxbaum implementieren dieses Interface.
/// </summary>
public interface IExpression
{
    /// <summary>
    /// Wertet den Ausdruck aus und gibt das Ergebnis zurück.
    /// </summary>
    /// <param name="context">Kontext mit Variablenwerten</param>
    /// <returns>Das berechnete Ergebnis</returns>
    double Evaluate(EvaluationContext context);

    /// <summary>
    /// Sammelt alle Variablen, die in diesem Ausdruck verwendet werden.
    /// </summary>
    /// <returns>Eine Menge von Variablennamen</returns>
    ISet<char> GetVariables();
}
