using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace IndicatorControl
{
    public class Indicator : Control
    {
        static Indicator()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Indicator), new FrameworkPropertyMetadata(typeof(Indicator)));
        }

        #region Dependency Properties

        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(double), typeof(Indicator),
                new PropertyMetadata(0.0, OnValueChanged));

        public double MinValue
        {
            get { return (double)GetValue(MinValueProperty); }
            set { SetValue(MinValueProperty, value); }
        }

        public static readonly DependencyProperty MinValueProperty =
            DependencyProperty.Register("MinValue", typeof(double), typeof(Indicator),
                new PropertyMetadata(0.0));

        public double MaxValue
        {
            get { return (double)GetValue(MaxValueProperty); }
            set { SetValue(MaxValueProperty, value); }
        }

        public static readonly DependencyProperty MaxValueProperty =
            DependencyProperty.Register("MaxValue", typeof(double), typeof(Indicator),
                new PropertyMetadata(100.0));

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(Indicator),
                new PropertyMetadata(string.Empty));

        public int PointerAngle
        {
            get { return (int)GetValue(PointerAngleProperty); }
            private set { SetValue(PointerAngleProperty, value); }
        }

        public static readonly DependencyProperty PointerAngleProperty =
            DependencyProperty.Register("PointerAngle", typeof(int), typeof(Indicator),
                new PropertyMetadata(0));

        #endregion

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (Indicator)d;
            control.UpdatePointerAngle();
        }

        private void UpdatePointerAngle()
        {
            const int startAngle = -135;
            const int endAngle = 135;
            int valueRange = (int)(MaxValue - MinValue);
            int angleRange = endAngle - startAngle;

            int angle = startAngle + (int)((Value - MinValue) * angleRange / valueRange);
            PointerAngle = Math.Max(startAngle, Math.Min(endAngle, angle));
        }
    }
}