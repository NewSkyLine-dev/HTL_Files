using System.Windows;
using System.Windows.Input;
using CalculatorWPF.ViewModels;

namespace CalculatorWPF;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// Minimal code-behind - only handles View-specific interactions (dialogs).
/// </summary>
public partial class MainWindow : Window
{
    private CalculatorViewModel ViewModel => (CalculatorViewModel)DataContext;

    public MainWindow()
    {
        InitializeComponent();

        // Set up callback for requesting variable values via dialog
        // This must be in code-behind because dialogs are View-specific
        Loaded += (_, _) =>
        {
            ViewModel.VariableRequestCallback = RequestVariableValue;
        };
    }

    /// <summary>
    /// Prompts the user for a variable value via dialog.
    /// </summary>
    private double? RequestVariableValue(string variableName)
    {
        var dialog = new VariableInputDialog(variableName)
        {
            Owner = this
        };

        if (dialog.ShowDialog() == true)
        {
            ViewModel.UpdateVariablesDisplay();
            return dialog.Value;
        }

        return null;
    }

    /// <summary>
    /// Handles Enter key to calculate (InputBinding alternative).
    /// </summary>
    private void ExpressionTextBox_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            ViewModel.CalculateCommand.Execute(null);
            e.Handled = true;
        }
    }
}
