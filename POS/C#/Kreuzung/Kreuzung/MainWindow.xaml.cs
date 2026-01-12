using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Threading;

namespace Kreuzung
{
    public partial class MainWindow : Window
    {
        private Crossroad crossroad;
        public ObservableCollection<Car> EastCars { get; set; } = [];
        public ObservableCollection<Car> NorthCars { get; set; } = [];
        public ObservableCollection<Car> SouthCars { get; set; } = [];
        public ObservableCollection<Car> WestCars { get; set; } = [];
        public ObservableCollection<Car> MiddleCars { get; set; } = [];

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            crossroadComboBox.ItemsSource = new List<string> { "Crossroad", "Large Crossroad", "Traffic Light Crossroad" };
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            crossroad = crossroadComboBox.SelectedItem switch
            {
                "Crossroad" => new Crossroad(),
                "Large Crossroad" => new LargeCrossroad(),
                "Traffic Light Crossroad" => new TrafficLightCrossroad(),
                _ => throw new NotImplementedException()
            };

            int carCount = int.Parse(carCountTextBox.Text);
            for (int i = 0; i < carCount; i++)
            {
                Car car = new(this) { Id = i+1 };
                AddCarToDirectionList(car);
                Thread carThread = new(() => car.Drive(crossroad)) { Name = $"Car {i+1}"};
                carThread.Start();
            }
        }

        public void UpdateCarStatus(Car car, string status)
        {
            Dispatcher.Invoke(() =>
            {
                car.Status = status;
                if (status == "Crossing")
                {
                    RemoveCarFromDirectionList(car);
                    MiddleCars.Add(car);
                }
                else if (status == "Finished")
                {
                    MiddleCars.Remove(car);
                    switch (car.Direction)
                    {
                        case Direction.North:
                            NorthCars.Remove(car);
                            SouthCars.Add(car);
                            car.Direction = Direction.South;
                            break;
                        case Direction.South:
                            SouthCars.Remove(car);
                            NorthCars.Add(car);
                            car.Direction = Direction.North;
                            break;
                        case Direction.East:
                            EastCars.Remove(car);
                            WestCars.Add(car);
                            car.Direction = Direction.West;
                            break;
                        case Direction.West:
                            WestCars.Remove(car);
                            EastCars.Add(car);
                            car.Direction = Direction.East;
                            break;
                    }
                }
            });
        }

        private void AddCarToDirectionList(Car car)
        {
            Dispatcher.Invoke(() =>
            {
                GetCarCollectionByDirection(car.Direction).Add(car);
            });
        }

        private void RemoveCarFromDirectionList(Car car)
        {
            Dispatcher.Invoke(() =>
            {
                GetCarCollectionByDirection(car.Direction).Remove(car);
            });
        }

        private ObservableCollection<Car> GetCarCollectionByDirection(Direction direction)
        {
            return direction switch
            {
                Direction.North => NorthCars,
                Direction.South => SouthCars,
                Direction.East => EastCars,
                Direction.West => WestCars,
                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
            };
        }
    }
}
