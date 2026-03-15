using LinqToDB;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Osterhasen_Hilfsprogramm;

public partial class MainWindow : Window
{
    private readonly Database _database;
    public double CanvasWidth { get; set; }
    public double CanvasHeight { get; set; }
    public new double Left { get; } = 16.209652;
    public new double Top { get; } = 47.846533;
    public double Right { get; } = 16.281017;
    public double Bottom { get; } = 47.786898;

    private class ClusterData
    {
        public List<Person> Points { get; set; } = [];
        public SolidColorBrush Color { get; set; } = Brushes.Black;
        public Person? StartPoint { get; set; }
    }
    private readonly List<ClusterData> _currentClusters = new();

    public MainWindow()
    {
        InitializeComponent();
        DataContext = this;

        var bitmap = new BitmapImage(new Uri("pack://application:,,,/Images/Stadtplan-Wiener-Neustadt.jpg"));
        CanvasWidth = bitmap.PixelWidth;
        CanvasHeight = bitmap.PixelHeight;

        cvMap.MouseLeftButtonDown += (s, arg) =>
        {
            arg.Handled = true;
            var pos = arg.GetPosition(cvMap);

            // Create person when clicked on empty area
            if (cvMap.Children.OfType<Rectangle>().All(r =>
                !(pos.X >= Canvas.GetLeft(r) && pos.X <= Canvas.GetLeft(r) + r.Width &&
                  pos.Y >= Canvas.GetTop(r) && pos.Y <= Canvas.GetTop(r) + r.Height)))
            {
                double longitude = Left + (pos.X / CanvasWidth) * (Right - Left);
                double latitude = Top - (pos.Y / CanvasHeight) * (Top - Bottom);
                tbXCoord.Text = longitude.ToString(CultureInfo.InvariantCulture);
                tbYCoord.Text = latitude.ToString(CultureInfo.InvariantCulture);
            }
        };

        _database = Database.Instance;
        _database.CreateTable<Person>();
        FirstDraw();
    }

    private static List<List<Person>> KMeans(int k, List<Person> points)
    {
        if (points.Count < k)
            return [points];

        var random = new Random();
        var centroids = points.OrderBy(_ => random.Next()).Take(k).Select(p => new Person
        {
            Latitude = p.Latitude,
            Longitude = p.Longitude
        }).ToList();

        var converged = false;
        var maxIterations = 100;
        var iteration = 0;

        List<List<Person>> clusters = [];

        while (!converged && iteration < maxIterations)
        {
            clusters = Enumerable.Range(0, k).Select(_ => new List<Person>()).ToList();

            foreach (var point in points)
            {
                var closestIndex = 0;
                var minDistance = Distance(point, centroids[0]);

                for (int j = 1; j < k; j++)
                {
                    var d = Distance(point, centroids[j]);
                    if (d < minDistance)
                    {
                        minDistance = d;
                        closestIndex = j;
                    }
                }
                clusters[closestIndex].Add(point);
            }

            var newCentroids = new List<Person>();
            converged = true;

            for (int i = 0; i < k; i++)
            {
                if (clusters[i].Count > 0)
                {
                    var newCentroid = CalculateCentroid(clusters[i]);
                    newCentroids.Add(newCentroid);

                    if (Distance(newCentroid, centroids[i]) > 0.0001)
                        converged = false;
                }
                else
                {
                    newCentroids.Add(centroids[i]);
                }
            }

            centroids = newCentroids;
            iteration++;
        }

        return clusters;
    }

    private static double Distance(Person p1, Person p2)
    {
        return Math.Sqrt(Math.Pow(p1.Latitude - p2.Latitude, 2) + Math.Pow(p1.Longitude - p2.Longitude, 2));
    }

    private static Person CalculateCentroid(List<Person> cluster)
    {
        var latitude = cluster.Average(p => p.Latitude);
        var longitude = cluster.Average(p => p.Longitude);
        return new Person { Latitude = latitude, Longitude = longitude };
    }

    private void Save_Button(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(tbName.Text))
        {
            MessageBox.Show("Bitte geben Sie einen Namen ein.");
            return;
        }

        if (!double.TryParse(tbXCoord.Text.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out double xCoord) || !double.TryParse(tbYCoord.Text.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out double yCoord))
        {
            MessageBox.Show("Keine oder ungültige Koordinaten eingegeben.");
            return;
        }

        if (xCoord < Left || xCoord > Right || yCoord < Bottom || yCoord > Top)
        {
            MessageBox.Show("Koordinaten außerhalb gültigem Bereich.");
            return;
        }

        var person = new Person
        {
            Name = tbName.Text,
            Latitude = yCoord,
            Longitude = xCoord
        };
        _database.Insert(person);
        MessageBox.Show($"Person '{person.Name}' erfolgreich hinzugefügt.");

