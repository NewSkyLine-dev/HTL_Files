using PA2_Control;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace PA2_4A_2025
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private int _countdownWert = 10;

        public int CountdownWert
        {
            get { return _countdownWert; }
            set { _countdownWert = value; NotifyPropertyChanged(); }
        }

        private string _currentTime = DateTime.Now.ToString("T");

        public string CurrentTime
        {
            get { return _currentTime; }
            set { _currentTime = value; NotifyPropertyChanged(); }
        }


        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;

            ThreadPool.QueueUserWorkItem((object _) =>
            {
                int counter = 0;
                while (true)
                {
                    CurrentTime = DateTime.Now.ToString("T");
                    counter++;
                    Thread.Sleep(1000);

                    if (counter % 10 == 0)
                    {
                        App.Current.Dispatcher.BeginInvoke(() =>
                        {
                            Countdown c = new();
                            c.CountdownWert = new Random().Next(30, 60);
                            c.Counteddown += (sender, e) =>
                            {
                                Countdowns.Items.Remove(sender);
                            };
                            Countdowns.Items.Add(c);
                        });
                    }
                }
            });
        }

        private void CountdownInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(CountdownInput.Text, out int result))
            {
                CountdownWert = result > 0 ? result : 10;
            } 
            else
            {
                CountdownWert = 10;
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}