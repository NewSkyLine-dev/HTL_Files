using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

namespace Taschenrechner;

/// <summary>
/// Dialog zur Eingabe von Variablenwerten.
/// </summary>
public partial class VariableInputDialog : Window
{
    public ObservableCollection<VariableInput> Variables { get; } = new();

    public VariableInputDialog(IEnumerable<char> variableNames)
    {
        InitializeComponent();

        foreach (var name in variableNames.OrderBy(c => c))
        {
            Variables.Add(new VariableInput(name));
        }

        VariablesPanel.ItemsSource = Variables;
    }

    public Dictionary<char, double> GetVariableValues()
    {
        var result = new Dictionary<char, double>();
        foreach (var variable in Variables)
        {
            if (variable.ParsedValue.HasValue)
            {
                result[variable.VariableName] = variable.ParsedValue.Value;
            }
        }
        return result;
    }

    private void OkButton_Click(object sender, RoutedEventArgs e)
    {
        // Validierung
        foreach (var variable in Variables)
        {
            if (!variable.ParsedValue.HasValue)
            {
                MessageBox.Show(
                    $"Bitte geben Sie einen gültigen Wert für Variable '{variable.Name}' ein.",
                    "Ungültige Eingabe",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }
        }

        DialogResult = true;
        Close();
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }
}

/// <summary>
/// Modell für eine Variableneingabe.
/// </summary>
public class VariableInput : INotifyPropertyChanged
{
    private string _value = "0";

    public char VariableName { get; }
    public string Name => $"{VariableName} =";

    public string Value
    {
        get => _value;
        set
        {
            _value = value;
            OnPropertyChanged(nameof(Value));
            OnPropertyChanged(nameof(ParsedValue));
        }
    }

    public double? ParsedValue
    {
        get
        {
            var normalized = _value.Replace(',', '.');
            if (double.TryParse(normalized,
                System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.InvariantCulture,
                out var result))
            {
                return result;
            }
            return null;
        }
    }

    public VariableInput(char name)
    {
        VariableName = name;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
