using Network;
using Server;
using System.Collections.ObjectModel;
using System.Net.Sockets;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MT;

public partial class MainWindow : Window
{
    private static double TOP = 48.42;
    private static double BOTTOM = 46.37;
    private static double LEFT = 9.53;
    private static double RIGHT = 17.16;
    private Transfer<MSG> transfer;
    private TcpClient client;
    public ObservableCollection<City> Places { get; set; } = [];
    public ObservableCollection<City> PermanentPlaces { get; set; } = [];

    public MainWindow()
    {
        InitializeComponent();
        DataContext = this;

        client = new("localhost", 12345);
        transfer = new(client);

        transfer.OnMessageReceived += ReceivedFunction;

        BitmapImage img = new(new Uri("pack://application:,,,/at.jpg"));
        cvMap.Background = new ImageBrush(img);
        cvMap.Width = img.Width;
        cvMap.Height = img.Height;
    }

    private void ReceivedFunction(object? sender, MSG e)
    {
        Dispatcher.Invoke(() =>
        {
            Places.Clear();

            var cities = e.Cities;

            foreach (var city in cities) Places.Add(city);

            UpdateMap();
        });
    }

    private void UpdateMap()
    {
        cvMap.Children.Clear();

        var selectedCity = cbPlaces.SelectedItem as City;
        var selectedCities = lbPlaces.SelectedItems.Cast<City>().ToList();

        Random rand = new();

        foreach (var city in Places)
        {
            if (!HasCoordinates(city))
                continue;

            var x = (city.Lng!.Value - LEFT) / (RIGHT - LEFT) * cvMap.Width;
            var y = (TOP - city.Lat!.Value) / (TOP - BOTTOM) * cvMap.Height;

            var color = Brushes.Red;

            if (selectedCity is not null && city == selectedCity)
                color = Brushes.Green;
            else if (selectedCities != null && selectedCities.Count == 2 && selectedCities.Contains(city))
                color = new(new() { R = (byte)rand.Next(0, 255), G = (byte)rand.Next(0, 255), B = (byte)rand.Next(0, 255), A = 255 });

            Rectangle rec = new() { Width = 10, Height = 10, Fill = color };
            Canvas.SetTop(rec, y);
            Canvas.SetLeft(rec, x);

            cvMap.Children.Add(rec);
        }
        DrawLines();
    }

