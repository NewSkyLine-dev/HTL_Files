using System.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Threading;

namespace WeckerLibrary
{
    public class CountdownTimerControl : Control
    {
        private TimeSpan initialTime;
        private DispatcherTimer? countdownTimer;
        private bool isRunning;

        static CountdownTimerControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CountdownTimerControl), new FrameworkPropertyMetadata(typeof(CountdownTimerControl)));
        }


        public TimeSpan RemainingTime
        {
            get { return (TimeSpan)GetValue(RemainingTimeProperty); }
            set { SetValue(RemainingTimeProperty, value); }
        }

        public static readonly DependencyProperty RemainingTimeProperty =
            DependencyProperty.Register("RemainingTime",
                                        typeof(TimeSpan),
                                        typeof(CountdownTimerControl),
                                        new FrameworkPropertyMetadata(TimeSpan.Zero, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly RoutedEvent TimerExpiredEvent =
            EventManager.RegisterRoutedEvent(
                "TimerExpired",
                RoutingStrategy.Bubble,
                typeof(RoutedEventHandler),
                typeof(CountdownTimerControl));

        public event RoutedEventHandler TimerExpired
        {
            add { AddHandler(TimerExpiredEvent, value); }
            remove { RemoveHandler(TimerExpiredEvent, value); }
        }

        protected virtual void OnTimerExpired()
        {
            RaiseEvent(new RoutedEventArgs(TimerExpiredEvent));
            PlayAlarmSound();
        }

        private void PlayAlarmSound()
        {
            SoundPlayer player = new(@"c:\windows\media\alarm01.wav");
            player.Play();
        }

        private void OnTimerTick(object sender, EventArgs e)
        {
            if (RemainingTime.TotalSeconds > 0)
            {
                RemainingTime = RemainingTime.Subtract(TimeSpan.FromSeconds(1));
            }
            else
            {
                Stop();
                OnTimerExpired();
            }
        }

        public void Stop()
        {
            if (isRunning)
            {
                countdownTimer.Stop();
                isRunning = false;
            }
        }


        public void Start()
        {
            if (!isRunning && RemainingTime.TotalSeconds > 0)
            {
                countdownTimer.Start();
                isRunning = true;
            }
        }

        public void Pause()
        {
            if (isRunning)
            {
                countdownTimer.Stop();
                isRunning = false;
            }
        }

        public void Reset()
        {
            Pause();
            RemainingTime = initialTime;
        }

        public void SetTime(int minutes, int seconds)
        {
            initialTime = new TimeSpan(0, minutes, seconds);
            RemainingTime = initialTime;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            Button startButton = (Button)Template.FindName("PART_START_BUTTON", this);
            Button pauseButton = (Button)Template.FindName("PART_PAUSE_BUTTON", this);
            Button resetButton = (Button)Template.FindName("PART_RESET_BUTTON", this);
            Button setTimeButton = (Button)Template.FindName("PART_SET_TIME_BUTTON", this);
            TextBlock timeDisplay = (TextBlock)Template.FindName("PART_TIME_DISPLAY", this);

            if (startButton != null) startButton.Click += (s, e) => Start();
            if (pauseButton != null) pauseButton.Click += (s, e) => Pause();
            if (resetButton != null) resetButton.Click += (s, e) => Reset();
            if (setTimeButton != null) setTimeButton.Click += OnSetTimeClick;

            countdownTimer = new System.Windows.Threading.DispatcherTimer();
            countdownTimer.Interval = TimeSpan.FromSeconds(1);
            countdownTimer.Tick += OnTimerTick;

            // Binding für die Zeitanzeige
            if (timeDisplay != null)
            {
                Binding timeBinding = new Binding("RemainingTime")
                {
                    Source = this,
                    StringFormat = "{0:mm\\:ss}"
                };
                timeDisplay.SetBinding(TextBlock.TextProperty, timeBinding);
            }
        }

        private void OnSetTimeClick(object sender, RoutedEventArgs e)
        {
            TimerSettingsDialog dialog = new TimerSettingsDialog();
            if (dialog.ShowDialog() == true)
            {
                SetTime(dialog.Minutes, dialog.Seconds);
            }
        }
    }
}