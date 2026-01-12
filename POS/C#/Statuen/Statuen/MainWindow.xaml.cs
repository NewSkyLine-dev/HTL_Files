using System.ComponentModel;
using System.DirectoryServices;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Statuen;

public interface IQueensSolver
{
    public int[,] Board { get; set; }
    public int Solve();
}

public class NQueensSolverGenetic(int size) : IQueensSolver
{
    public int[,] Board { get; set; } = new int[size, size];

    public int Solve()
    {
        int populationSize = 100;
        int generationsCount = 1000;
        float mutationRate = 0.2f;
        float crossoverRate = 0.7f;
        int tournamentSize = 5;
        int eliteCount = 2;

        // Initialize population with random permutations
        var population = new List<int[]>(populationSize);
        for (int i = 0; i < populationSize; i++)
        {
            population.Add(GenerateRandomPermutation());
        }

        int[] bestSolution = [];
        float bestFitness = 0;

        // Main genetic algorithm loop
        for (int generation = 0; generation < generationsCount; generation++)
        {
            // Calculate fitness for each individual
            List<(int[] chromosome, float fitness)> populationWithFitness = [.. population.Select(p => (p, Fitness(p)))];

            // Find the best solution in this generation
            var (chromosome, fitness) = populationWithFitness.OrderByDescending(p => p.fitness).First();
            if (bestSolution == null || fitness > bestFitness)
            {
                bestSolution = (int[])chromosome.Clone();
                bestFitness = fitness;

                // If we found a perfect solution, we can stop
                if (Math.Abs(bestFitness - 1.0f) < 0.0001f)
                    break;
            }

            // Create a new population
            var newPopulation = new List<int[]>(populationSize);

            // Add elite individuals directly to the new population
            newPopulation.AddRange(populationWithFitness
                .OrderByDescending(p => p.fitness)
                .Take(eliteCount)
                .Select(p => (int[])p.chromosome.Clone()));

            // Fill the rest of the population with offspring
            while (newPopulation.Count < populationSize)
            {
                // Select parents using tournament selection
                int[] parent1 = TournamentSelection(populationWithFitness, tournamentSize);
                int[] parent2 = TournamentSelection(populationWithFitness, tournamentSize);

                // Create offspring through crossover and mutation
                int[] child;
                if (random.NextDouble() < crossoverRate)
                {
                    child = Crossover(parent1, parent2);
                }
                else
                {
                    child = (int[])parent1.Clone();
                }

                if (random.NextDouble() < mutationRate)
                {
                    Mutate(child);
                }

                newPopulation.Add(child);
            }

            // Replace old population with new one
            population = newPopulation;
        }

        // Place the best solution on the board
        PlaceQueens(bestSolution);

        // Return collisions (0 means a perfect solution)
        return CountCollisions(bestSolution);
    }

    private int[] GenerateRandomPermutation()
    {
        int[] permutation = Enumerable.Range(0, Board.GetLength(0)).ToArray();
        // Fisher-Yates shuffle
        for (int i = permutation.Length - 1; i > 0; i--)
        {
            int j = random.Next(i + 1);
            (permutation[i], permutation[j]) = (permutation[j], permutation[i]);
        }
        return permutation;
    }

    private static int[] TournamentSelection(List<(int[] chromosome, float fitness)> populationWithFitness, int tournamentSize)
    {
        // Select random individuals for the tournament
        var tournament = Enumerable.Range(0, tournamentSize)
            .Select(_ => populationWithFitness[random.Next(populationWithFitness.Count)])
            .ToList();

        // Return the fittest individual from the tournament
        return (int[])tournament.OrderByDescending(p => p.fitness).First().chromosome.Clone();
    }

    private static int[] Crossover(int[] parent1, int[] parent2)
    {
        int n = parent1.Length;
        int[] child = new int[n];
        Array.Fill(child, -1);  // Mark all positions as unfilled

        // Select a random segment from parent1
        int start = random.Next(n);
        int length = random.Next(1, n);
        int end = (start + length) % n;

        // Copy segment from parent1 to child
        if (start < end)
        {
            Array.Copy(parent1, start, child, start, length);
        }
        else
        {
            Array.Copy(parent1, start, child, start, n - start);
            Array.Copy(parent1, 0, child, 0, end);
        }

        // Fill remaining positions with values from parent2 in order
        int currentPos = end;
        for (int i = 0; i < n; i++)
        {
            // If this value from parent2 is not already in the child
            if (!child.Contains(parent2[i]))
            {
                // Find the next unfilled position
                while (child[currentPos] != -1)
                {
                    currentPos = (currentPos + 1) % n;
                }
                child[currentPos] = parent2[i];
            }
        }

        return child;
    }

