using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WPF_Formen
{
    class Kreis : Basis
    {
        public static readonly DependencyProperty RadiusProperty = DependencyProperty.Register(
            "Radius", typeof(double), typeof(Kreis), 
            new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | 
                FrameworkPropertyMetadataOptions.AffectsMeasure, OnRadiusChanged));

        [TypeConverter(typeof(LengthConverter))]
        public double Radius
        {
            get => (double)GetValue(RadiusProperty);
            set => SetValue(RadiusProperty, value);
        }

        private static void OnRadiusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Kreis kreis)
            {
                double radius = kreis.Radius;
                kreis.X1 -= radius;
                kreis.Y1 -= radius;
            }
        }

        protected override PathFigure CreatePathFigure()
        {
            PathFigure pathFigure = new()
            {
                StartPoint = new Point(X1 + Radius, Y1),
                IsClosed = true
            };

            ArcSegment arc1 = new()
            {
                Point = new Point(X1 - Radius, Y1),
                Size = new Size(Radius, Radius),
                SweepDirection = SweepDirection.Clockwise,
                IsLargeArc = true
            };

            ArcSegment arc2 = new()
            {
                Point = new Point(X1 + Radius, Y1),
                Size = new Size(Radius, Radius),
                SweepDirection = SweepDirection.Clockwise,
                IsLargeArc = true
            };

            pathFigure.Segments.Add(arc1);
            pathFigure.Segments.Add(arc2);

            return pathFigure;
        }
    }
}
