using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Kreuzung
{
    public class Car : INotifyPropertyChanged
    {
        public MainWindow mainWindow;
        public int Id { get; set; }
        public Direction Direction { get; set; }
        private string status;
        public string Status
        {
            get => status;
            set
            {
                status = value;
                OnPropertyChanged();
            }
        }

        public Car(MainWindow mw)
        {
            Direction = GetRandomDirection();
            Status = "Driving";
            mainWindow = mw;
        }

        private static Direction GetRandomDirection()
        {
            Array values = Enum.GetValues(typeof(Direction));
            return (Direction)values.GetValue(new Random().Next(values.Length));
        }

        public void Drive(Crossroad crossroad)
        {
            Thread.Sleep(new Random().Next(1000, 10000));
            mainWindow.UpdateCarStatus(this, "Crossing");
            crossroad.Cross(this);
            mainWindow.UpdateCarStatus(this, "Finished");
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    public enum Direction
    {
        North,
        South,
        East,
        West
    }
}
