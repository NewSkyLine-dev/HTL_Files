using GraphControl;
using IP_Graph.DataModel;
using LinqToDB.Async;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace IP_Graph
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
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

                foreach (var router in routers)
                {
                    Dispatcher.Invoke(() =>
                    {
                        var n = new Node();
                        n.Colors.Add(Colors.Red);
                        n.Text = router.City;
                        graph.Nodes.Add(n);
                    });
                }
            }
        }

        private void Dijkstra()
        {

        }

        private void Initialize(List<Node> graph, Node startknoten, Dictionary<Node, int> distance, List<Node> previous, List<Router> q)
        {
            foreach (var node in graph)
            {
                distance[node] = int.MaxValue;
                previous[node] = null;
            }
            distance[startknoten] = 0;
        }

        private void test_StartChanged(object sender, RoutedEventArgs e)
        {
            
        }

        private void test_EndChanged(object sender, RoutedEventArgs e)
        {

        }
    }
}