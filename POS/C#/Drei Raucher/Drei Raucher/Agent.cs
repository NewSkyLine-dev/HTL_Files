using System.Collections.ObjectModel;
using System.Threading;

namespace Drei_Raucher
{
    public class Agent
    {
        private readonly object _lock = new();
        public ObservableCollection<Ingredients> Table { get; } = [];
        private static readonly Random _random = new();

        public void Run()
        {
            while (true)
            {
                WaitUntilIngredientsConsumed();
                PlaceRandomIngredients();
            }
        }

        public bool TryConsumeIngredients(Ingredients smokerIngredient)
        {
            lock (_lock)
            {
                if (Table.Contains(smokerIngredient)) return false;
                Table.Clear();
                Monitor.PulseAll(_lock);
                return true;
            }
        }

        private void PlaceRandomIngredients()
        {
            lock (_lock)
            {
                // Put 2 random ingredients on the table
                var randomIngredients = Enum.GetValues<Ingredients>().ToList()
                        .Where(ingredient => ingredient != (Ingredients)_random.NextInt64(0, 2))
                        .ToList();
                // Add all elements of randomIngredients to Table
                randomIngredients.ForEach(ing => Table.Add(ing));
                // Notify all smokers, that the ingredients are ready
                Monitor.PulseAll(_lock);
            }
        }

        public void NotifyAgent()
        {
            lock (_lock)
            {
                Table.Clear();
                Monitor.Pulse(_lock);  // Notify the agent to put new ingredients
            }
        }

        private void WaitUntilIngredientsConsumed()
        {
            lock (_lock)
            {
                while (Table.Count > 0)
                    Monitor.Wait(_lock);
            }
        }
    }
}
