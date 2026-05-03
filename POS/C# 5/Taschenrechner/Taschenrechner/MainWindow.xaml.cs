using System.ComponentModel;
using System.Runtime.CompilerServices;
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

namespace Taschenrechner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private string _input = string.Empty;
        public string Input
        {
            get => _input;
            set
            {
                _input = value;
                OnPropertyChanged();
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        // Add
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string text = (string)((Button)sender).Content;
            Input += text;
        }

        // =
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                Context ctx = new();
                Parser p = new(Input);
                var expression = p.Parse();
                int result = expression.Interprete(ctx);
                Input = result.ToString();
            } 
            catch (Exception ex)
            {
                Input = ex.Message;
            }
        }

        // C
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Input = "";
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}