using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PA2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {  
                string code = ProgramInput.Text;
                Tokenizer tknzr = new(code);
                var tokens = tknzr.Tokenize();
                ProgramExpression pe = new();
                pe.Parse(tokens);

                if (ProgramExpression.HasErrors())
                {
                    StringBuilder sb = new();
                    foreach (var err in ProgramExpression.GetErrors())
                    {
                        sb.AppendLine(err);
                    }
                    MessageBox.Show(sb.ToString(), "Errors", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                pe.Run(Painter);
            } 
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}