using GraphControl;
using System.Windows;
using System.Windows.Media;

namespace IPGraph
{
    public partial class MainWindow : Window
    {
        private MyDb db;
        private Dictionary<long, Node> _routerMap = new();

        public MainWindow()
        {
            InitializeComponent();

            db = new("Data Source=Routing.db");

            var connections = db.Connections.ToList();
            var routers = db.Routers.ToList();

            foreach (var router in routers)
            {
                Node n = new();
                n.Colors.Add(Colors.Red);
                n.Text = router.City;

                graph.Nodes.Add(n);
                _routerMap[router.Id.Value] = n;
            }

            foreach (var connection in connections)
            {
                Edge e = new();
                e.First = _routerMap[connection.Endpoint1];
                e.Second = _routerMap[connection.Endpoint2];
                e.Text = connection.TransmissionTime.ToString();
                graph.Edges.Add(e);
            }
        }

        private void graph_StartChanged(object sender, RoutedEventArgs e)
        {
            if (graph.Start == null || graph.End == null) return;

            RunDijkstra();

            graph.Start = null;
            graph.End = null;
        }

        private void RunDijkstra()
        {
            List<Node> visited = [];
            Dictionary<Node, (double, Node)> tabelle = new();

            foreach (var node in graph.Nodes)
            {
                Node vorganger = null;
                double gewicht = double.MaxValue;
                if (node == graph.Start)
                {
                    gewicht = 0;
                    vorganger = graph.Start;
                }
                tabelle.Add(node, (gewicht, vorganger));
            }

            Node current = graph.Start;
            while (visited.Count < graph.Nodes.Count)
            {
                List<Edge> available = graph.Edges.Where(e => e.First == current || e.Second == current).ToList();
                foreach (var edge in available)
                {
                    if (visited.Contains(edge.First) && visited.Contains(edge.Second)) continue;
                    Node different = edge.First == current ? edge.Second : edge.First;
                    double gewicht = tabelle[current].Item1 + double.Parse(edge.Text);

                    if (tabelle[different].Item1 == double.MaxValue)
                    {
                        tabelle[different] = (gewicht, current);
                        continue;
                    }

                    if (tabelle[different].Item1 > gewicht)
                    {
                        tabelle[different] = (gewicht, current);
                        continue;
                    }
                }

                visited.Add(current);
                current = tabelle
                    .Where(kvp => !visited.Contains(kvp.Key))
                    .OrderBy(kvp => kvp.Value.Item1)
                    .FirstOrDefault().Key;
                if (current == null) break;
            }

            Dispatcher.Invoke(() =>
            {
                Node current = graph.End;
                (double, Node) first = tabelle[graph.End];
                Random rand = new();
                var color = Color.FromRgb((byte)rand.Next(0, 255), (byte)rand.Next(0, 255), (byte)rand.Next(0, 255));
                current.Colors.Add(color);
                graph.Start.Colors.Add(color);

                while (first.Item2 != graph.Start)
                {
                    first.Item2.Colors.Add(color);
                    first = tabelle[first.Item2];
                }
            });
        }
    }
}