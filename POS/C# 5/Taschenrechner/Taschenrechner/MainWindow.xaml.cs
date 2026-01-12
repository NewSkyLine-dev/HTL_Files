using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Taschenrechner.AST;

namespace Taschenrechner;

/// <summary>
/// Hauptfenster des Taschenrechners.
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        InputTextBox.Focus();
    }

    /// <summary>
    /// Fügt den Text des geklickten Buttons zur Eingabe hinzu.
    /// </summary>
    private void Button_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button button)
        {
            var text = button.Content.ToString();
            var caretIndex = InputTextBox.CaretIndex;
            InputTextBox.Text = InputTextBox.Text.Insert(caretIndex, text ?? "");
            InputTextBox.CaretIndex = caretIndex + (text?.Length ?? 0);
            InputTextBox.Focus();
        }
    }

    /// <summary>
    /// Löscht die aktuelle Eingabe.
    /// </summary>
    private void ClearButton_Click(object sender, RoutedEventArgs e)
    {
        InputTextBox.Text = "";
        ResultDisplay.Text = "0";
        InputTextBox.Focus();
    }

    /// <summary>
    /// Wertet den eingegebenen Ausdruck aus.
    /// </summary>
    private void EvaluateButton_Click(object sender, RoutedEventArgs e)
    {
        EvaluateExpression();
    }

    /// <summary>
    /// Behandelt Tastatureingaben (Enter zum Auswerten).
    /// </summary>
    private void InputTextBox_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            EvaluateExpression();
            e.Handled = true;
        }
    }

    /// <summary>
    /// Hauptlogik für die Auswertung des Ausdrucks.
    /// </summary>
    private void EvaluateExpression()
    {
        var input = InputTextBox.Text.Trim();

        if (string.IsNullOrEmpty(input))
        {
            ResultDisplay.Text = "0";
            return;
        }

        // 1. Lexer: String in Tokens umwandeln
        var lexer = new Lexer(input);
        var tokens = lexer.Tokenize();

        // Prüfe auf Lexer-Fehler
        if (lexer.HasErrors)
        {
            var errors = lexer.Errors
                .Select(e => (e.Position, e.Length, e.Message))
                .ToList();

            var errorDialog = new ErrorDialog(input, errors);
            errorDialog.Owner = this;
            errorDialog.ShowDialog();
            return;
        }

        // 2. Parser: Tokens in AST umwandeln
        var parser = new Parser(tokens);
        IExpression? expression;

        try
        {
            expression = parser.Parse();
        }
        catch (ParseException ex)
        {
            // Zeige Parse-Fehler an
            var errors = parser.Errors
                .Select(e => (e.Position, 1, e.Message))
                .ToList();

            if (errors.Count == 0)
            {
                errors.Add((ex.Position, 1, ex.Message));
            }

            var errorDialog = new ErrorDialog(input, errors);
            errorDialog.Owner = this;
            errorDialog.ShowDialog();
            return;
        }

        // 3. Variablen sammeln
        var variables = expression.GetVariables();

        // 4. Kontext für die Auswertung erstellen
        var context = new EvaluationContext();

        // 5. Falls Variablen vorhanden, Dialog zur Eingabe anzeigen
        if (variables.Count > 0)
        {
            var variableDialog = new VariableInputDialog(variables);
            variableDialog.Owner = this;

            if (variableDialog.ShowDialog() == true)
            {
                var variableValues = variableDialog.GetVariableValues();
                foreach (var (name, value) in variableValues)
                {
                    context.SetVariable(name, value);
                }
            }
            else
            {
                // Benutzer hat abgebrochen
                return;
            }
        }

        // 6. Ausdruck auswerten
        try
        {
            var result = expression.Evaluate(context);

            // Ergebnis formatieren (sinnvolle Dezimalstellen)
            if (Math.Abs(result - Math.Round(result)) < 1e-10)
            {
                ResultDisplay.Text = ((long)Math.Round(result)).ToString();
            }
            else
            {
                ResultDisplay.Text = result.ToString("G10");
            }
        }
        catch (DivideByZeroException)
        {
            MessageBox.Show(
                "Division durch Null ist nicht erlaubt!",
                "Rechenfehler",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"Fehler bei der Berechnung: {ex.Message}",
                "Fehler",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
    }
}