    public static double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
    {
        var dLat = ToRadians(lat2 - lat1);
        var dLon = ToRadians(lon2 - lon1);

        var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2)) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

        var c = 2 * Math.Asin(Math.Min(1, Math.Sqrt(a)));
        return 6371.0 * c;
    }

    private static double ToRadians(double angle)
    {
        return Math.PI * angle / 180.0;
    }

    private static bool HasCoordinates(City city)
    {
        return city.Lat.HasValue && city.Lng.HasValue;
    }

    private List<List<City>> kMeans(int k)
    {
        List<City> centroids = new(k);
        int iterations = 0;

        // Wähle zufällige Centroiden aus
        Random rand = new();
        foreach (var city in PermanentPlaces.OrderBy(_ => rand.Next()).ToList().Take(k))
            centroids.Add(city);
        
        // Konvergiert bedeutet, dass sich die Daten nicht mehr ändern
        bool converged = false;

        List<List<City>> clusters = Enumerable.Range(0, k)
            .Select(_ => new List<City>())
            .ToList();

        while (!converged && iterations < 100)
        {
            // 1. erstelle k kluster
            clusters = Enumerable.Range(0, k)
                .Select(_ => new List<City>())
                .ToList();

            // 2. gehe jeden punkt durch
            for (int i = 0; i < PermanentPlaces.Count; i++)
            {
                var point = PermanentPlaces[i];

                var closestIndex = 0;
                var minDistance = CalculateDistance(point.Lat!.Value, point.Lng!.Value, centroids[0].Lat!.Value, centroids[0].Lng!.Value);

                for (int j = 1; j < k; j++)
                {
                    var d = CalculateDistance(point.Lat!.Value, point.Lng!.Value, centroids[j].Lat!.Value, centroids[j].Lng!.Value);
                    if (d < minDistance)
                    {
                        minDistance = d;
                        closestIndex = j;
                    }   
                }
                clusters[closestIndex].Add(point);
            }

            List<City> newCentroids = [];

            for (int i = 0; i < k; i++)
            {
                City newCentroid = calculateCentroid(clusters[i]);
                newCentroids.Add(newCentroid);
            }

            if (newCentroids == centroids) converged = true;
            else centroids = newCentroids;
            iterations++;
        }

        return clusters;
    }

    private City calculateCentroid(List<City> cities)
    {
        double meanLat = cities.Average(c => c.Lat!.Value);
        double meanLng = cities.Average(c => c.Lng!.Value);

        return new() { City1 = string.Empty, Lat = meanLat, Lng = meanLng };
    }

    public void DrawLines()
    {
        foreach (var city in PermanentPlaces)
        {
            foreach (var city2 in PermanentPlaces)
            {
                if (city == city2)
                    continue;

                if (!HasCoordinates(city2))
                    continue;

                var distance = CalculateDistance(city.Lat!.Value, city.Lng!.Value, city2.Lat!.Value, city2.Lng!.Value);

                if (distance <= 300)
                {
                    var line = new Polyline() { Stroke = Brushes.Orange, StrokeThickness = 2 };
                    var x1 = (city.Lng!.Value - LEFT) / (RIGHT - LEFT) * cvMap.Width;
                    var y1 = (TOP - city.Lat!.Value) / (TOP - BOTTOM) * cvMap.Height;

                    var x2 = (city2.Lng!.Value - LEFT) / (RIGHT - LEFT) * cvMap.Width;
                    var y2 = (TOP - city2.Lat!.Value) / (TOP - BOTTOM) * cvMap.Height;

                    line.Points.Add(new(x1, y1));
                    line.Points.Add(new(x2, y2));

                    cvMap.Children.Add(line);
                }
            }
        }
    }

    // Suchen
    private void Button_Click(object sender, RoutedEventArgs e)
    {
        string searchText = tbSearch.Text;
        if (string.IsNullOrEmpty(searchText))
            MessageBox.Show("Suchtext darf nicht leer sein!");

        transfer.Send(new() { SearchText = searchText });
    }


    private void cbPlaces_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        UpdateMap();
    }

    // Hinzufügen
    private void Button_Click_1(object sender, RoutedEventArgs e)
    {
        var selectedCity = cbPlaces.SelectedItem as City;

        if (selectedCity != null && !PermanentPlaces.Contains(selectedCity))
        {
            PermanentPlaces.Add(selectedCity);
            UpdateMap();
        }
        else
        {
            MessageBox.Show("Stadt ist schon enthalten!");
        }
    }

    private void lbPlaces_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        UpdateMap();
    }

    // Löschen
    private void Button_Click_2(object sender, RoutedEventArgs e)
    {
        var selectedCities = lbPlaces.SelectedItems.Cast<City>().ToList();

        foreach (var city in selectedCities)
        {
            PermanentPlaces.Remove(city);
        }

        UpdateMap();
    }

    // Cluster
    private void Button_Click_3(object sender, RoutedEventArgs e)
    {
        int k = int.Parse(tbKCluster.Text);

        if (k <= 1)
        {
            UpdateMap();
        }
        else
        {
            var clustered = kMeans(k);
            Random rand = new();

            foreach (var cluster in clustered)
            {
                SolidColorBrush color = new(new() { R = (byte)rand.Next(0, 255), G = (byte)rand.Next(0, 255), B = (byte)rand.Next(0, 255), A = 255 });

                foreach (var city in cluster)
                {
                    if (!HasCoordinates(city))
                        continue;

                    var x = (city.Lng!.Value - LEFT) / (RIGHT - LEFT) * cvMap.Width;
                    var y = (TOP - city.Lat!.Value) / (TOP - BOTTOM) * cvMap.Height;

                    Rectangle rec = new() { Width = 10, Height = 10, Fill = color };
                    Canvas.SetTop(rec, y);
                    Canvas.SetLeft(rec, x);

                    cvMap.Children.Add(rec);
                }
            }

            DrawLines();
        }
    }
}