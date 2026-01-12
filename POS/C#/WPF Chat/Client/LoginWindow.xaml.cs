using DataModel;
using NetworkLib;
using System.Windows;
using System.Windows.Media;

namespace Client;

public partial class LoginWindow : Window, IListener
{
    private ChatClient? _chatClient;
    private string _selectedColor = "#FF0000FF";

    public string ServerAddress { get; private set; } = "";
    public int Port { get; private set; } = 8888;

    public LoginWindow()
    {
        InitializeComponent();
        ColorPreview.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString(_selectedColor));
    }

    private async void LoginButton_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(LoginUsernameTextBox.Text) ||
            string.IsNullOrWhiteSpace(LoginPasswordBox.Password))
        {
            MessageBox.Show("Please enter username and password.", "Validation Error",
                          MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        if (!ParseServerAddress())
            return;

        try
        {
            _chatClient = new ChatClient(this);
            LoginButton.IsEnabled = false;
            LoginButton.Content = "Connecting...";

            if (await _chatClient.ConnectAsync(ServerAddress, Port))
            {
                await _chatClient.LoginAsync(LoginUsernameTextBox.Text, LoginPasswordBox.Password);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Connection error: {ex.Message}", "Error",
                          MessageBoxButton.OK, MessageBoxImage.Error);
            _chatClient?.Disconnect();
            ResetLoginButton();
        }
    }

    private async void RegisterButton_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(RegisterUsernameTextBox.Text) ||
            string.IsNullOrWhiteSpace(RegisterPasswordBox.Password) ||
            string.IsNullOrWhiteSpace(ConfirmPasswordBox.Password))
        {
            MessageBox.Show("Please fill in all fields.", "Validation Error",
                          MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        if (RegisterPasswordBox.Password != ConfirmPasswordBox.Password)
        {
            MessageBox.Show("Passwords do not match.", "Validation Error",
                          MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        if (!ParseServerAddress())
            return;

        try
        {
            _chatClient = new ChatClient(this);
            RegisterButton.IsEnabled = false;
            RegisterButton.Content = "Connecting...";

            if (await _chatClient.ConnectAsync(ServerAddress, Port))
            {
                await _chatClient.RegisterAsync(RegisterUsernameTextBox.Text,
                                               RegisterPasswordBox.Password, _selectedColor);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Connection error: {ex.Message}", "Error",
                          MessageBoxButton.OK, MessageBoxImage.Error);
            _chatClient?.Disconnect();
            ResetRegisterButton();
        }
    }

    private void ChooseColorButton_Click(object sender, RoutedEventArgs e)
    {
        // Use a simple color picker dialog
        var colors = new[] { "#FF0000", "#00FF00", "#0000FF", "#FFFF00", "#FF00FF", "#00FFFF", "#FFA500", "#800080" };
        var colorNames = new[] { "Red", "Green", "Blue", "Yellow", "Magenta", "Cyan", "Orange", "Purple" };

        var dialog = new Window
        {
            Title = "Choose Color",
            Width = 300,
            Height = 200,
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
            Owner = this
        };

        var listBox = new System.Windows.Controls.ListBox();
        for (int i = 0; i < colors.Length; i++)
        {
            var item = new System.Windows.Controls.ListBoxItem
            {
                Content = colorNames[i],
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(colors[i])),
                Tag = colors[i]
            };
            listBox.Items.Add(item);
        }

        listBox.SelectionChanged += (s, args) =>
        {
            if (listBox.SelectedItem is System.Windows.Controls.ListBoxItem item)
            {
                _selectedColor = item.Tag.ToString()!;
                ColorPreview.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString(_selectedColor));
                dialog.Close();
            }
        };

        dialog.Content = listBox;
        dialog.ShowDialog();
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }

    private bool ParseServerAddress()
    {
        try
        {
            var parts = ServerAddressTextBox.Text.Split(':');
            ServerAddress = parts[0];
            Port = parts.Length > 1 ? int.Parse(parts[1]) : 8888;
            return true;
        }
        catch
        {
            MessageBox.Show("Invalid server address format. Use: hostname:port", "Validation Error",
                          MessageBoxButton.OK, MessageBoxImage.Warning);
            return false;
        }
    }

    private void ResetLoginButton()
    {
        LoginButton.IsEnabled = true;
        LoginButton.Content = "🔑 Login";
    }

    private void ResetRegisterButton()
    {
        RegisterButton.IsEnabled = true;
        RegisterButton.Content = "📝 Register";
    }

    // IListener implementation
    public void OnConnected() 
    {

    }

    public void OnConnectionFailed(string reason)
    {
        Dispatcher.Invoke(() =>
        {
            MessageBox.Show($"Connection failed: {reason}", "Connection Error",
                          MessageBoxButton.OK, MessageBoxImage.Error);
            ResetLoginButton();
            ResetRegisterButton();
        });
    }

    public void OnDisconnected() { }

    public void OnLoginSuccess(User user, List<ChatRoom> rooms)
    {
        Dispatcher.Invoke(() =>
        {
            DialogResult = true;
            Close();
        });
    }

    public void OnLoginFailed(string reason)
    {
        Dispatcher.Invoke(() =>
        {
            MessageBox.Show($"Login failed: {reason}", "Login Error",
                          MessageBoxButton.OK, MessageBoxImage.Error);
            _chatClient?.Disconnect();
            ResetLoginButton();
        });
    }

    public void OnRegistrationSuccess(User user)
    {
        Dispatcher.Invoke(() =>
        {
            DialogResult = true;
            Close();
        });
    }

    public void OnRegistrationFailed(string reason)
    {
        Dispatcher.Invoke(() =>
        {
            MessageBox.Show($"Registration failed: {reason}", "Registration Error",
                          MessageBoxButton.OK, MessageBoxImage.Error);
            _chatClient?.Disconnect();
            ResetRegisterButton();
        });
    }

    public void OnMessageReceived(ChatMessage message) { }
    public void OnPrivateMessageReceived(ChatMessage message) { }
    public void OnUserJoinedRoom(int roomId, string username) { }
    public void OnUserLeftRoom(int roomId, string username) { }
    public void OnRoomCreated(ChatRoom room) { }
    public void OnUserListUpdated(List<User> users) { }
    public void OnProfileUpdated(User user) { }
    public void OnError(string error) { }
}
