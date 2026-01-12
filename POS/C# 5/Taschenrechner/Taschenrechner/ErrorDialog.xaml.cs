using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace Taschenrechner;

/// <summary>
/// Dialog zur Anzeige von Fehlern mit Hervorhebung.
/// </summary>
public partial class ErrorDialog : Window
{
    public ErrorDialog(string input, IEnumerable<(int Position, int Length, string Message)> errors)
    {
        InitializeComponent();

        DisplayInputWithErrors(input, errors);
        DisplayErrorList(errors);
    }

    private void DisplayInputWithErrors(string input, IEnumerable<(int Position, int Length, string Message)> errors)
    {
        var paragraph = new Paragraph();
        var errorPositions = new HashSet<int>();

        // Sammle alle Fehlerpositionen
        foreach (var error in errors)
        {
            for (int i = error.Position; i < error.Position + error.Length && i < input.Length; i++)
            {
                errorPositions.Add(i);
            }
        }

        // Baue den formatierten Text auf
        int currentPos = 0;
        while (currentPos < input.Length)
        {
            if (errorPositions.Contains(currentPos))
            {
                // Finde zusammenhängende Fehlerregion
                int errorStart = currentPos;
                while (currentPos < input.Length && errorPositions.Contains(currentPos))
                {
                    currentPos++;
                }

                var errorText = input.Substring(errorStart, currentPos - errorStart);
                var errorRun = new Run(errorText)
                {
                    Background = new SolidColorBrush(Color.FromRgb(255, 200, 200)),
                    Foreground = Brushes.DarkRed,
                    FontWeight = FontWeights.Bold,
                    TextDecorations = TextDecorations.Underline
                };
                paragraph.Inlines.Add(errorRun);
            }
            else
            {
                // Finde zusammenhängende normale Region
                int normalStart = currentPos;
                while (currentPos < input.Length && !errorPositions.Contains(currentPos))
                {
                    currentPos++;
                }

                var normalText = input.Substring(normalStart, currentPos - normalStart);
                paragraph.Inlines.Add(new Run(normalText));
            }
        }

        InputDisplay.Document.Blocks.Clear();
        InputDisplay.Document.Blocks.Add(paragraph);
    }

    private void DisplayErrorList(IEnumerable<(int Position, int Length, string Message)> errors)
    {
        var errorMessages = errors
            .Select(e => $"Position {e.Position + 1}: {e.Message}")
            .ToList();

        ErrorList.ItemsSource = errorMessages;
    }

    private void OkButton_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }
}
