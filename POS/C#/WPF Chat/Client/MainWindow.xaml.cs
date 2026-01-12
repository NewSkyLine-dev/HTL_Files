using NetworkLib;
using NetworkLib.Models;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Microsoft.Win32;
using DataModel;

namespace Client;

public partial class MainWindow : Window, IListener
{
    private ChatClient? _chatClient;
    private readonly ObservableCollection<ChatRoom> _rooms = [];
    private readonly ObservableCollection<User> _users = [];
    private readonly Dictionary<int, ObservableCollection<ChatMessage>> _roomMessages = [];
    private readonly DispatcherTimer _timeTimer;

    public MainWindow()
    {
        InitializeComponent();

        RoomsListBox.ItemsSource = _rooms;
        UsersListBox.ItemsSource = _users;

        _timeTimer = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(1)
        };
        _timeTimer.Tick += (s, e) => TimeText.Text = DateTime.Now.ToString("HH:mm:ss");
        _timeTimer.Start();

        SetDefaultProfileImage();
    }

    private void SetDefaultProfileImage()
    {
        // Create a simple default image programmatically
        var bitmap = new WriteableBitmap(50, 50, 96, 96, PixelFormats.Bgr32, null);
        var rect = new Int32Rect(0, 0, 50, 50);
        var pixels = new byte[50 * 50 * 4];
        // Fill with light gray
        for (int i = 0; i < pixels.Length; i += 4)
        {
            pixels[i] = 200; // Blue
            pixels[i + 1] = 200; // Green
            pixels[i + 2] = 200; // Red
            pixels[i + 3] = 255; // Alpha
        }
        bitmap.WritePixels(rect, pixels, 50 * 4, 0);
        ProfileImageBrush.ImageSource = bitmap;
    }

    private async void ConnectMenuItem_Click(object sender, RoutedEventArgs e)
    {
        var loginWindow = new LoginWindow { Owner = this };
        if (loginWindow.ShowDialog() == true)
        {
            _chatClient = new ChatClient(this);
            await _chatClient.ConnectAsync(loginWindow.ServerAddress, loginWindow.Port);
        }
    }

    private void DisconnectMenuItem_Click(object sender, RoutedEventArgs e)
    {
        _chatClient?.Disconnect();
    }

    private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private async void ChangeColorMenuItem_Click(object sender, RoutedEventArgs e)
    {
        var color = ShowColorDialog();
        if (color != null)
        {
            await _chatClient?.UpdateProfileAsync(color: color)!;
        }
    }

    private string? ShowColorDialog()
    {
        var colors = new[]
        {
            "#FF0000", "#00FF00", "#0000FF", "#FFFF00", "#FF00FF", "#00FFFF",
            "#FFA500", "#800080", "#008000", "#800000", "#000080", "#808000"
        };
        var colorNames = new[]
        {
            "Red", "Green", "Blue", "Yellow", "Magenta", "Cyan",
            "Orange", "Purple", "Dark Green", "Maroon", "Navy", "Olive"
        };

        var dialog = new Window
        {
            Title = "Choose Color",
            Width = 250,
            Height = 300,
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
            Owner = this,
            ResizeMode = ResizeMode.NoResize
        };

        var stackPanel = new StackPanel { Margin = new Thickness(10) };

        for (int i = 0; i < colors.Length; i++)
        {
            var button = new Button
            {
                Content = colorNames[i],
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(colors[i])),
                Foreground = Brushes.White,
                Height = 30,
                Margin = new Thickness(0, 2, 0, 2),
                Tag = colors[i]
            };

            button.Click += (s, e) =>
            {
                dialog.Tag = ((Button)s!).Tag;
                dialog.DialogResult = true;
                dialog.Close();
            };

            stackPanel.Children.Add(button);
        }

        dialog.Content = stackPanel;
        return dialog.ShowDialog() == true ? dialog.Tag?.ToString() : null;
    }

    private async void ChangeProfilePictureMenuItem_Click(object sender, RoutedEventArgs e)
    {
        var openFileDialog = new OpenFileDialog
        {
            Filter = "Image files (*.png;*.jpg;*.jpeg;*.bmp)|*.png;*.jpg;*.jpeg;*.bmp|All files (*.*)|*.*",
            Title = "Select Profile Picture"
        };

        if (openFileDialog.ShowDialog() == true)
        {
            try
            {
                var imageBytes = await File.ReadAllBytesAsync(openFileDialog.FileName);
                var base64Image = Convert.ToBase64String(imageBytes);
                await _chatClient?.UpdateProfileAsync(profileImageBase64: base64Image)!;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading image: {ex.Message}", "Error",
                                  MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    private async void JoinRoomMenuItem_Click(object sender, RoutedEventArgs e)
    {
        // Implementation for joining existing rooms
        MessageBox.Show("Join room functionality to be implemented");
    }

    private async void CreateRoomMenuItem_Click(object sender, RoutedEventArgs e)
    {
        var roomName = ShowInputDialog("Create Room", "Enter room name:");

        if (!string.IsNullOrWhiteSpace(roomName))
        {
            await _chatClient?.CreateRoomAsync(roomName, "User created room")!;
        }
    }

    // Replace InputBox with a proper dialog
    private string? ShowInputDialog(string title, string prompt, string defaultValue = "")
    {
        var dialog = new Window
        {
            Title = title,
            Width = 300,
            Height = 150,
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
            Owner = this,
            ResizeMode = ResizeMode.NoResize
        };

        var grid = new Grid();
        grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
        grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
        grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

        var label = new TextBlock { Text = prompt, Margin = new Thickness(10) };
        var textBox = new TextBox { Text = defaultValue, Margin = new Thickness(10) };
        var buttonPanel = new StackPanel { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Right, Margin = new Thickness(10) };

        var okButton = new Button { Content = "OK", Width = 75, Margin = new Thickness(0, 0, 10, 0) };
        var cancelButton = new Button { Content = "Cancel", Width = 75 };

        okButton.Click += (s, e) => { dialog.DialogResult = true; dialog.Close(); };
        cancelButton.Click += (s, e) => { dialog.DialogResult = false; dialog.Close(); };

        buttonPanel.Children.Add(okButton);
        buttonPanel.Children.Add(cancelButton);

        Grid.SetRow(label, 0);
        Grid.SetRow(textBox, 1);
        Grid.SetRow(buttonPanel, 2);

        grid.Children.Add(label);
        grid.Children.Add(textBox);
        grid.Children.Add(buttonPanel);

        dialog.Content = grid;

        return dialog.ShowDialog() == true ? textBox.Text : null;
    }

    private async void RoomsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (RoomsListBox.SelectedItem is ChatRoom room)
        {
            await LoadRoomTab(room);
        }
    }

    private async void UsersListBox_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        if (UsersListBox.SelectedItem is User user && user.Id != _chatClient?.CurrentUser?.Id)
        {
            // Open private message tab
            await OpenPrivateMessageTab(user);
        }
    }

    private async Task LoadRoomTab(ChatRoom room)
    {
        // Check if tab already exists
        var existingTab = ChatTabControl.Items.Cast<TabItem>()
            .FirstOrDefault(t => t.Tag is int id && id == room.Id);

        if (existingTab == null)
        {
            var messages = await _chatClient?.GetRoomMessagesAsync((int)room.Id);
            _roomMessages[(int)room.Id] = new ObservableCollection<ChatMessage>(messages ?? new List<ChatMessage>());

            var chatTab = CreateChatTab(room.Name, (int)room.Id, _roomMessages[(int)room.Id]);
            ChatTabControl.Items.Add(chatTab);
            ChatTabControl.SelectedItem = chatTab;
        }
        else
        {
            ChatTabControl.SelectedItem = existingTab;
        }
    }

    private async Task OpenPrivateMessageTab(User user)
    {
        var tabName = $"Private: {user.Username}";
        var existingTab = ChatTabControl.Items.Cast<TabItem>()
            .FirstOrDefault(t => t.Header.ToString() == tabName);

        if (existingTab == null)
        {
            var messages = new ObservableCollection<ChatMessage>();
            var chatTab = CreateChatTab(tabName, -(int)user.Id, messages, isPrivate: true);
            ChatTabControl.Items.Add(chatTab);
            ChatTabControl.SelectedItem = chatTab;
        }
        else
        {
            ChatTabControl.SelectedItem = existingTab;
        }
    }

    private TabItem CreateChatTab(string title, int id, ObservableCollection<ChatMessage> messages, bool isPrivate = false)
    {
        var tab = new TabItem { Header = title, Tag = id };

        var grid = new Grid();
        grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
        grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

        // Messages list
        var messagesListBox = new ListBox
        {
            ItemsSource = messages
        };

        // Message input
        var inputGrid = new Grid();
        inputGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        inputGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

        var messageTextBox = new TextBox
        {
            Height = 30,
            Margin = new Thickness(5),
            Name = "MessageTextBox"
        };

        var sendButton = new Button
        {
            Content = "Send",
            Width = 60,
            Height = 30,
            Margin = new Thickness(0, 5, 5, 5)
        };

        sendButton.Click += async (s, e) =>
        {
            if (!string.IsNullOrWhiteSpace(messageTextBox.Text))
            {
                if (isPrivate)
                {
                    var targetUserId = Math.Abs(id);
                    await _chatClient?.SendPrivateMessageAsync(targetUserId, messageTextBox.Text)!;
                }
                else
                {
                    await _chatClient?.SendMessageAsync(id, messageTextBox.Text)!;
                }
                messageTextBox.Clear();
            }
        };

        Grid.SetColumn(messageTextBox, 0);
        Grid.SetColumn(sendButton, 1);
        inputGrid.Children.Add(messageTextBox);
        inputGrid.Children.Add(sendButton);

        Grid.SetRow(messagesListBox, 0);
        Grid.SetRow(inputGrid, 1);
        grid.Children.Add(messagesListBox);
        grid.Children.Add(inputGrid);

        tab.Content = grid;
        return tab;
    }

    // IListener implementation
    public void OnConnected()
    {
        Dispatcher.Invoke(() =>
        {
            ConnectionStatusIndicator.Fill = Brushes.Orange;
            ConnectionStatusText.Text = "Connected";
            StatusBarText.Text = "Connected to server";
        });
    }

    public void OnConnectionFailed(string reason)
    {
        Dispatcher.Invoke(() =>
        {
            MessageBox.Show($"Connection failed: {reason}", "Connection Error",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            StatusBarText.Text = "Connection failed";
        });
    }

    public void OnDisconnected()
    {
        Dispatcher.Invoke(() =>
        {
            ConnectionStatusIndicator.Fill = Brushes.Red;
            ConnectionStatusText.Text = "Disconnected";
            StatusBarText.Text = "Disconnected";
            UsernameText.Text = "Not connected";

            ConnectMenuItem.IsEnabled = true;
            DisconnectMenuItem.IsEnabled = false;
            ChangeColorMenuItem.IsEnabled = false;
            ChangeProfilePictureMenuItem.IsEnabled = false;
            JoinRoomMenuItem.IsEnabled = false;
            CreateRoomMenuItem.IsEnabled = false;

            _rooms.Clear();
            _users.Clear();
            ChatTabControl.Items.Clear();
        });
    }

    public void OnLoginSuccess(User user, List<ChatRoom> rooms)
    {
        Dispatcher.Invoke(() =>
        {
            ConnectionStatusIndicator.Fill = Brushes.Green;
            ConnectionStatusText.Text = "Logged in";
            UsernameText.Text = user.Username;
            StatusBarText.Text = $"Logged in as {user.Username}";

            if (!string.IsNullOrEmpty(user.Color))
            {
                var color = (Color)ColorConverter.ConvertFromString(user.Color);
                UserColorRectangle.Fill = new SolidColorBrush(color);
            }

            ConnectMenuItem.IsEnabled = false;
            DisconnectMenuItem.IsEnabled = true;
            ChangeColorMenuItem.IsEnabled = true;
            ChangeProfilePictureMenuItem.IsEnabled = true;
            JoinRoomMenuItem.IsEnabled = true;
            CreateRoomMenuItem.IsEnabled = true;

            _rooms.Clear();
            foreach (var room in rooms)
            {
                _rooms.Add(room);
            }
        });
    }

    public void OnLoginFailed(string reason)
    {
        Dispatcher.Invoke(() =>
        {
            MessageBox.Show($"Login failed: {reason}", "Login Error",
                              MessageBoxButton.OK, MessageBoxImage.Error);
        });
    }

    public void OnRegistrationSuccess(User user)
    {
        Dispatcher.Invoke(() =>
        {
            MessageBox.Show("Registration successful! You are now logged in.", "Success",
                              MessageBoxButton.OK, MessageBoxImage.Information);
            OnLoginSuccess(user, new List<ChatRoom>());
        });
    }

    public void OnRegistrationFailed(string reason)
    {
        Dispatcher.Invoke(() =>
        {
            MessageBox.Show($"Registration failed: {reason}", "Registration Error",
                              MessageBoxButton.OK, MessageBoxImage.Error);
        });
    }

    public void OnMessageReceived(ChatMessage message)
    {
        Dispatcher.Invoke(() =>
        {
            if (message.ChatRoomId.HasValue && _roomMessages.ContainsKey((int)message.ChatRoomId.Value))
            {
                _roomMessages[(int)message.ChatRoomId.Value].Add(message);
            }
        });
    }

    public void OnPrivateMessageReceived(ChatMessage message)
    {
        Dispatcher.Invoke(() =>
        {
            // Handle private message display
            var senderName = message.User.Username;
            var tabName = $"Private: {senderName}";

            // Find or create private message tab
            var existingTab = ChatTabControl.Items.Cast<TabItem>()
                .FirstOrDefault(t => t.Header.ToString() == tabName);

            if (existingTab != null)
            {
                // Add message to existing tab
                var grid = (Grid)existingTab.Content;
                var messagesListBox = (ListBox)grid.Children[0];
                ((ObservableCollection<ChatMessage>)messagesListBox.ItemsSource).Add(message);
            }
        });
    }

    public void OnUserJoinedRoom(int roomId, string username)
    {
        Dispatcher.Invoke(() =>
        {
            StatusBarText.Text = $"{username} joined the room";
        });
    }

    public void OnUserLeftRoom(int roomId, string username)
    {
        Dispatcher.Invoke(() =>
        {
            StatusBarText.Text = $"{username} left the room";
        });
    }

    public void OnRoomCreated(ChatRoom room)
    {
        Dispatcher.Invoke(() =>
        {
            _rooms.Add(room);
        });
    }

    public void OnUserListUpdated(List<User> users)
    {
        Dispatcher.Invoke(() =>
        {
            _users.Clear();
            foreach (var user in users)
            {
                _users.Add(user);
            }
        });
    }

    public void OnProfileUpdated(User user)
    {
        Dispatcher.Invoke(() =>
        {
            if (!string.IsNullOrEmpty(user.Color))
            {
                var color = (Color)ColorConverter.ConvertFromString(user.Color);
                UserColorRectangle.Fill = new SolidColorBrush(color);
            }

            if (!string.IsNullOrEmpty(user.ProfileImageBase64))
            {
                try
                {
                    var imageBytes = Convert.FromBase64String(user.ProfileImageBase64);
                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.StreamSource = new MemoryStream(imageBytes);
                    bitmap.EndInit();
                    ProfileImageBrush.ImageSource = bitmap;
                }
                catch
                {
                    // Handle image loading error
                }
            }
        });
    }

    public void OnError(string error)
    {
        Dispatcher.Invoke(() =>
        {
            StatusBarText.Text = $"Error: {error}";
        });
    }

    protected override void OnClosed(EventArgs e)
    {
        _chatClient?.Disconnect();
        base.OnClosed(e);
    }
}