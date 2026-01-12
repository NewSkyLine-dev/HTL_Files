using System.Windows;

namespace Die_speisenden_Philosophen
{
    public partial class MainWindow : Window
    {
        private Philosopher p1;
        private Philosopher p2;
        private Philosopher p3;
        private Philosopher p4;
        private Philosopher p5;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void StartGame(object sender, RoutedEventArgs e)
        {
            Fork fork1 = new Fork(1);
            Fork fork2 = new Fork(2);
            Fork fork3 = new Fork(3);
            Fork fork4 = new Fork(4);
            Fork fork5 = new Fork(5);

            p1 = new Philosopher(fork5, fork1, status1);
            p2 = new Philosopher(fork1, fork2, status2);
            p3 = new Philosopher(fork2, fork3, status3);
            p4 = new Philosopher(fork3, fork4, status4);
            p5 = new Philosopher(fork4, fork5, status5);

            Philosopher.MeanThink = int.Parse(MeanThink.Text);
            Philosopher.MeanEat = int.Parse(MeanEat.Text);
            Philosopher.VarianceThink = int.Parse(VarianceThink.Text); 
            Philosopher.VarianceEat = int.Parse(VarianceEat.Text); 
            Philosopher.TakeFork = int.Parse(TakeForkTime.Text);

            p1.StartThread();
            p2.StartThread();
            p3.StartThread();
            p4.StartThread();
            p5.StartThread();
        }

        private void EndGame(object sender, RoutedEventArgs e)
        {
            p1.StopThread();
            p2.StopThread();
            p3.StopThread();
            p4.StopThread();
            p5.StopThread();
        }
    }
}