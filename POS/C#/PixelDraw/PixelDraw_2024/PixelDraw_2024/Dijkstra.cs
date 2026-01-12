using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace PixelDraw_2024
{
    public class Node
    {
        public bool IsVisited { get; set; }
        public bool IsAllowed { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public double Distance { get; set; } = double.MaxValue;
        public Node? Parent { get; set; }
    }

    public class Dijkstra
    {
        public Node Start { get; set; }
        public Node End { get; set; }
        public Node[,] Nodes { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public void Run()
        {
            Start.Distance = 0;

            // Priority queue to store nodes to visit
            //var open = new HashSet<Node> { Start };
            var open = new PriorityQueue<Node, double>();
            open.Enqueue(Start, Start.Distance);

            while (open.Count > 0)
            {
                var current = open.Dequeue();

                if (current.IsVisited) continue;
                current.IsVisited = true;

                // Debug view: Set pixel color based on distance
                int alpha = (int)(current.Distance % 255);
                Color debugColor = Color.FromArgb((byte)alpha, 0, 0, 255);
                MainWindow.SetPixel(debugColor, current.X, current.Y);

                // If we reached the end, reconstruct and draw the path
                if (current.X == End.X && current.Y == End.Y)
                {
                    break;
                }

                // Process all valid neighbors
                foreach (var neighbor in GetNeighbors(Nodes, current))
                {
                    if (neighbor.IsVisited || !neighbor.IsAllowed) continue;

                    // Calculate new distance to neighbor
                    double newDistance = current.Distance + 1; // Using 1 as cost between adjacent pixels

                    if (newDistance < neighbor.Distance)
                    {
                        neighbor.Distance = newDistance;
                        neighbor.Parent = current;
                        open.Enqueue(neighbor, newDistance);
                    }
                }
            }
        }

        private static IEnumerable<Node> GetNeighbors(Node[,] graph, Node current)
        {
            int[] dx = [-1, 1, 0, 0];
            int[] dy = [0, 0, -1, 1];

            for (int i = 0; i < 4; i++)
            {
                int newX = current.X + dx[i];
                int newY = current.Y + dy[i];

                // Check bounds
                if (newX >= 0 && newX < graph.GetLength(0) &&
                    newY >= 0 && newY < graph.GetLength(1))
                {
                    yield return graph[newX, newY];
                }
            }
        }
    }
}
