using System.Text;
using System.Windows.Input;
using CalculatorWPF.AST;
using CalculatorWPF.Parser;
using CalculatorWPF.Token;
using AstExpression = CalculatorWPF.AST.Expression;

namespace CalculatorWPF.ViewModels;

/// <summary>
/// ViewModel for the Calculator MainWindow.
/// Implements MVVM pattern with data bindings.
/// </summary>
public class CalculatorViewModel : BaseViewModel
{
    private readonly EvaluationContext _context;

    private string _expression = "3 + x - (5*3)^2";
    private string _result = "= ";
    private string _variablesDisplay = "No variables defined";
    private string _astTree = "";

    public CalculatorViewModel()
    {
        _context = new EvaluationContext();

        // Initialize commands
        CalculateCommand = new RelayCommand(Calculate);
        ClearCommand = new RelayCommand(Clear);
        ClearVariablesCommand = new RelayCommand(ClearVariables);
        InputCommand = new RelayCommand(Input);
    }

    #region Properties

    public string Expression
    {
        get => _expression;
        set => SetProperty(ref _expression, value);
    }

    public string Result
    {
        get => _result;
        set => SetProperty(ref _result, value);
    }

    public string VariablesDisplay
    {
        get => _variablesDisplay;
        set => SetProperty(ref _variablesDisplay, value);
    }

    public string AstTree
    {
        get => _astTree;
        set => SetProperty(ref _astTree, value);
    }

    /// <summary>
    /// Callback for requesting variable values (set by View for dialog support).
    /// </summary>
    public Func<string, double?>? VariableRequestCallback
    {
        get => _context.VariableRequestCallback;
        set => _context.VariableRequestCallback = value;
    }

    #endregion

    #region Commands

    public ICommand CalculateCommand { get; }
    public ICommand ClearCommand { get; }
    public ICommand ClearVariablesCommand { get; }
    public ICommand InputCommand { get; }

    #endregion

    #region Command Methods

    private void Input(object? parameter)
    {
        if (parameter is string input)
        {
            Expression += input;
        }
    }

    private void Clear()
    {
        Expression = "";
        Result = "= ";
        AstTree = "";
    }

    private void ClearVariables()
    {
        _context.Clear();
        UpdateVariablesDisplay();
    }

    private void Calculate()
    {
        string expression = Expression.Trim();

        if (string.IsNullOrEmpty(expression))
        {
            Result = "= ";
            AstTree = "";
            return;
        }

        try
        {
            // Step 1: Tokenize (Lexical Analysis)
            var tokenizer = new Tokenizer(expression);
            List<Token.Token> tokens = tokenizer.Tokenize();

            // Check for lexer errors
            var errorTokens = tokens.Where(t => t.Type == TokenType.Error).ToList();
            if (errorTokens.Count != 0)
            {
                string errors = string.Join(", ", errorTokens.Select(t => $"'{t.Text}' at position {t.Position}"));
                throw new Exception($"Invalid characters: {errors}");
            }

            // Step 2: Parse (Syntactic Analysis) - Build AST
            var parser = new Parser.Parser(tokens);
            AstExpression ast = parser.Parse();

            // Check for parser errors
            if (parser.HasErrors)
            {
                string errors = string.Join("\n", parser.GetErrors().Select(e => e.ToString()));
                throw new Exception($"Parse errors:\n{errors}");
            }

            // Display AST for debugging
            AstTree = ast.ToTreeString();

            // Step 3: Interpret (Evaluate AST using Interpreter Pattern)
            _context.ClearErrors();
            double result = ast.Interpret(_context);

            // Update variables display after evaluation (may have new variables from callback)
            UpdateVariablesDisplay();

            // Display result
            Result = $"= {FormatResult(result)}";
        }
        catch (DivideByZeroException)
        {
            Result = "= Error: Division by zero";
            AstTree = "";
        }
        catch (Exception ex)
        {
            Result = $"= Error: {ex.Message}";
            AstTree = "";
        }
    }

    #endregion

    #region Helper Methods

    public void UpdateVariablesDisplay()
    {
        var vars = _context.Variables;
        if (vars.Count == 0)
        {
            VariablesDisplay = "No variables defined";
        }
        else
        {
            var sb = new StringBuilder();
            foreach (var kvp in vars.OrderBy(x => x.Key))
            {
                if (sb.Length > 0) sb.Append("  |  ");
                sb.Append($"{kvp.Key} = {kvp.Value}");
            }
            VariablesDisplay = sb.ToString();
        }
    }

    private static string FormatResult(double value)
    {
        if (double.IsNaN(value)) return "NaN";
        if (double.IsPositiveInfinity(value)) return "∞";
        if (double.IsNegativeInfinity(value)) return "-∞";

        if (value == Math.Floor(value) && Math.Abs(value) < 1e15)
        {
            return value.ToString("F0", System.Globalization.CultureInfo.InvariantCulture);
        }

        return value.ToString("G15", System.Globalization.CultureInfo.InvariantCulture);
    }

    #endregion
}
