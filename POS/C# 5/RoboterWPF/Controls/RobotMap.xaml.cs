using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using RoboterWPF.Models;

namespace RoboterWPF.Controls;

public partial class RobotMap : UserControl
{
    private int _width = 10;
    private int _height = 10;
    private List<RobotElement> _elements = [];

    public RobotMap()
    {
        InitializeComponent();
        SizeChanged += OnSizeChanged;
    }

    private void OnSizeChanged(object sender, SizeChangedEventArgs e)
    {
        Render();
    }

    public void SetSize(int width, int height)
    {
        _width = width;
        _height = height;
        Render();
    }

    public void SetElements(List<RobotElement> elements)
    {
        _elements = elements;
        Render();
    }

    public List<RobotElement> GetElements()
    {
        return _elements;
    }

    public int GridWidth => _width;
    public int GridHeight => _height;

    public void ClearLevel()
    {
        _elements.Clear();
        Render();
    }

    public void Render()
    {
        MapCanvas.Children.Clear();

        double canvasWidth = MapCanvas.ActualWidth;
        double canvasHeight = MapCanvas.ActualHeight;

        if (canvasWidth <= 0 || canvasHeight <= 0)
        {
            canvasWidth = ActualWidth > 0 ? ActualWidth : 400;
            canvasHeight = ActualHeight > 0 ? ActualHeight : 400;
        }

        double cellWidth = canvasWidth / _width;
        double cellHeight = canvasHeight / _height;
        double cellSize = Math.Min(cellWidth, cellHeight);

        // Calculate offset to center the grid
        double offsetX = (canvasWidth - (cellSize * _width)) / 2;
        double offsetY = (canvasHeight - (cellSize * _height)) / 2;

        // Draw grid background
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                var rect = new Rectangle
                {
                    Width = cellSize,
                    Height = cellSize,
                    Fill = Brushes.LightGray,
                    Stroke = Brushes.DarkGray,
                    StrokeThickness = 1
                };
                Canvas.SetLeft(rect, offsetX + x * cellSize);
                Canvas.SetTop(rect, offsetY + y * cellSize);
                MapCanvas.Children.Add(rect);
            }
        }

        // Draw elements
        foreach (var element in _elements)
        {
            DrawElement(element, cellSize, offsetX, offsetY);
        }
    }

    private void DrawElement(RobotElement element, double cellSize, double offsetX, double offsetY)
    {
        double x = offsetX + element.X * cellSize;
        double y = offsetY + element.Y * cellSize;
        double margin = cellSize * 0.1;

        switch (element.Type)
        {
            case ElementType.Robot:
                DrawRobot(x, y, cellSize, margin);
                break;
            case ElementType.Obstacle:
                DrawObstacle(x, y, cellSize, margin);
                break;
            case ElementType.Wall:
                DrawWall(x, y, cellSize);
                break;
            case ElementType.Letter:
                DrawLetter(x, y, cellSize, element.Letter);
                break;
        }
    }

    private void DrawRobot(double x, double y, double cellSize, double margin)
    {
        // Robot body (blue circle)
        var body = new Ellipse
        {
            Width = cellSize - 2 * margin,
            Height = cellSize - 2 * margin,
            Fill = Brushes.DodgerBlue,
            Stroke = Brushes.DarkBlue,
            StrokeThickness = 2
        };
        Canvas.SetLeft(body, x + margin);
        Canvas.SetTop(body, y + margin);
        MapCanvas.Children.Add(body);

        // Robot eyes
        double eyeSize = cellSize * 0.15;
        double eyeOffset = cellSize * 0.15;

        var leftEye = new Ellipse
        {
            Width = eyeSize,
            Height = eyeSize,
            Fill = Brushes.White
        };
        Canvas.SetLeft(leftEye, x + cellSize / 2 - eyeOffset - eyeSize / 2);
        Canvas.SetTop(leftEye, y + cellSize * 0.35);
        MapCanvas.Children.Add(leftEye);

        var rightEye = new Ellipse
        {
            Width = eyeSize,
            Height = eyeSize,
            Fill = Brushes.White
        };
        Canvas.SetLeft(rightEye, x + cellSize / 2 + eyeOffset - eyeSize / 2);
        Canvas.SetTop(rightEye, y + cellSize * 0.35);
        MapCanvas.Children.Add(rightEye);
    }

    private void DrawObstacle(double x, double y, double cellSize, double margin)
    {
        // Stone/obstacle (gray rectangle with rounded corners)
        var rect = new Rectangle
        {
            Width = cellSize - 2 * margin,
            Height = cellSize - 2 * margin,
            Fill = Brushes.Gray,
            Stroke = Brushes.DarkGray,
            StrokeThickness = 2,
            RadiusX = 5,
            RadiusY = 5
        };
        Canvas.SetLeft(rect, x + margin);
        Canvas.SetTop(rect, y + margin);
        MapCanvas.Children.Add(rect);
    }

    private void DrawWall(double x, double y, double cellSize)
    {
        // Wall (dark rectangle)
        var rect = new Rectangle
        {
            Width = cellSize,
            Height = cellSize,
            Fill = Brushes.DarkSlateGray,
            Stroke = Brushes.Black,
            StrokeThickness = 1
        };
        Canvas.SetLeft(rect, x);
        Canvas.SetTop(rect, y);
        MapCanvas.Children.Add(rect);
    }

    private void DrawLetter(double x, double y, double cellSize, string letter)
    {
        // Letter background (yellow/gold circle)
        double margin = cellSize * 0.15;
        var background = new Ellipse
        {
            Width = cellSize - 2 * margin,
            Height = cellSize - 2 * margin,
            Fill = Brushes.Gold,
            Stroke = Brushes.DarkOrange,
            StrokeThickness = 2
        };
        Canvas.SetLeft(background, x + margin);
        Canvas.SetTop(background, y + margin);
        MapCanvas.Children.Add(background);

        // Letter text
        var text = new TextBlock
        {
            Text = letter,
            FontSize = cellSize * 0.5,
            FontWeight = FontWeights.Bold,
            Foreground = Brushes.DarkRed,
            TextAlignment = TextAlignment.Center
        };

        // Measure text to center it
        text.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
        double textWidth = text.DesiredSize.Width;
        double textHeight = text.DesiredSize.Height;

        Canvas.SetLeft(text, x + (cellSize - textWidth) / 2);
        Canvas.SetTop(text, y + (cellSize - textHeight) / 2);
        MapCanvas.Children.Add(text);
    }
}
