using System.Windows;
using System.Windows.Input;

namespace CalculatorWPF;

/// <summary>
/// Dialog window to prompt the user for a variable value.
/// </summary>
public partial class VariableInputDialog : Window
{
    public string VariableName { get; }
    public double Value { get; private set; }

    public VariableInputDialog(string variableName)
    {
        InitializeComponent();
        VariableName = variableName;
        PromptText.Text = $"Enter value for variable '{variableName}':";
        ValueTextBox.Focus();
    }

    private void OkButton_Click(object sender, RoutedEventArgs e)
    {
        if (TryParseValue())
        {
            DialogResult = true;
            Close();
        }
    }

    private void ValueTextBox_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            if (TryParseValue())
            {
                DialogResult = true;
                Close();
            }
            e.Handled = true;
        }
    }

    private bool TryParseValue()
    {
        string input = ValueTextBox.Text.Trim();
        
        if (string.IsNullOrEmpty(input))
        {
            MessageBox.Show("Please enter a value.", "Input Required", 
                MessageBoxButton.OK, MessageBoxImage.Warning);
            return false;
        }

        if (double.TryParse(input, System.Globalization.NumberStyles.Any,
            System.Globalization.CultureInfo.InvariantCulture, out double value))
        {
            Value = value;
            return true;
        }

        MessageBox.Show("Please enter a valid number.", "Invalid Input", 
            MessageBoxButton.OK, MessageBoxImage.Warning);
        return false;
    }
}
