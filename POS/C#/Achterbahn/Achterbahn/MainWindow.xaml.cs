using System.Windows;

namespace Achterbahn
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Rollercoaster Rollercoaster { get; set; } = new();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            Rollercoaster.PassengerCount = (int)PassengerSlider.Value;
            Rollercoaster.CarCapacity = (int)CapacitySlider.Value;
            Rollercoaster.MaxRides = (int)RideSlider.Value;

            for (int i = 0; i < (int)PassengerSlider.Value; i++)
            {
                var passenger = new Passenger($"Passenger {i + 1}")
                {
                    Rollercoaster = Rollercoaster  // Set the Rollercoaster reference
                };
                Rollercoaster.Queue.Add(passenger);
                passenger.Start();
            }

            Rollercoaster.Start();
        }
    }
}