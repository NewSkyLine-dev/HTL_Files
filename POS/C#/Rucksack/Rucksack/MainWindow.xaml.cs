using System.Windows;

namespace Rucksack
{
    public class Item(int size, int value)
    {
        public int Size { get; } = size;
        public int Value { get; } = value;
    }

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            SolveKnapsackProblem();
        }

        private void SolveKnapsackProblem()
        {
            int capacity = 170;
            List<Item> items =
            [
                new Item(41, 442),
                new Item(50, 525),
                new Item(49, 511),
                new Item(59, 593),
                new Item(55, 546),
                new Item(57, 564),
                new Item(60, 617)
            ];

            var (maxValue, selectedItems) = Knapsack(items, capacity);

            // Ergebnisse anzeigen
            ResultTextBlock.Text = $"Maximaler Wert: {maxValue} Gold\n\nAusgewählte Gegenstände:\n";
            foreach (var index in selectedItems)
            {
                ResultTextBlock.Text += $"Gegenstand {index + 1}: {items[index].Size} Plätze, {items[index].Value} Gold\n";
            }
        }

        private static (int maxValue, List<int> selectedItems) Knapsack(List<Item> items, int capacity)
        {
            // Erstelle DP-Tabelle [Anzahl Items + 1, Kapazität + 1]
            int[,] knapsackTable = new int[items.Count + 1, capacity + 1];

            // Fülle die DP-Tabelle
            for (int i = 1; i <= items.Count; i++)
            {
                for (int currentWeight = 1; currentWeight <= capacity; currentWeight++)
                {
                    if (items[i - 1].Size <= currentWeight)
                    {
                        // Maximiere: Entweder aktuellen Gegenstand nehmen oder weglassen
                        knapsackTable[i, currentWeight] = Math.Max(
                            knapsackTable[i - 1, currentWeight],  // ohne aktuellen Gegenstand
                            knapsackTable[i - 1, currentWeight - items[i - 1].Size] + items[i - 1].Value  // mit aktuellem Gegenstand
                        );
                    }
                    else
                    {
                        // Gegenstand ist zu groß, übernehme Wert ohne diesen Gegenstand
                        knapsackTable[i, currentWeight] = knapsackTable[i - 1, currentWeight];
                    }
                }
            }

            // Rekonstruiere die optimale Lösung
            List<int> selected = [];
            int totalWeight = capacity;
            for (int i = items.Count; i > 0; i--)
            {
                if (knapsackTable[i, totalWeight] != knapsackTable[i - 1, totalWeight])
                {
                    selected.Add(i - 1);  // Gegenstand wurde in optimaler Lösung verwendet
                    totalWeight -= items[i - 1].Size;
                }
            }
            selected.Reverse();  // Stelle ursprüngliche Reihenfolge wieder her

            return (knapsackTable[items.Count, capacity], selected);
        }
    }
}