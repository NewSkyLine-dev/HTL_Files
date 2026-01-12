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
using Schiffe_versenken.ViewModels;

namespace Schiffe_versenken
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly GameViewModel _viewModel;

        public MainWindow()
        {
            InitializeComponent();
            _viewModel = new GameViewModel();
            DataContext = _viewModel;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            
            // R-Taste zum Rotieren der Schiffe
            if (e.Key == Key.R && _viewModel.RotateShipCommand.CanExecute(null))
            {
                _viewModel.RotateShipCommand.Execute(null);
            }
        }
    }
}