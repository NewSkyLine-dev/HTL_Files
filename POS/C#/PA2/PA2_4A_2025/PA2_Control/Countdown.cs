using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace PA2_Control
{
    public class Countdown : Control
    {
        private readonly DispatcherTimer timer = new();
        private RotateTransform? imageRotation;

        static Countdown()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Countdown), new FrameworkPropertyMetadata(typeof(Countdown)));
        }

        public static readonly DependencyProperty CountdownWertProperty = DependencyProperty.Register("CountdownWert", typeof(int), typeof(Countdown), new PropertyMetadata(10));
        public int CountdownWert
        {
            get { return (int)GetValue(CountdownWertProperty); }
            set { SetValue(CountdownWertProperty, value); }
        }

        public static readonly RoutedEvent CounteddownEvent =
            EventManager.RegisterRoutedEvent(
                "Counteddown",
                RoutingStrategy.Bubble,
                typeof(RoutedEventHandler),
                typeof(Countdown));

        public event RoutedEventHandler Counteddown
        {
            add { AddHandler(CounteddownEvent, value); }
            remove { RemoveHandler(CounteddownEvent, value); }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            imageRotation = GetTemplateChild("ImageRotation") as RotateTransform;

            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            if (CountdownWert > 0)
            {
                CountdownWert--;
                imageRotation.Angle = CountdownWert * 6;
            }
            else
            {
                timer.Stop();
                RaiseEvent(new RoutedEventArgs(CounteddownEvent));
            }
        }
    }
}