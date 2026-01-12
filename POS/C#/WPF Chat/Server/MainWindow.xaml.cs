using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace Server;

public partial class MainWindow : Window
{
    private ChatServer? _server;
    private Task? _serverTask;
    private readonly ObservableCollection<string> _activityLog = new();
    private DateTime _serverStartTime;
    private readonly DispatcherTimer _uptimeTimer;
    private int _connectedClients;
    private int _totalMessages;

    public MainWindow()
    {
        InitializeComponent();
        ActivityListBox.ItemsSource = _activityLog;

        _uptimeTimer = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(1)
        };
        _uptimeTimer.Tick += UpdateUptime;

        UpdateStatistics();
    }

    private async void StartButton_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            if (!int.TryParse(PortTextBox.Text, out int port) || port < 1024 || port > 65535)
            {
                MessageBox.Show("Please enter a valid port number (1024-65535)", "Invalid Port",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            _server = new ChatServer(port);
            _server.LogActivity += OnServerActivity;
            _server.ClientConnected += OnClientConnected;
            _server.ClientDisconnected += OnClientDisconnected;
            _server.MessageSent += OnMessageSent;

            _serverTask = Task.Run(async () => await _server.StartAsync());
            _serverStartTime = DateTime.Now;

            StartButton.IsEnabled = false;
            StopButton.IsEnabled = true;
            PortTextBox.IsEnabled = false;

            StatusText.Text = $"Server running on port {port}";
            StatusIndicator.Fill = Brushes.Green;
            ServerStartTimeText.Text = _serverStartTime.ToString("yyyy-MM-dd HH:mm:ss");

            _uptimeTimer.Start();
            await UpdateStatisticsFromDatabase();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to start server: {ex.Message}", "Error",
                          MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void StopButton_Click(object sender, RoutedEventArgs e)
    {
        StopServer();
    }

    private void ClearLogButton_Click(object sender, RoutedEventArgs e)
    {
        _activityLog.Clear();
    }

    private void StopServer()
    {
        _server?.Stop();
        _server = null;
        _serverTask = null;

        StartButton.IsEnabled = true;
        StopButton.IsEnabled = false;
        PortTextBox.IsEnabled = true;

        StatusText.Text = "Server stopped";
        StatusIndicator.Fill = Brushes.Red;
        UptimeText.Text = "-";

        _uptimeTimer.Stop();
        _connectedClients = 0;
        UpdateStatistics();
    }

    private void OnServerActivity(string activity)
    {
        Dispatcher.Invoke(() =>
        {
            var timestamp = DateTime.Now.ToString("HH:mm:ss");
            _activityLog.Add($"[{timestamp}] {activity}");

            LastActivityText.Text = $"Last: {activity}";

            // Auto-scroll to bottom
            if (ActivityListBox.Items.Count > 0)
            {
                ActivityListBox.ScrollIntoView(ActivityListBox.Items[ActivityListBox.Items.Count - 1]);
            }

            // Keep log size manageable
            if (_activityLog.Count > 1000)
            {
                _activityLog.RemoveAt(0);
            }
        });
    }

    private void OnClientConnected()
    {
        Dispatcher.Invoke(() =>
        {
            _connectedClients++;
            UpdateStatistics();
        });
    }

    private void OnClientDisconnected()
    {
        Dispatcher.Invoke(() =>
        {
            _connectedClients = Math.Max(0, _connectedClients - 1);
            UpdateStatistics();
        });
    }

    private void OnMessageSent()
    {
        Dispatcher.Invoke(() =>
        {
            _totalMessages++;
            UpdateStatistics();
        });
    }

    private void UpdateUptime(object? sender, EventArgs e)
    {
        if (_server != null)
        {
            var uptime = DateTime.Now - _serverStartTime;
            UptimeText.Text = $"{uptime.Days}d {uptime.Hours:D2}h {uptime.Minutes:D2}m {uptime.Seconds:D2}s";
        }
    }

    private void UpdateStatistics()
    {
        ConnectedClientsText.Text = _connectedClients.ToString();
        TotalMessagesText.Text = _totalMessages.ToString();
    }

    private async Task UpdateStatisticsFromDatabase()
    {
        try
        {
            // This would require access to DatabaseManager
            // For now, we'll update these through events
            ActiveRoomsText.Text = "1"; // At least General room exists
            RegisteredUsersText.Text = "0"; // Will be updated as users register
        }
        catch
        {
            // Handle database access errors
        }
    }

    protected override void OnClosed(EventArgs e)
    {
        StopServer();
        base.OnClosed(e);
    }
}
