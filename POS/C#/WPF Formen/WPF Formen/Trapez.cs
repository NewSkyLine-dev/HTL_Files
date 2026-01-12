using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

namespace WPF_Formen
{
    class Trapez : Rechteck
    {
        public static readonly DependencyProperty X3Property = DependencyProperty.Register("X3", typeof(Double), typeof(Rechteck), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));
        public static readonly DependencyProperty Y3Property = DependencyProperty.Register("Y3", typeof(Double), typeof(Rechteck), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));
        public static readonly DependencyProperty X4Property = DependencyProperty.Register("X4", typeof(Double), typeof(Rechteck), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));
        public static readonly DependencyProperty Y4Property = DependencyProperty.Register("Y4", typeof(Double), typeof(Rechteck), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));

        [TypeConverter(typeof(LengthConverter))]
        public double X3
        {
            get => (double)GetValue(X3Property);
            set => SetValue(X3Property, value);
        }

        [TypeConverter(typeof(LengthConverter))]
        public double Y3
        {
            get => (double)GetValue(Y2Property);
            set => SetValue(Y2Property, value);
        }


        [TypeConverter(typeof(LengthConverter))]
        public double X4
        {
            get => (double)GetValue(X4Property);
            set => SetValue(X4Property, value);
        }

        [TypeConverter(typeof(LengthConverter))]
        public double Y4
        {
            get => (double)GetValue(Y4Property);
            set => SetValue(Y4Property, value);
        }

        protected override PathFigure CreatePathFigure()
        {
            return new PathFigure
            {
                StartPoint = new Point(X1, Y1),
                IsClosed = true,
                Segments = [
                    new LineSegment(new Point(X2, Y2), true),
                    new LineSegment(new Point(X3, Y3), true),
                    new LineSegment(new Point(X4, Y4), true)
                ]
            };
        }
    }
}
