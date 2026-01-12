using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Waldwunder_Verwaltung.Views;

/// <summary>
/// Interaction logic for MapControl.xaml
/// </summary>
public partial class MapControl : UserControl
{
    private const float MAP_MIN_LONGITUDE = 9.362383f;
    private const float MAP_MAX_LONGITUDE = 17.231941f;
    private const float MAP_MIN_LATITUDE = 46.308597f;
    private const float MAP_MAX_LATITUDE = 49.063175f;

    private readonly Dictionary<Ellipse, int> _markerToWonderId = new();

    public event EventHandler<int>? MarkerClicked;

    private ObservableCollection<DataModel.Waldwunder> _wonders = new();

    public ObservableCollection<DataModel.Waldwunder> Wonders
    {
        get => _wonders;
        set
        {
            _wonders = value;
            UpdateMarkers();
        }
    }

    public MapControl()
    {
        InitializeComponent();
        var img = mapImage;
        if (img != null)
            img.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/map.png"));
        DataContextChanged += MapControl_DataContextChanged;
        SizeChanged += MapControl_SizedChanged;
    }

    private void MapControl_SizedChanged(object sender, SizeChangedEventArgs e)
    {
        UpdateMarkers();
    }

    private void MapControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        if (e.NewValue is MainViewModel viewMode)
        {
            Wonders = viewMode.Wonders;
            Wonders.CollectionChanged += (s, args) => UpdateMarkers();
        }
    }

    private void UpdateMarkers()
    {
        mapCanvas.Children.Clear();
        mapCanvas.Children.Add(mapImage);
        _markerToWonderId.Clear();

        if (Wonders == null)
            return;

        foreach (var wonder in Wonders)
        {
            var canvasPoint = GeoToCanvas(wonder.Latitude ?? 0, wonder.Longitude ?? 0);

            var marker = new Ellipse
            {
                Width = 10,
                Height = 10,
                Fill = Brushes.Red,
                Stroke = Brushes.White,
                StrokeThickness = 2,
                ToolTip = wonder.Name
            };

            Canvas.SetLeft(marker, canvasPoint.X - marker.Width / 2);
            Canvas.SetTop(marker, canvasPoint.Y - marker.Height / 2);

            // Track marker and wonder ID
            _markerToWonderId[marker] = (int)wonder.Id;

            // Add click handler
            marker.MouseLeftButtonDown += Marker_MouseLeftButtonDown;

            // Add to canvas
            mapCanvas.Children.Add(marker);
        }
    }

    private void Marker_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (sender is Ellipse marker && _markerToWonderId.TryGetValue(marker, out int wonderId))
        {
            MarkerClicked?.Invoke(this, wonderId);
        }
    }

    private Point GeoToCanvas(decimal latitude, decimal longitude)
    {
        double relX = ((float)longitude - MAP_MIN_LONGITUDE) / (MAP_MAX_LONGITUDE - MAP_MIN_LONGITUDE);
        double relY = 1 - ((float)latitude - MAP_MIN_LATITUDE) / (MAP_MAX_LATITUDE - MAP_MIN_LATITUDE);

        double canvasX = relX * mapCanvas.ActualWidth;
        double canvasY = relY * mapCanvas.ActualHeight;

        return new Point(canvasX, canvasY);
    }

    public void SelectMarkerByWonderId(int wonderId)
    {
        // Find the marker for the given wonderId and visually highlight it
        foreach (var kvp in _markerToWonderId)
        {
            if (kvp.Value == wonderId)
            {
                kvp.Key.Stroke = Brushes.Yellow;
                kvp.Key.StrokeThickness = 4;
            }
            else
            {
                kvp.Key.Stroke = Brushes.White;
                kvp.Key.StrokeThickness = 2;
            }
        }
    }
}
