using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Achterbahn
{
    public class Rollercoaster : INotifyPropertyChanged
    {
        public ObservableCollection<Passenger> Queue { get; set; } = [];
        private RollercoasterStatus status = RollercoasterStatus.WaitingForPassengers;
        private readonly Thread Thread;
        public ManualResetEventSlim RideReadyEvent { get; private set; }
        public ManualResetEventSlim RideCompletedEvent { get; private set; }
        private CountdownEvent BoardEvent;

        private readonly object syncLock = new();

        public int PassengerCount { get; set; }
        private int carCapacity;
        public int CarCapacity
        {
            get { return carCapacity; }
            set
            {
                carCapacity = value;
                BoardEvent = new CountdownEvent(value);
            }
        }
        public int MaxRides { get; set; }
        public RollercoasterStatus Status
        {
            get { return status; }
            set { status = value; OnPropertyChanged(); }
        }

        public Rollercoaster()
        {
            // Initialize events in constructor
            RideReadyEvent = new ManualResetEventSlim(false);
            RideCompletedEvent = new ManualResetEventSlim(false);
            Thread = new(StartRides) { Name = "Rollercoaster" };
        }

        private void StartRides()
        {
            for (int i = 0; i < MaxRides; i++)
            {
                Status = RollercoasterStatus.WaitingForPassengers;

                // Signal that a new boarding cycle can begin
                RideReadyEvent.Set();

                // Wait until the wagon is full
                BoardEvent.Wait();

                // Stop accepting new passengers
                RideReadyEvent.Reset();

                Status = RollercoasterStatus.AllPassengers;
                Thread.Sleep(1000);

                Status = RollercoasterStatus.Riding;
                Thread.Sleep(2000);

                Status = RollercoasterStatus.PassengersLeaving;

                // Signal ride completion and wait for passengers to exit
                RideCompletedEvent.Set();
                Thread.Sleep(500);

                // Reset for next cycle
                RideCompletedEvent.Reset();
                BoardEvent.Reset();
            }

            Status = RollercoasterStatus.Done;
        }

        public void Start()
        {
            Thread.Start();
        }

        public bool TryBoard()
        {
            lock (syncLock)
            {
                if (Status == RollercoasterStatus.WaitingForPassengers && !BoardEvent.IsSet)
                {
                    BoardEvent.Signal();
                    return true;
                }
                return false;
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public enum RollercoasterStatus
    {
        WaitingForPassengers,
        AllPassengers,
        Riding,
        PassengersLeaving,
        Done
    }
}
