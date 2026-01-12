using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

namespace PA2_4A_2025
{
    class Kreuz : Basis
    {

        public static readonly DependencyProperty BreiteInnenProperty =
            DependencyProperty.Register(
                "BreiteInnen",
                typeof(double),
                typeof(Kreuz),
                new PropertyMetadata(default(double), null));

        [TypeConverter(typeof(LengthConverter))]
        public double BreiteInnen
        {
            get => (double)GetValue(BreiteInnenProperty);
            set => SetValue(BreiteInnenProperty, value);
        }


        public static readonly DependencyProperty BreiteAußenProperty =
            DependencyProperty.Register(
                "BreiteAußen",
                typeof(double),
                typeof(Kreuz),
                new PropertyMetadata(default(double), null));

        [TypeConverter(typeof(LengthConverter))]
        public double BreiteAußen
        {
            get => (double)GetValue(BreiteAußenProperty);
            set => SetValue(BreiteAußenProperty, value);
        }

        protected override Geometry DefiningGeometry
        {
            get
            {
                PathGeometry geometry = new()
                {
                    FillRule = FillRule.EvenOdd,
                    Figures = [CreatePathFigure()]
                };

                return geometry;
            }
        }

        protected virtual PathFigure CreatePathFigure()
        {
            double innen = BreiteInnen / 2;
            double außen = BreiteAußen / 2;

            PathFigure myPathFigure = new()
            {
                StartPoint = new Point(X1 - innen, Y1 - innen),
                Segments = [
                    new LineSegment(new Point(X1 - innen, Y1 - außen), true),
                    new LineSegment(new Point(X1 + innen, Y1 - außen), true),

                    new LineSegment(new Point(X1 + innen, Y1 - innen), true),
                    new LineSegment(new Point(X1 + außen, Y1 - innen), true),
                    
                    new LineSegment(new Point(X1 + außen, Y1 + innen), true),
                    new LineSegment(new Point(X1 + innen, Y1 + innen), true),
                    
                    new LineSegment(new Point(X1 + innen, Y1 + außen), true),
                    new LineSegment(new Point(X1 - innen, Y1 + außen), true),
                    
                    new LineSegment(new Point(X1 - innen, Y1 + innen), true),
                    new LineSegment(new Point(X1 - außen, Y1 + innen), true),

                    new LineSegment(new Point(X1 - außen, Y1 - innen), true),
                    new LineSegment(new Point(X1 - innen, Y1 - innen), true),
                ],
                IsClosed = false
            };
            return myPathFigure;
        }
    }
}
