using System.ComponentModel;
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

namespace WPF_Indicator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private int _minValue = 0;
        public int MinValue
        {
            get => _minValue;
            set
            {
                if (_minValue != value)
                {
                    _minValue = value;
                    OnPropertyChanged(nameof(MinValue));
                }
            }
        }

        private int _maxValue = 287;
        public int MaxValue
        {
            get => _maxValue;
            set
            {
                if (_maxValue != value)
                {
                    _maxValue = value;
                    OnPropertyChanged(nameof(MaxValue));
                    // Ensure CurrentValue stays within bounds
                    if (CurrentValue < value)
                        CurrentValue = value;
                }
            }
        }

        private int _currentValue = 0;
        public int CurrentValue
        {
            get => _currentValue;
            set
            {
                if (_currentValue != value)
                {
                    _currentValue = Math.Clamp(value, MinValue, MaxValue);
                    OnPropertyChanged(nameof(CurrentValue));
                }
            }
        }

        private int _imageSize = 1;

        public int ImageSize
        {
            get { return _imageSize; }
            set 
            { 
                if (_imageSize != value)
                {
                    _imageSize = value;
                    OnPropertyChanged(nameof(ImageSize));
                }
            }
        }


        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }
    }
}