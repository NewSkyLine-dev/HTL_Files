using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Achterbahn
{
    public class Passenger : INotifyPropertyChanged
    {
        public string Name { get; set; }
        private Rollercoaster rollercoaster;
        public Rollercoaster Rollercoaster
        {
            get => rollercoaster;
            set
            {
                rollercoaster = value;
                OnPropertyChanged();
            }
        }

        private Status status;
        private readonly Thread Thread;

        public Status Status
        {
            get { return status; }
            set { status = value; OnPropertyChanged(); }
        }

        public Passenger(string name)
        {
            Name = name;
            Thread = new Thread(Board) { Name = name };
        }

        private void Board()
        {
            while (true)
            {
                Status = Status.Waiting;

                // Wait for the ride to be ready for boarding
                Rollercoaster.RideReadyEvent.Wait();

                // Try to board the ride
                if (Rollercoaster.TryBoard())
                {
                    Status = Status.Boarding;
                    Thread.Sleep(1000);

                    Status = Status.Riding;

                    // Wait for ride completion
                    Rollercoaster.RideCompletedEvent.Wait();

                    Status = Status.Walking;
                    Thread.Sleep(200);
                }

                if (Rollercoaster.Status == RollercoasterStatus.Done)
                    break;
            }
        }

        public void Start()
        {
            Thread.Start();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    // Enum for the status of a passenger
    public enum Status
    {
        Waiting,
        Boarding,
        Riding,
        Walking
    }
}
