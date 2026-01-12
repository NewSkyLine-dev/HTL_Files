using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;

namespace PixelDraw
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly int imageWidth = 390;
        private static readonly int imageHeigth = 500;
        

        public Color Color { get; set; } = Colors.Black;

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            Clean();
        }

        #region Hilfsfunktionen

        private static WriteableBitmap _wb;
        private static int _bytesPerPixel;
        private static int _stride;
        private static byte[] _colorArray;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Color = ((SolidColorBrush)((Button)sender).Background).Color;
        }

        private static byte[] ConvertColor(Color color)
        {
            byte[] c = new byte[4];
            c[0] = color.B;
            c[1] = color.G;
            c[2] = color.R;
            c[3] = color.A;
            return c;
        }

        private static Color ConvertColor(byte[] color)
        {
            Color c = new Color();
            c.B = color[0];
            c.G = color[1];
            c.R = color[2];
            c.A = color[3];
            return c;
        }

        private void setPixel(Color c, double x, double y)
        {
            if (x < _wb.PixelWidth && x > 0 && y < _wb.PixelHeight && y > 0)
            {
                _wb.WritePixels(new Int32Rect((int)x, (int)y, 1, 1), ConvertColor(c), _stride, 0);
            }
        }

        private void setPixel(double x, double y)
        {
            if (x < _wb.PixelWidth && x > 0 && y < _wb.PixelHeight && y > 0)
            {
                _wb.WritePixels(new Int32Rect((int)x, (int)y, 1, 1), _colorArray, _stride, 0);
            }
        }

        private static byte[] _readArray = ConvertColor(Colors.Black);

        private void setPixelThreaded(Color c, double x, double y)
        {
            _wb.Dispatcher.Invoke(new Action(() =>
            {
                if (x < _wb.PixelWidth && x > 0 && y < _wb.PixelHeight && y > 0)
                {
                    _wb.WritePixels(new Int32Rect((int)x, (int)y, 1, 1), ConvertColor(c), _stride, 0);
                }
            }));

        }

        private Color getPixelThreaded(double x, double y)
        {
            Color res = Colors.Transparent;
            _wb.Dispatcher.Invoke(new Action(() =>
            {
                if (x < _wb.PixelWidth && x > 0 && y < _wb.PixelHeight && y > 0)
                {
                    _wb.CopyPixels(new Int32Rect((int)x, (int)y, 1, 1), _readArray, _stride, 0);
                    res = ConvertColor(_readArray);
                }
            }));
            return res;
        }

        private Color getPixel(double x, double y)
        {
            Color res = Colors.Transparent;
            if (x < _wb.PixelWidth && x > 0 && y < _wb.PixelHeight && y > 0)
            {
                _wb.CopyPixels(new Int32Rect((int)x, (int)y, 1, 1), _readArray, _stride, 0);
                res = ConvertColor(_readArray);
            }

            return res;
        }


        private void Clean()
        {
            BitmapImage bitmap = new BitmapImage(new Uri("pack://application:,,,/Background.png"));
            //_wb = new WriteableBitmap(imageWidth, imageHeigth, 96, 96, PixelFormats.Bgra32, null);
            _wb = new WriteableBitmap(bitmap);
            _bytesPerPixel = (_wb.Format.BitsPerPixel + 7) / 8;
            _stride = _wb.PixelWidth * _bytesPerPixel;
            _colorArray = ConvertColor(Colors.Black);
            drawing.Source = _wb;
        }

        #endregion


        private void drawing_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // Beispiel LeftButtonDown auf Image mit Umrechnung in Pixelkoordinaten
            Point p = e.GetPosition(drawing);
            p.X = p.X * imageWidth / drawing.ActualWidth;
            p.Y = p.Y * imageHeigth / drawing.ActualHeight;

            Thread paintThread = new Thread(() =>
            {
                Color oldColor = getPixelThreaded((int)p.X, (int)p.Y);
                Color newColor = Color;
                if (!oldColor.Equals(newColor))
                {
                    FloodFill(p.X, p.Y, oldColor, newColor);
                }
            });

            paintThread.Start();
        }

        private void FloodFill(double x, double y, Color oldColor, Color newColor)
        {
            if (oldColor.Equals(newColor))
                return;

            Stack<Point> pixels = new Stack<Point>();
            pixels.Push(new Point(x, y));

            while (pixels.Count > 0)
            {
                Point p = pixels.Pop();
                double px = p.X;
                double py = p.Y;

                _wb.Dispatcher.Invoke(() =>
                {
                    Color currentColor = getPixelThreaded(px, py); // Current Color is the color of the pixel at the current position
                    if (currentColor.Equals(oldColor))
                    {
                        setPixelThreaded(newColor, px, py);

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
    }
}
