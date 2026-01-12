using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media.Animation;

namespace Primzahlengenerator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public int Iterations { get; set; } = 3;
        public ObservableCollection<long> PrimeNumbers { get; set; } = [];
        private int runningCalculations = 0;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            LoadingImage.Visibility = Visibility.Visible;

            Storyboard loadingAnimation = (Storyboard)FindResource("loadingRotation");
            loadingAnimation.Begin();

            Interlocked.Increment(ref runningCalculations);

            ThreadPool.QueueUserWorkItem(state =>
            {
                long output = GetPrime(Iterations);

                Dispatcher.Invoke(() =>
                {
                    PrimeNumbers.Add(output);

                    if (Interlocked.Decrement(ref runningCalculations) == 0)
                    {
                        loadingAnimation.Stop();
                        LoadingImage.Visibility = Visibility.Collapsed;
                    }
                });
            });
        }

        private static long GetPrime(int iteration)
        {
            int primeCount = 0;
            int number = 2;

            while (true)
            {
                bool isPrime = true;
                for (int i = 2; i <= Math.Sqrt(number); i++)
                {
                    if (number % i == 0)
                    {
                        isPrime = false;
                        break;
                    }
                }
                if (isPrime)
                {
                    primeCount++;
                    if (primeCount == iteration)
                    {
                        return number;
                    }
                }

                number++;
            }
        }
    }
}