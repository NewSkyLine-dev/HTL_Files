using System.Windows.Controls;
using System.Windows.Media;

namespace Die_speisenden_Philosophen
{
    internal class Philosopher(Fork leftFork, Fork rightFork, TextBox status)
    {
        public static int VarianceThink { get; set; } = 200;
        internal static int VarianceEat { get; set; } = 40;
        internal static int TakeFork { get; set; } = 40;
        public static int MeanThink { get; set; } = 1000;
        public static int MeanEat { get; set; } = 200;

        private Fork leftFork = leftFork;
        private Fork rightFork = rightFork;
        private TextBox status = status;
        private bool running;
        private Thread thread;
        private static Random rand = new Random();

        public void StartThread()
        {
            running = true;
            ThreadStart start = new ThreadStart(Start);
            thread = new Thread(start);
            thread.IsBackground = true;
            thread.Start();
        }

        public void StopThread()
        {
            running = false;
            if (thread != null)
            {
                thread.Interrupt();
                thread.Join();
            }
        }

        private void Start()
        {
            try
            {
                while (running)
                {
                    Think();
                    Eat();
                }
            }
            catch (ThreadInterruptedException ex)
            {

            }
        }

        private void ChangeStatus(SolidColorBrush brush, String text)
        {
            status.Dispatcher.BeginInvoke(new Action(() =>
            {
                status.Text = text;
                status.Background = brush;
            }));
        }

        private void Think()
        {
            ChangeStatus(Brushes.White, "denkt");
            int thinkTime = GetRandomNumber(MeanThink, VarianceThink);
            Thread.Sleep(thinkTime);
        }

        static private int GetRandomNumber(int mean, int variance)
        {
            int val = rand.Next(variance * 2) - variance + mean;
            return Math.Max(val, 0);
        }

        private void Eat()
        {
            ChangeStatus(Brushes.Tomato, "wartet");
            lock (rightFork)
            {
                ChangeStatus(Brushes.Green, "nimmt rechte Gabel");
                Thread.Sleep(TakeFork);
                lock (leftFork)
                {
                    ChangeStatus(Brushes.Green, "nimmt linke Gabel");
                    Thread.Sleep(TakeFork);
                    ChangeStatus(Brushes.SaddleBrown, "isst");
                    int eatTime = GetRandomNumber(MeanEat, VarianceEat);
                    Thread.Sleep(eatTime);
                }
            }
            ChangeStatus(Brushes.White, "denkt");
        }
    }
}
