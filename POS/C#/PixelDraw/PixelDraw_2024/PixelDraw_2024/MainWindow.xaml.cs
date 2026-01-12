using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PixelDraw_2024
{
    public partial class MainWindow : Window
    {
        private static readonly int imageSize = 300;
        private static WriteableBitmap? _wb;
        private static int _bytesPerPixel;
        private static int _stride;
        private static byte[]? _colorArray;
        private static byte[] _readArray = ConvertColor(Colors.Black);
        private static Point? startingPoint = null;
        private static Color _currentColor = Colors.Black;
        private static bool _isDrawing = false;
        private static bool _setStartPoint = false;
        private static bool _setEndPoint = false;

        private static Point _startPoint;
        private static Point _endPoint;

        public MainWindow()
        {
            InitializeComponent();
            _wb = new WriteableBitmap(imageSize, imageSize, 96, 96, PixelFormats.Bgra32, null);
            _bytesPerPixel = (_wb.Format.BitsPerPixel + 7) / 8;
            _stride = _wb.PixelWidth * _bytesPerPixel;
            _colorArray = ConvertColor(Colors.Black);
            drawing.Source = _wb;
        }

        #region Hilfsfunktionen
        private static int GetDistance(Point a, Point b)
        {
            return (int)Math.Sqrt(Math.Pow(b.X - a.X, 2) + Math.Pow(b.Y - a.Y, 2));
        }
        private void setPixelThreaded(Color c, int x, int y)
        {
            try
            {
                _wb.Dispatcher.Invoke(
                  System.Windows.Threading.DispatcherPriority.Normal
                  , new System.Windows.Threading.DispatcherOperationCallback(delegate
                  {
                      if (x < _wb.PixelWidth && x > 0 && y < _wb.PixelHeight && y > 0)
                      {
                          _wb.WritePixels(new Int32Rect(x, y, 1, 1), ConvertColor(c), _stride, 0);
                      }
                      return null;
                  }), null);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }

        }

        private Color getPixelThreaded(int x, int y)
        {
            Color res = Colors.Transparent;
            try
            {
                _wb.Dispatcher.Invoke(
                  System.Windows.Threading.DispatcherPriority.Normal
                  , new System.Windows.Threading.DispatcherOperationCallback(delegate
                  {
                      if (x < _wb.PixelWidth && x > 0 && y < _wb.PixelHeight && y > 0)
                      {
                          _wb.CopyPixels(new Int32Rect(x, y, 1, 1), _readArray, _stride, 0);
                          res = ConvertColor(_readArray);
                      }
                      return null;
                  }), null);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
            return res;
        }

        private Color getPixel(int x, int y)
        {
            Color res = Colors.Transparent;
            if (x < _wb.PixelWidth && x > 0 && y < _wb.PixelHeight && y > 0)
            {
                _wb.CopyPixels(new Int32Rect(x, y, 1, 1), _readArray, _stride, 0);
                res = ConvertColor(_readArray);
            }

            return res;
        }

        private static byte[] ConvertColor(Color color)
        {
            byte[] c = [color.B, color.G, color.R, color.A];
            return c;
        }

        private static Color ConvertColor(byte[] color)
        {
            Color c = new()
            {
                R = color[2],
                G = color[1],
                B = color[0],
                A = color[3]
            };
            return c;
        }

        public static void SetPixel(Color c, int x, int y)
        {
            if (x < _wb.PixelWidth && x > 0 && y < _wb.PixelHeight && y > 0)
            {
                _wb.WritePixels(new Int32Rect(x, y, 1, 1), ConvertColor(c), _stride, 0);
            }
        }

        private static void SetPixel(int x, int y)
        {
            if (x < _wb.PixelWidth && x > 0 && y < _wb.PixelHeight && y > 0)
            {
                _wb.WritePixels(new Int32Rect(x, y, 1, 1), _colorArray, _stride, 0);
            }
        }

        private ANode[,] ImageTo2DMatrix()
        {
            ANode[,] matrix = new ANode[_wb.PixelWidth, _wb.PixelHeight];
            for (int i = 0; i < _wb.PixelWidth; i++)
            {
                for (int j = 0; j < _wb.PixelHeight; j++)
                {
                    matrix[i, j] = new ANode
                    {
                        IsVisited = false,
                        IsAllowed = !getPixel(i, j).Equals(Colors.Black),
                        X = i,
                        Y = j,
                    };
                }
            }
            return matrix;
        }
        #endregion

        private static void DrawLine(int x1, int y1, int x2, int y2)
        {
            int dx = x2 - x1;
            int dy = y2 - y1;
            int steps = Math.Max(Math.Abs(dx), Math.Abs(dy));

            for (int i = 0; i < steps; i++)
            {
                int x = x1 + (i * dx / steps);
                int y = y1 + (i * dy / steps);
                SetPixel(_currentColor, x, y);
            }
        }

        private static void DrawCircle(int x1, int y1, int radius)
        {
            double circumference = 2 * Math.PI * radius;
            double angle = 2 * Math.PI / circumference;

            for (int i = 0; i < circumference; i++)
            {
                int x = (int)(x1 + radius * Math.Cos(angle * i));
                int y = (int)(y1 + radius * Math.Sin(angle * i));
                SetPixel(x, y);
            }
        }

        private void drawing_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point position = e.GetPosition(drawing);
            int x = (int)(position.X * _wb.PixelWidth / drawing.ActualWidth);
            int y = (int)(position.Y * _wb.PixelHeight / drawing.ActualHeight);

            if (_setStartPoint)
            {
                _startPoint = new Point(x, y);
                _setStartPoint = false;
            }
            else if (_setEndPoint)
            {
                _endPoint = new Point(x, y);
                _setEndPoint = false;
            }
            else
            {
                if (_isDrawing)
                {
                    if (startingPoint == null)
                    {
                        startingPoint = new Point(x, y);
                    }
                    else
                    {
                        DrawLine((int)startingPoint.Value.X, (int)startingPoint.Value.Y, x, y);
                        startingPoint = null;
                    }
                }
                else
                {
                    ThreadPool.QueueUserWorkItem((state) => FloodFill(x, y, getPixelThreaded(x, y), _currentColor));
                }
            }
        }

        private void FloodFill(double x, double y, Color oldColor, Color newColor)
        {
            if (oldColor.Equals(newColor))
                return;

            Stack<Point> pixels = new();
            pixels.Push(new Point(x, y));

            while (pixels.Count > 0)
            {
                Point p = pixels.Pop();
                double px = p.X;
                double py = p.Y;

                _wb.Dispatcher.Invoke(() =>
                {
                    Color currentColor = getPixelThreaded((int)px, (int)py);
                    if (currentColor.Equals(oldColor))
                    {
                        setPixelThreaded(newColor, (int)px, (int)py);

                        if (px + 1 < _wb.PixelWidth)
                            pixels.Push(new Point(px + 1, py));
                        if (px - 1 >= 0)
                            pixels.Push(new Point(px - 1, py));
                        if (py + 1 < _wb.PixelHeight)
                            pixels.Push(new Point(px, py + 1));
                        if (py - 1 >= 0)
                            pixels.Push(new Point(px, py - 1));
                    }
                });
            }
        }

        #region Bezier
        private static void DrawCatmullRomSpline(IEnumerable<Point> points, int numPoints = 1000)
        {
            Point[] p = points.ToArray();
            if (p.Length < 4)
                throw new ArgumentException("At least four points are required for Catmull-Rom spline");

            foreach (Point point in p)
            {
                DrawCircle((int)point.X, (int)point.Y, 5);
            }

            for (int i = 0; i < p.Length - 3; i++)
            {
                for (int j = 0; j <= numPoints; j++)
                {
                    double t = (double)j / numPoints;

                    double tt = t * t;
                    double ttt = tt * t;

                    double q1 = -ttt + 2 * tt - t;
                    double q2 = 3 * ttt - 5 * tt + 2;
                    double q3 = -3 * ttt + 4 * tt + t;
                    double q4 = ttt - tt;

                    double tx = 0.5 * (p[i].X * q1 + p[i + 1].X * q2 + p[i + 2].X * q3 + p[i + 3].X * q4);
                    double ty = 0.5 * (p[i].Y * q1 + p[i + 1].Y * q2 + p[i + 2].Y * q3 + p[i + 3].Y * q4);

                    SetPixel((int)tx, (int)ty);
                }
            }
        }
        #endregion

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem item = (ComboBoxItem)ColorCB.SelectedItem;
            _currentColor = (Color)ColorConverter.ConvertFromString(item.Content.ToString());
        }

        private void ComboBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem item = (ComboBoxItem)TypeCB.SelectedItem;
            _isDrawing = item.Content.ToString() switch
            {
                "Draw" => true,
                "Fill" => false,
                _ => false,
            };
            ;
        }

        private void Button_SetStart(object sender, RoutedEventArgs e) => _setStartPoint = true;
        private void Button_SetEnd(object sender, RoutedEventArgs e) => _setEndPoint = true;

        private void Dijkstra_Click(object sender, RoutedEventArgs e)
        {
            var graph = ImageTo2DMatrix();
            ANode start = graph[(int)_startPoint.X, (int)_startPoint.Y];
            ANode end = graph[(int)_endPoint.X, (int)_endPoint.Y];

            AStar dijkstra = new() 
            {
                Height = _wb.PixelHeight,
                Width = _wb.PixelWidth,
                Start = start,
                End = end,
                Nodes = graph
            };


            dijkstra.Run();
            DrawPath(dijkstra.End);
        }

        private static void DrawPath(ANode end)
        {
            ANode? current = end;
            while (current != null)
            {
                SetPixel(Colors.Blue, current.X, current.Y);
                current = current.Parent;
            }
        }
    }
}