    private static void Mutate(int[] chromosome)
    {
        // Swap mutation - swap two random positions
        int i = random.Next(chromosome.Length);
        int j = random.Next(chromosome.Length);
        (chromosome[i], chromosome[j]) = (chromosome[j], chromosome[i]);
    }

    private static int CountCollisions(int[] permutation)
    {
        int collisions = 0;
        int n = permutation.Length;
        for (int i = 0; i < n; i++)
        {
            for (int j = i + 1; j < n; j++)
            {
                if (Math.Abs(i - j) == Math.Abs(permutation[i] - permutation[j]))
                    collisions++;
            }
        }
        return collisions;
    }

    private void PlaceQueens(int[] permutation)
    {
        int n = permutation.Length;
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                Board[i, j] = (j == permutation[i]) ? 1 : 0;
            }
        }
    }

    private static readonly Random random = new();

    private static float Fitness(int[] permutation)
    {
        int n = permutation.Length;
        int collisions = 0;
        for (int i = 0; i < n; i++)
        {
            for (int j = i + 1; j < n; j++)
            {
                if (Math.Abs(i - j) == Math.Abs(permutation[i] - permutation[j]))
                    collisions++;
            }
        }
        return 1.0f / (1 + collisions);
    }
}

public class NQueensSolverTabu(int size) : IQueensSolver
{
    public int[,] Board { get; set; } = new int[size, size];
    private readonly int n = size;
    private static readonly Random random = new();

    public int Solve()
    {
        // Initialize with the identity permutation
        int[] currentState = [.. Enumerable.Range(0, n)];
        int currentCollisions = CountCollisions(currentState);

        // Copy original values for current state, since there is no other solution currently
        int[] bestState = (int[])currentState.Clone();
        int bestCollisions = currentCollisions;

        int tabuSize = 7;
        Queue<int[]> tabuQueue = new(tabuSize);
        int iterations = 100000;

        for (int iter = 0; iter < iterations && bestCollisions > 0; iter++)
        {
            int[] candidate = GenerateNeighbor(currentState, tabuQueue);
            int candidateCollisions = CountCollisions(candidate);

            // Update the best solution so far if improved.
            if (candidateCollisions < bestCollisions)
            {
                bestState = (int[])candidate.Clone();
                bestCollisions = candidateCollisions;
            }

            // Accept candidate move for diversification
            currentState = candidate;
            tabuQueue.Enqueue((int[])candidate.Clone());
            if (tabuQueue.Count > tabuSize)
                tabuQueue.Dequeue();
        }
        PlaceQueens(bestState);
        return CountCollisions(bestState);
    }

    private int[] GenerateNeighbor(int[] current, Queue<int[]> tabuQueue)
    {
        int[] neighbor;
        // Loop until a neighbor is generated that is not in the tabu list.
        // A maximum iteration count can be set here to prevent an infinite loop.
        int maxAttempts = 100;
        int attempts = 0;
        do
        {
            neighbor = (int[])current.Clone();
            int i = random.Next(n);
            int j = random.Next(n);
            while (j == i)
                j = random.Next(n);
            (neighbor[i], neighbor[j]) = (neighbor[j], neighbor[i]);
            attempts++;
        } while (IsTabu(neighbor, tabuQueue) && attempts < maxAttempts);

        return neighbor;
    }

    private static bool IsTabu(int[] candidate, Queue<int[]> tabuQueue)
    {
        // Check if a slice of the candidate is in the tabu list.
        foreach (var scandidate in tabuQueue)
        {
            if (scandidate.SequenceEqual(candidate))
                return true;
        }

        return false;
    }

    private int CountCollisions(int[] permutation)
    {
        int collisions = 0;
        for (int i = 0; i < n; i++)
        {
            for (int j = i + 1; j < n; j++)
            {
                if (Math.Abs(i - j) == Math.Abs(permutation[i] - permutation[j]))
                    collisions++;
            }
        }
        return collisions;
    }

    private void PlaceQueens(int[] permutation)
    {
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                Board[i, j] = (j == permutation[i]) ? 1 : 0;
            }
        }
    }
}

class NQueensSolverSimulatedAnnealing(int size) : IQueensSolver
{
    public int[,] Board { get; set; } = new int[size, size];
    private readonly int n = size;
    private static readonly Random random = new();

