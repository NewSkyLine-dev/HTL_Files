using System.Windows;

namespace WeckerLibrary
{
    /// <summary>
    /// Interaction logic for TimerSettingsDialog.xaml
    /// </summary>
    public partial class TimerSettingsDialog : Window
    {
        public int Minutes { get; private set; }
        public int Seconds { get; private set; }

        public TimerSettingsDialog()
        {
            InitializeComponent();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(minutesTextBox.Text, out int min) &&
                int.TryParse(secondsTextBox.Text, out int sec))
            {
                Minutes = Math.Max(0, min);
                Seconds = Math.Max(0, Math.Min(59, sec));
                DialogResult = true;
            }
            else
            {
                MessageBox.Show(
                    "Bitte geben Sie gültige Zahlen ein.",
                    "Ungültige Eingabe",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
