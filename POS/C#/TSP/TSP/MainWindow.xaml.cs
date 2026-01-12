using LinqToDB;
using LinqToDB.Data;
using LinqToDB.DataProvider.SQLite;
using LinqToDB.Mapping;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TSP;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private const double NorthLatitude = 55.1;
    private const double SouthLatitude = 45.7;
    private const double WestLongitude = 5.5;
    private const double EastLongitude = 17.2;

    private readonly string ConnectionString;

    private double mapImageWidth;
    private double mapImageHeight;

    public MainWindow()
    {
        InitializeComponent();

        var dbPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "natural-earth.sqlite");
        ConnectionString = $"Data Source={dbPath}";

        Loaded += MainWindow_Loaded;
        SizeChanged += MainWindow_SizeChanged;
    }

    private void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        UpdateMapAndMarkers();
    }

    private void MainWindow_Loaded(object sender, RoutedEventArgs e)
    {
        LoadMapAndPlotPlaces();
    }

    private void LoadMapAndPlotPlaces()
    {
        BitmapImage mapImage = new(new Uri("pack://application:,,,/Assets/DACH.png"));

        // Store original dimensions
        mapImageWidth = mapImage.PixelWidth;
        mapImageHeight = mapImage.PixelHeight;

        // Set up the image brush to stretch to fill
        ImageBrush mapBrush = new(mapImage)
        {
            Stretch = Stretch.Uniform
        };

        mapCanvas.Background = mapBrush;

        // Get places from database and plot them
        List<Place> places = GetPlacesFromDatabase();

        // Store places for replotting on resize
        mapCanvas.Tag = places;

        UpdateMapAndMarkers();
    }

    private void UpdateMapAndMarkers()
    {
        // Clear existing markers
        mapCanvas.Children.Clear();

        // Adjust canvas size to match the container
        mapCanvas.Width = mainContainer.ActualWidth;
        mapCanvas.Height = mainContainer.ActualHeight;

        // Calculate the actual display size of the image while maintaining aspect ratio
        double aspectRatio = mapImageWidth / mapImageHeight;
        double displayWidth, displayHeight;

        if (mapCanvas.Width / mapCanvas.Height > aspectRatio)
        {
            // Window is wider than the image
            displayHeight = mapCanvas.Height;
            displayWidth = displayHeight * aspectRatio;
        }
        else
        {
            // Window is taller than the image
            displayWidth = mapCanvas.Width;
            displayHeight = displayWidth / aspectRatio;
        }

        // Calculate offsets to center the image
        double xOffset = (mapCanvas.Width - displayWidth) / 2;
        double yOffset = (mapCanvas.Height - displayHeight) / 2;

        // Replot places
        List<Place> places = mapCanvas.Tag as List<Place>;
        if (places != null)
        {
            PlotPlacesOnMap(places, displayWidth, displayHeight, xOffset, yOffset);
        }
    }

    private List<Place> GetPlacesFromDatabase()
    {
        try
        {
            using var db = new DataConnection(
                SQLiteTools.GetDataProvider("SQLite"),
                ConnectionString);
            // Query the places table
            return [.. db.GetTable<Place>()];
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error connecting to database: {ex.Message}", "Database Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
            return [];
        }
    }

    private void PlotPlacesOnMap(List<Place> places, double displayWidth, double displayHeight, double xOffset, double yOffset)
    {
        foreach (var place in places)
        {
            // Skip if outside map boundaries
            if (place.Latitude > NorthLatitude || place.Latitude < SouthLatitude ||
                place.Longitude < WestLongitude || place.Longitude > EastLongitude)
                continue;

            // Convert geographic coordinates to canvas coordinates
            double x = MapLongitudeToX(place.Longitude, displayWidth) + xOffset;
            double y = MapLatitudeToY(place.Latitude, displayHeight) + yOffset;

            // Create a red circle for the place
            Ellipse placeMarker = new()
            {
                Width = 5,
                Height = 5,
                Fill = Brushes.Red,
                ToolTip = place.Name
            };

            // Position the circle on the canvas
            Canvas.SetLeft(placeMarker, x - placeMarker.Width / 2);
            Canvas.SetTop(placeMarker, y - placeMarker.Height / 2);

            // Add the circle to the canvas
            mapCanvas.Children.Add(placeMarker);
        }
    }

    private static double MapLongitudeToX(double longitude, double canvasWidth)
    {
        // Convert longitude to X coordinate
        return ((longitude - WestLongitude) / (EastLongitude - WestLongitude)) * canvasWidth;
    }

    private static double MapLatitudeToY(double latitude, double canvasHeight)
    {
        // Convert latitude to Y coordinate (note: Y increases downward)
        return ((NorthLatitude - latitude) / (NorthLatitude - SouthLatitude)) * canvasHeight;
    }
}

[Table("places")]
public class Place
{
    [Column("name")]
    public string Name { get; set; }

    [Column("latitude")]
    public double Latitude { get; set; }

    [Column("longitude")]
    public double Longitude { get; set; }
}
