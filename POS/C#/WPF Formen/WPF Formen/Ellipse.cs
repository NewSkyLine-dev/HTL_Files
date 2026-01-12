using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

namespace WPF_Formen
{
    class Ellipse : Kreis
    {
        public static DependencyProperty Radius2Property = DependencyProperty.Register(
            "Radius2", typeof(double), typeof(Ellipse),
            new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | 
                FrameworkPropertyMetadataOptions.AffectsMeasure, OnRadius2Changed));

        [TypeConverter(typeof(LengthConverter))]
        public double Radius2
        {
            get => (double)GetValue(Radius2Property);
            set => SetValue(Radius2Property, value);
        }

        private static void OnRadius2Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Ellipse el)
            {
                el.X1 -= el.Radius;
                el.Y1 -= el.Radius2;
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
                Size = new Size(Radius, Radius2),
                SweepDirection = SweepDirection.Clockwise,
                IsLargeArc = true
            };

            ArcSegment arc2 = new()
            {
                Point = new Point(X1 + Radius, Y1),
                Size = new Size(Radius, Radius2),
                SweepDirection = SweepDirection.Clockwise,
                IsLargeArc = true
            };

            pathFigure.Segments.Add(arc1);
            pathFigure.Segments.Add(arc2);

            return pathFigure;
        }
    }
}
