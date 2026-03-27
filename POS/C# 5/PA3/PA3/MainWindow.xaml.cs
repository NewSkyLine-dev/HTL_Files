using Client;
using PA3.Network;
using System.Collections.ObjectModel;
using System.Net.Sockets;
using System.Numerics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PA3;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private TcpClient client;
    private Transfer<MSG> transfer;
    public ObservableCollection<Worldcity> Cities { get; set; } = [];
    public ObservableCollection<Worldcity> TspCities { get; set; } = [];

    public MainWindow()
    {
        InitializeComponent();
        DataContext = this;

        client = new("localhost", 12345);
        transfer = new(client);

        BitmapImage bitmap = new(new Uri("pack://application:,,,/Earthmap.jpg"));
        cvMap.Width = bitmap.PixelWidth;
        cvMap.Height = bitmap.PixelHeight;
        cvMap.Background = new ImageBrush(bitmap);

        transfer.OnMessageReceived += ReceivedMessage;
    }

    private void RunNearestNeighborTsp()
    {
        if (TspCities.Count == 0) return;

        var points = TspCities.Select(c => new { City = c, Point = new Vector2((float)c.Lng, (float)c.Lat) }).ToList();

        if (points.Count == 0) return;

        var tour = new List<int> { 0 };
        var visited = new HashSet<int> { 0 };

        while (visited.Count < points.Count)
        {
            var last = tour.Last();
            var lastPoint = points[last].Point;

            double minDist = double.MaxValue;
            int next = -1;

            for (int i = 0; i < points.Count; i++)
            {
                if (visited.Contains(i)) continue;

                double d = Measure(lastPoint, points[i].Point);
                if (d < minDist)
                {
                    minDist = d;
                    next = i;
                }
            }

            if (next == -1) break;

            tour.Add(next);
            visited.Add(next);
        }

        tour.Add(0);

        var polyLine = new Polyline
        {
            Stroke = Brushes.Blue,
            StrokeThickness = 2
        };

        foreach (int i in tour)
        {
            var city = points[i].City;
            double x = (city.Lng + 180) * (cvMap.Width / 360.0);
            double y = (90 - city.Lat) * (cvMap.Height / 180.0);
            polyLine.Points.Add(new Point(x, y));
        }

        cvMap.Children.Add(polyLine);
    }

    private void ReceivedMessage(object? sender, MSG e)
    {
        Dispatcher.Invoke(() =>
        {
            cvMap.Children.Clear();

            BitmapImage bitmap = new(new Uri("pack://application:,,,/Earthmap.jpg"));
            cvMap.Width = bitmap.PixelWidth;
            cvMap.Height = bitmap.PixelHeight;
            cvMap.Background = new ImageBrush(bitmap);

            foreach (var city in e.Cities)
            {
                Rectangle rect = new()
                {
                    Width = 10,
                    Height = 10,
                    Fill = Brushes.Red
                };
                Cities.Add(city);

                Canvas.SetLeft(rect, (city.Lng + 180) * (cvMap.Width / 360));
                Canvas.SetTop(rect, (90 - city.Lat) * (cvMap.Height / 180));

                cvMap.Children.Add(rect);
            }
        });
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        if (client.Connected)
        {
            string message = tbSearch.Text;
            transfer.Send(new MSG() { Name = message });
            Cities.Clear();
        }
    }

    private void Button_Click_1(object sender, RoutedEventArgs e)
    {
        var selectedCities = lbCities.SelectedItems.Cast<Worldcity>().ToList();
        if (selectedCities.Count > 0)
        {
            foreach (var city in selectedCities)
            {
                TspCities.Add(city);
            }
        }
    }



    double Measure(Vector2 p1, Vector2 p2)
    {
        var R = 6378.137; // Radius of earth in km

        var dLat = p2.Y * Math.PI / 180 - p1.Y * Math.PI / 180;

        var dLon = p2.X * Math.PI / 180 - p1.X * Math.PI / 180;

        var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) + Math.Cos(p1.Y * Math.PI / 180) * Math.Cos(p2.Y * Math.PI / 180) * Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

        return R * c; // km
    }

    private void Button_Click_2(object sender, RoutedEventArgs e)
    {
        RunNearestNeighborTsp();
    }
}