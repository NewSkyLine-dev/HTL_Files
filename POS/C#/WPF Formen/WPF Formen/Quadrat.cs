using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

namespace WPF_Formen
{
    class Quadrat : Rechteck
    {
        public static readonly DependencyProperty SeitenlaengeProperty = DependencyProperty.Register(
            "Seitenlaenge", typeof(double), typeof(Quadrat),
            new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure, OnSeitenlaengeChanged));

        [TypeConverter(typeof(LengthConverter))]
        public double Seitenlaenge
        {
            get => (double)GetValue(SeitenlaengeProperty);
            set => SetValue(SeitenlaengeProperty, value);
        }

        private static void OnSeitenlaengeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Quadrat quadrat)
            {
                double seitenlaenge = quadrat.Seitenlaenge;
                quadrat.X2 = quadrat.X1 + seitenlaenge;
                quadrat.Y2 = quadrat.Y1 + seitenlaenge;
            }
        }

        protected override PathFigure CreatePathFigure()
        {
            return base.CreatePathFigure();
        }
    }
}
