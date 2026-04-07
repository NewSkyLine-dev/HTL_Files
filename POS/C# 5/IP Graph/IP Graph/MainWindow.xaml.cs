using GraphControl;
using IP_Graph.DataModel;
using LinqToDB.Async;
using System.Windows;
using System.Windows.Media;

namespace IP_Graph
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Dictionary<Node, List<(Node neighbor, double weight)>> _adjacencyList = [];
        private readonly Dictionary<long, Node> _routerMap = [];

        public MainWindow()
        {
            InitializeComponent();
            Loaded += LoadData;
        }

        private async void LoadData(object sender, RoutedEventArgs e)
        {
            using var db = new Database("Data Source=Routing.db");

            var hasAnyRouters = await db.Routers.AnyAsync();

            if (hasAnyRouters)
            {
                var routers = await db.Routers.ToListAsync();
                var connections = await db.Connections.ToListAsync();

                Dispatcher.Invoke(() =>
                {
                    foreach (var router in routers)
                    {
                        var n = new Node();
                        n.Colors.Add(Colors.Red);
                        n.Text = router.City;
                        
                        _routerMap[router.Id.Value] = n;
                        _adjacencyList[n] = [];
                        graph.Nodes.Add(n);
                    }

                    foreach (var connection in connections)
                    {
                        if (_routerMap.TryGetValue(connection.Endpoint1, out var node1) &&
                            _routerMap.TryGetValue(connection.Endpoint2, out var node2))
                        {
                            var edge = new Edge
                            {
                                First = node1,
                                Second = node2,
                                Text = connection.TransmissionTime.ToString("0.0")
                            };
                            graph.Edges.Add(edge);

                            _adjacencyList[node1].Add((node2, connection.TransmissionTime));
                            _adjacencyList[node2].Add((node1, connection.TransmissionTime));
                        }
                    }
                });
            }
        }

        private void RunDijkstra()
        {
            if (graph.Start == null || graph.End == null) return;
            
            var startNode = graph.Start;
            var endNode = graph.End;

            var distances = new Dictionary<Node, double>();
            var previous = new Dictionary<Node, Node?>();
            var unvisited = new HashSet<Node>();

            foreach (var node in _adjacencyList.Keys)
            {
                distances[node] = double.MaxValue;
                previous[node] = null;
                unvisited.Add(node);
            }

            distances[startNode] = 0;

            while (unvisited.Count > 0)
            {
                var u = unvisited.OrderBy(n => distances[n]).First();
                unvisited.Remove(u);

                if (u == endNode) break; // found target

                foreach (var neighbor in _adjacencyList[u])
                {
                    if (!unvisited.Contains(neighbor.neighbor)) continue;

                    var alt = distances[u] + neighbor.weight;
                    if (alt < distances[neighbor.neighbor])
                    {
                        distances[neighbor.neighbor] = alt;
                        previous[neighbor.neighbor] = u;
                    }
                }
            }

            if (previous[endNode] != null || startNode == endNode)
            {
                var path = new List<Node>();
                var current = endNode;
                while (current != null)
                {
                    path.Add(current);
                    current = previous[current];
                }

                var r = new Random();
                var randomColor = Color.FromRgb((byte)r.Next(256), (byte)r.Next(256), (byte)r.Next(256));

                foreach (var node in path)
                {
                    node.Colors.Add(randomColor);
                }
            }

            graph.Start = null;
            graph.End = null;
        }

        private void test_StartChanged(object sender, RoutedEventArgs e)
        {
            RunDijkstra();
        }

        private void test_EndChanged(object sender, RoutedEventArgs e)
        {
            RunDijkstra();
        }
    }
}