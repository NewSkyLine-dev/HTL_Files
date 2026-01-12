using System.ComponentModel;
using System.Diagnostics.Metrics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace IndicatorTemplate
{
    public class Indicator : Control
    {
        static Indicator()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Indicator), new FrameworkPropertyMetadata(typeof(Indicator)));
        }

        #region Dependency Properties

        public double Minimum
        {
            get { return (double)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }

        public static readonly DependencyProperty MinimumProperty =
            DependencyProperty.Register("Minimum",
                                        typeof(double),
                                        typeof(Indicator),
                                        new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(OnMinimumMaximumValueChanged)));

        public double Maximum
        {
            get { return (double)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }

        public static readonly DependencyProperty MaximumProperty =
            DependencyProperty.Register("Maximum",
                                        typeof(double),
                                        typeof(Indicator),
                                        new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(OnMinimumMaximumValueChanged)));

        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value",
                                        typeof(double),
                                        typeof(Indicator),
                                        new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(OnMinimumMaximumValueChanged)));

        public double PointerAngle
        {
            get { return (double)GetValue(PointerAngleProperty); }
            private set { SetValue(PointerAngleProperty, value); }
        }

        public static readonly DependencyProperty PointerAngleProperty =
            DependencyProperty.Register("PointerAngle", 
                                        typeof(double), 
                                        typeof(Indicator),
                                        new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender));


        public double ViewSize
        {
            get { return (double)GetValue(ViewSizeProperty); }
            set { SetValue(ViewSizeProperty, value); }
        }

        public static readonly DependencyProperty ViewSizeProperty =
            DependencyProperty.Register("ViewSize",
                                        typeof(double),
                                        typeof(Indicator),
                                        new FrameworkPropertyMetadata(100.0, FrameworkPropertyMetadataOptions.AffectsRender));


        #endregion

        private static void OnMinimumMaximumValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Indicator indicator = (Indicator)d;
            indicator?.UpdatePointerAngle();
        }

        private void UpdatePointerAngle()
        {
            PointerAngle = Value;
            Console.WriteLine(Value);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            Image meterBackground = (Image)Template.FindName("PART_METERBACKGROUND", this);
            Image meterIndicator = (Image)Template.FindName("PART_METERINDICATOR", this);

            if (meterBackground != null && meterIndicator != null)
            {
                meterBackground.Source = new BitmapImage(new Uri("pack://application:,,,/IndicatorTemplate;component/Resources/MeterBackground.png"));
                meterIndicator.Source = new BitmapImage(new Uri("pack://application:,,,/IndicatorTemplate;component/Resources/MeterPointer.png"));
            }
        }
    }
}