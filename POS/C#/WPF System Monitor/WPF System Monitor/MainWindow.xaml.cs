using System.ComponentModel;
using System.Diagnostics;
using System.Management;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace WPF_System_Monitor
{
    public partial class MainWindow : Window
    {
        private Thread? monitorThread;
        private bool isRunning = true;
        private bool showGraphs = false;
        private readonly List<double> cpuHistory = [];
        private readonly List<double> memoryHistory = [];
        private const int MAX_HISTORY_POINTS = 50;
        private readonly PerformanceCounter cpuCounter = new("Processor", "% Processor Time", "_Total");
        private readonly PerformanceCounter memCounter = new("Memory", "Available MBytes");

        public MainWindow()
        {
            InitializeComponent(); 
            InitializeMonitoring();
        }

        private void InitializeMonitoring()
        {
            monitorThread = new Thread(MonitoringLoop)
            {
                IsBackground = true
            };
            monitorThread.Start();
        }

        public static string? QueryComputerSystem(string type)
        {
            string? str = null;
            ManagementObjectSearcher objCS = new("SELECT * FROM Win32_ComputerSystem");
            foreach (ManagementObject objMgmt in objCS.Get().Cast<ManagementObject>())
            {
                str = objMgmt[type].ToString();
            }
            return str;
        }

        private void MonitoringLoop()
        {
            while (isRunning)
            {
                var cpuValue = cpuCounter.NextValue();

                var memValue = memCounter.NextValue();

                // Update UI
                Dispatcher.Invoke(() =>
                {
                    UpdateIndicators(cpuValue, memValue);
                    if (showGraphs)
                        UpdateGraphs(cpuValue, memValue);
                });

                Thread.Sleep(1000);
            }
        }

        private void UpdateIndicators(float cpu, float memory)
        {
            CpuIndicator.Title = "CPU %";

            CpuIndicator.MinValue = 0;
            CpuIndicator.Value = (int)cpu;
            CpuIndicator.MaxValue = 100.0;


            MemoryIndicator.Title = "Hauptspeicher (GB)";
            MemoryIndicator.MinValue = 0;
            MemoryIndicator.Value = (int)memory;
            if (double.TryParse(QueryComputerSystem("totalphysicalmemory"), out double total))
            {
                MemoryIndicator.MaxValue = total;
            }
        }

        private void UpdateGraphs(float cpu, float memory)
        {
            cpuHistory.Add(cpu);
            memoryHistory.Add(memory);

            if (cpuHistory.Count > MAX_HISTORY_POINTS)
            {
                cpuHistory.RemoveAt(0);
                memoryHistory.RemoveAt(0);
            }

            // Update graph points
            var cpuPoints = new PointCollection();
            var memPoints = new PointCollection();

            for (int i = 0; i < cpuHistory.Count; i++)
            {
                double x = (GraphPanel.ActualWidth / MAX_HISTORY_POINTS) * i;
                double cpuY = GraphPanel.ActualHeight - (cpuHistory[i] / 100.0 * GraphPanel.ActualHeight);
                double memY = GraphPanel.ActualHeight - (memoryHistory[i] / 100.0 * GraphPanel.ActualHeight);

                cpuPoints.Add(new Point(x, cpuY));
                memPoints.Add(new Point(x, memY));
            }

            CpuGraph.Points = cpuPoints;
            MemoryGraph.Points = memPoints;
        }

        private void MainGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void ShowIndicator_Click(object sender, RoutedEventArgs e)
        {
            showGraphs = false;
            IndicatorPanel.Visibility = Visibility.Visible;
            GraphPanel.Visibility = Visibility.Collapsed;
        }

        private void ShowGraphs_Click(object sender, RoutedEventArgs e)
        {
            showGraphs = true;
            IndicatorPanel.Visibility = Visibility.Collapsed;
            GraphPanel.Visibility = Visibility.Visible;
        }

        private void ToggleWindow_Click(object sender, RoutedEventArgs e)
        {
            Visibility = Visibility == Visibility.Visible ? Visibility.Hidden : Visibility.Visible;
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            isRunning = false;
        }
    }
}