using System.ComponentModel;
using System.Threading;

namespace Drei_Raucher
{
    public class Smoker : INotifyPropertyChanged
    {
        private readonly Agent _agent;

        public Ingredients Ingredient { get; }
        public int WaitTime { get; set; }

        private Status _currentStatus;
        public Status CurrentStatus
        {
            get => _currentStatus;
            set { _currentStatus = value; OnPropertyChanged(); }
        }

        public Smoker(Ingredients ingredient, Agent agent)
        {
            Ingredient = ingredient;
            _agent = agent;
        }

        public void TryToSmoke()
        {
            while (true)
            {
                if (_agent.TryConsumeIngredients(Ingredient))
                {
                    CurrentStatus = Status.Smoking;
                    Thread.Sleep(WaitTime);
                    CurrentStatus = Status.Waiting;
                    _agent.NotifyAgent();
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