    public int Solve()
    {
        int[] currentState = [.. Enumerable.Range(0, n)];
        int currentCollision = CountCollisions(currentState);

        double temperature = 1.0;
        double coolingRate = 0.995;
        int iterations = 10000;

        for (int i = 0; i < iterations && currentCollision > 0; i++)
        {
            int[] newState = GenerateNeighbor(currentState);
            int newCollisions = CountCollisions(newState);

            if (AcceptanceProbability(currentCollision, newCollisions, temperature) > random.NextDouble())
            {
                currentState = newState;
                currentCollision = newCollisions;
            }

            temperature *= coolingRate;
        }

        PlaceQueens(currentState);
        return CountCollisions(currentState);
    }

    private int[] GenerateNeighbor(int[] current)
    {
        int[] neighbor = (int[])current.Clone();
        int i = random.Next(n);
        int j = random.Next(n);
        (neighbor[i], neighbor[j]) = (neighbor[j], neighbor[i]);
        return neighbor;
    }

    private static double AcceptanceProbability(int oldCollision, int newCollision, double temperature)
    {
        if (newCollision < oldCollision)
            return 1.0;
        return Math.Exp((oldCollision - newCollision) / temperature);
    }

    private int CountCollisions(int[] permutation)
    {
        int collisions = 0;
        for (int i = 0; i < n; i++)
        {
            for (int j = i + 1; j < n; j++)
            {
                if (Math.Abs(i - j) == Math.Abs(permutation[i] - permutation[j]))
                {
                    collisions++;
                }
            }
        }
        return collisions;
    }

    private void PlaceQueens(int[] permutation)
    {
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                Board[i, j] = (j == permutation[i]) ? 1 : 0;
            }
        }
    }
}

