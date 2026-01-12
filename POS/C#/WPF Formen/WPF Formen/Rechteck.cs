using System.ComponentModel;
using System.Windows.Media;
using System.Windows;

namespace WPF_Formen
{
    internal class Rechteck : Basis
    {
        public static readonly DependencyProperty X2Property = DependencyProperty.Register("X2", typeof(Double), typeof(Rechteck), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));
        public static readonly DependencyProperty Y2Property = DependencyProperty.Register("Y2", typeof(Double), typeof(Rechteck), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));

        [TypeConverter(typeof(LengthConverter))]
        public double X2
        {
            get => (double)GetValue(X2Property);
            set => SetValue(X2Property, value);
        }

        [TypeConverter(typeof(LengthConverter))]
        public double Y2
        {
            get => (double)GetValue(Y2Property);
            set => SetValue(Y2Property, value);
        }

        /// <summary>
        /// Zeichnet ein Rechteck
        /// </summary>
        protected override PathFigure CreatePathFigure()
        {
            return new PathFigure()
            {
                StartPoint = new Point(X1, Y1),
                IsClosed = true,
                Segments = [
                    new LineSegment(new Point(X1, Y2), true),
                    new LineSegment(new Point(X2, Y2), true),
                    new LineSegment(new Point(X2, Y1), true)
                ]
            };
        }
    }
}