        double posX = (xCoord - Left) / (Right - Left) * CanvasWidth;
        double posY = (Top - yCoord) / (Top - Bottom) * CanvasHeight;
        drawBox(posX, posY);

        tbName.Clear();
        tbXCoord.Clear();
        tbYCoord.Clear();
    }

    private void FirstDraw()
    {
        var people = _database.Select<Person>();
        foreach (var person in people)
        {
            double posX = (person.Longitude - Left) / (Right - Left) * CanvasWidth;
            double posY = (Top - person.Latitude) / (Top - Bottom) * CanvasHeight;
            drawBox(posX, posY);
        }
    }

    private void drawBox(double posX, double posY)
    {
        Rectangle rect = new()
        {
            Width = 40,
            Height = 40,
            Stroke = Brushes.Red,     
            StrokeThickness = 3,
            Fill = new SolidColorBrush(Colors.Yellow)
        };
        Canvas.SetLeft(rect, posX);
        Canvas.SetTop(rect, posY);
        cvMap.Children.Add(rect);
    }

    private void Cluster_Button(object sender, RoutedEventArgs e)
    {
        var people = _database.Select<Person>();
        if (people.Count == 0)
        {
            MessageBox.Show("Keine Personen vorhanden zum Clustern.");
            return;
        }
        if (!int.TryParse(tbHelperCount.Text, out int k) || k < 1)
        {
            MessageBox.Show("Bitte gültige Anzahl an Helfern eingeben.");
            return;
        }

        cvMap.Children.Clear();
        _currentClusters.Clear();

        var image = new Image
        {
            Source = new BitmapImage(new Uri("pack://application:,,,/Images/Stadtplan-Wiener-Neustadt.jpg")),
            Stretch = Stretch.UniformToFill
        };
        Canvas.SetLeft(image, 0);
        Canvas.SetTop(image, 0);
        cvMap.Children.Add(image);

        var clusters = KMeans(k, people);

        foreach (var cluster in clusters)
        {
            var color = new SolidColorBrush(Color.FromRgb((byte)Random.Shared.Next(256), (byte)Random.Shared.Next(256), (byte)Random.Shared.Next(256)));

            _currentClusters.Add(new ClusterData { Points = cluster, Color = color });

            foreach (var person in cluster)
            {
                double posX = (person.Longitude - Left) / (Right - Left) * CanvasWidth;
                double posY = (Top - person.Latitude) / (Top - Bottom) * CanvasHeight;
                Rectangle rect = new()
                {
                    Width = 40,
                    Height = 40,
                    Stroke = color,
                    StrokeThickness = 3,
                    Fill = color
                };
                rect.MouseLeftButtonDown += (s, args) =>
                {
                    args.Handled = true;

                    var unassignedClusters = _currentClusters.Where(c => c.StartPoint == null).ToList();
                    if (unassignedClusters.Count == 0)
                    {
                        MessageBox.Show("Alle Helfer haben bereits einen Startpunkt.");
                        return;
                    }

                    var startPos = new Person { Latitude = person.Latitude, Longitude = person.Longitude, Name = person.Name };

                    var personCluster = _currentClusters.FirstOrDefault(c => c.Points.Contains(person));
                    if (personCluster == null || personCluster.StartPoint != null) return;

                    personCluster.StartPoint = startPos;
                    DrawShortestPath(personCluster);
                };
                Canvas.SetLeft(rect, posX);
                Canvas.SetTop(rect, posY);
                cvMap.Children.Add(rect);
            }
        }
    }

    private void DrawShortestPath(ClusterData cluster)
    {
        if (cluster.StartPoint == null || cluster.Points.Count == 0) return;

        var unvisited = new List<Person>(cluster.Points);
        var current = cluster.StartPoint;
        var path = new List<Person> { current };

        while (unvisited.Count > 0)
        {
            var next = unvisited.OrderBy(p => Distance(current, p)).First();
            path.Add(next);
            unvisited.Remove(next);
            current = next;
        }

        for (int i = 0; i < path.Count - 1; i++)
        {
            var p1 = path[i];
            var p2 = path[i + 1];

            double x1 = (p1.Longitude - Left) / (Right - Left) * CanvasWidth;
            double y1 = (Top - p1.Latitude) / (Top - Bottom) * CanvasHeight;
            double x2 = (p2.Longitude - Left) / (Right - Left) * CanvasWidth;
            double y2 = (Top - p2.Latitude) / (Top - Bottom) * CanvasHeight;

            Line line = new()
            {
                X1 = x1,
                Y1 = y1,
                X2 = x2,
                Y2 = y2,
                Stroke = cluster.Color,
                StrokeThickness = 3,
                Opacity = 0.7
            };
            Panel.SetZIndex(line, 1);
            cvMap.Children.Add(line);
        }
    }
}