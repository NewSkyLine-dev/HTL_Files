using System.Windows;

namespace TerminControlLibrary
{
    /// <summary>
    /// Interaction logic for ChangeDialogNew.xaml
    /// </summary>
    public partial class ChangeDialogNew : Window
    {
        public ChangeDialogNew()
        {
            InitializeComponent();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