public struct State(int col, int[] positions)
{
    public int Col { get; set; } = col;
    public int[] Positions { get; set; } = positions;
}

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window, INotifyPropertyChanged
{
    private int statueNumbers = 10;
    public int StatueNumbers
    {
        get { return statueNumbers; }
        set { statueNumbers = value; NotifyPropertyChanged(); }
    }

    private string performance = string.Empty;
    public string Performance
    {
        get { return performance; }
        set { performance = value; NotifyPropertyChanged(); }
    }

    void SolveNQBitMask()
    {
        int n = statueNumbers;
        ulong allOnes = (1UL << n) - 1;
        int[] positions = new int[n];
        bool solutionFound = false;

        // Arrays to simulate recursion
        ulong[] colMasks = new ulong[n + 1];
        ulong[] leftDiagMasks = new ulong[n + 1];
        ulong[] rightDiagMasks = new ulong[n + 1];
        ulong[] safePositions = new ulong[n + 1];

        int colIndex = 0;
        colMasks[0] = 0;
        leftDiagMasks[0] = 0;
        rightDiagMasks[0] = 0;
        safePositions[0] = allOnes;

        while (colIndex >= 0)
        {
            if (safePositions[colIndex] != 0)
            {
                // Isolate the rightmost bit.
                ulong bit = safePositions[colIndex] & (ulong)-(long)safePositions[colIndex];
                safePositions[colIndex] -= bit;
                positions[colIndex] = BitOperations.TrailingZeroCount(bit);

                colMasks[colIndex + 1] = colMasks[colIndex] | bit;
                leftDiagMasks[colIndex + 1] = (leftDiagMasks[colIndex] | bit) << 1;
                rightDiagMasks[colIndex + 1] = (rightDiagMasks[colIndex] | bit) >> 1;

                // If all queens are placed, finish
                if (colMasks[colIndex + 1] == allOnes)
                {
                    solutionFound = true;
                    break;
                }
                colIndex++;
                safePositions[colIndex] = allOnes & ~(colMasks[colIndex] | leftDiagMasks[colIndex] | rightDiagMasks[colIndex]);
            }
            else
            {
                // Backtrack
                colIndex--;
            }
        }

        if (!solutionFound)
        {
            MessageBox.Show("Solution does not exist");
            return;
        }

        // Convert the one-dimensional solution into a 2D board.
        int[,] board = new int[n, n];
        for (int col = 0; col < n; col++)
        {
            int row = positions[col];
            board[row, col] = 1;
        }
        ColorQueens(board);
    }

    public MainWindow()
    {
        InitializeComponent();

        DataContext = this;

        for (int i = 0; i < statueNumbers; i++)
        {
            ContentArea.ColumnDefinitions.Add(new ColumnDefinition());
            ContentArea.RowDefinitions.Add(new RowDefinition());
        }

        // Fill the whole grid with textblocks
        for (int i = 0; i < statueNumbers; i++)
        {
            for (int j = 0; j < statueNumbers; j++)
            {
                Border border = new()
                {
                    BorderBrush = Brushes.Black,
                    BorderThickness = new Thickness(1)
                };

                TextBlock textBlock = new()
                {
                    Width = 50,
                    Height = 50,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                };

                border.Child = textBlock;
                Grid.SetRow(border, i);
                Grid.SetColumn(border, j);
                ContentArea.Children.Add(border);
            }
        }
    }

    void SolveBacktracking()
    {
        int[,] board = new int[statueNumbers, statueNumbers];

        if (!SolveBacktrackingUtil(board, 0))
        {
            MessageBox.Show("Solution does not exist");
            return;
        }

        ColorQueens(board);
    }

    private bool SolveBacktrackingUtil(int[,] board, int col)
    {
        // Base case: If all queens are placed, return true
        if (col >= statueNumbers)
            return true;

        // Consider this column and try placing this queen in all rows one by one
        for (int row = 0; row < statueNumbers; row++)
        {
            // Check if queen can be placed on board[row][col]
            if (IsSafe(board, row, col))
            {
                // Place this queen in board[row][col]
                board[row, col] = 1;

                // Recur to place rest of the queens
                if (SolveBacktrackingUtil(board, col + 1))
                    return true;

                // If placing queen in board[row][col] doesn'selectedItem lead to a solution,
                // then remove queen from board[row][col]
                board[row, col] = 0;
            }
        }

        // If queen can'selectedItem be placed in any row in this column col, return false
        return false;
    }

    private bool IsSafe(int[,] board, int row, int col)
    {
        int i, j;

        // Check this row on left side
        for (i = 0; i < col; i++)
            if (board[row, i] == 1)
                return false;

        // Check upper diagonal on left side
        for (i = row, j = col; i >= 0 && j >= 0; i--, j--)
            if (board[i, j] == 1)
                return false;

        // Check lower diagonal on left side
        for (i = row, j = col; j >= 0 && i < statueNumbers; i++, j--)
            if (board[i, j] == 1)
                return false;

        return true;
    }

    void ColorQueens(int[,] board)
    {
        for (int i = 0; i < statueNumbers; i++)
        {
            for (int j = 0; j < statueNumbers; j++)
            {
                if (board[i, j] == 1)
                {
                    Border border = new()
                    {
                        BorderBrush = Brushes.Black,
                        BorderThickness = new Thickness(1),
                        Background = Brushes.Green
                    };
                    TextBlock textBlock = new()
                    {
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center
                    };
                    border.Child = textBlock;
                    Grid.SetRow(border, i);
                    Grid.SetColumn(border, j);
                    ContentArea.Children.Add(border);
                }
            }
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    
    private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        // Reset the grid
        ContentArea.Children.Clear();
        ContentArea.ColumnDefinitions.Clear();
        ContentArea.RowDefinitions.Clear();
        for (int i = 0; i < statueNumbers; i++)
        {
            ContentArea.ColumnDefinitions.Add(new ColumnDefinition());
            ContentArea.RowDefinitions.Add(new RowDefinition());
        }
        
        // Fill the whole grid with textblocks
        for (int i = 0; i < statueNumbers; i++)
        {
            for (int j = 0; j < statueNumbers; j++)
            {
                Border border = new()
                {
                    BorderBrush = Brushes.Black,
                    BorderThickness = new Thickness(1)
                };
                TextBlock textBlock = new()
                {
                    Width = 50,
                    Height = 50,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                };
                border.Child = textBlock;
                Grid.SetRow(border, i);
                Grid.SetColumn(border, j);
                ContentArea.Children.Add(border);
            }
        }

        IQueensSolver solver;
        ComboBoxItem selectedItem = (ComboBoxItem)(AlgorithmCB.SelectedItem);

        if (selectedItem.Content.ToString() == "Simulated Anealing")
        {
            solver = new NQueensSolverSimulatedAnnealing(statueNumbers);
        }
        else if (selectedItem.Content.ToString() == "Tabu Search")
        {
            solver = new NQueensSolverTabu(statueNumbers);
        }
        else
        {
            solver = new NQueensSolverGenetic(statueNumbers);
        }

        int collisions = 0;
        StringBuilder sb = new();
        for (int i = 0; i < 100; i++)
        {
            int result = solver.Solve();
            // Solve N times and count collisions
            if (result > 0)
            {
                collisions += result;
                sb.AppendLine($"Solution {i + 1} has collisions");
            }

        }
        MessageBox.Show(sb.ToString());
        ColorQueens(solver.Board);
    }
}