using DataModel;
using NetworkLib.Models;
using System.Net.Sockets;
using System.Text.Json;

namespace NetworkLib;

public class ChatClient
{
    private TcpClient? _tcpClient;
    private Transfer<ChatRequest>? _requestTransfer;
    private Transfer<ChatResponse>? _responseTransfer;
    private readonly IListener _listener;
    private bool _isConnected;
    private CancellationTokenSource? _cancellationTokenSource;

    public User? CurrentUser { get; private set; }
    public bool IsConnected => _isConnected && _tcpClient?.Connected == true;

    public ChatClient(IListener listener)
    {
        _listener = listener;
    }

    public async Task<bool> ConnectAsync(string serverAddress, int port)
    {
        try
        {
            _tcpClient = new TcpClient();
            await _tcpClient.ConnectAsync(serverAddress, port);

            _requestTransfer = new Transfer<ChatRequest>(_tcpClient);
            _responseTransfer = new Transfer<ChatResponse>(_tcpClient);
            _cancellationTokenSource = new CancellationTokenSource();

            _isConnected = true;

            // Start listening for server messages
            _ = Task.Run(async () => await ListenForMessagesAsync(_cancellationTokenSource.Token));

            _listener.OnConnected();
            return true;
        }
        catch (Exception ex)
        {
            _listener.OnConnectionFailed(ex.Message);
            return false;
        }
    }

    public async Task<bool> LoginAsync(string username, string password)
    {
        if (!IsConnected) return false;

        try
        {
            var loginData = new LoginData { Username = username, Password = password };
            var request = new ChatRequest
            {
                Type = RequestType.Login,
                Data = JsonSerializer.Serialize(loginData)
            };

            // Send request and receive response
            await _requestTransfer.SendOnlyAsync(request);
            var response = await _responseTransfer.ReceiveAsync();

            if (response.Success && response.User != null)
            {
                CurrentUser = response.User;
                _listener.OnLoginSuccess(response.User, response.Rooms ?? new List<ChatRoom>());
                return true;
            }
            else
            {
                _listener.OnLoginFailed(response.Message);
                return false;
            }
        }
        catch (Exception ex)
        {
            _listener.OnError($"Login error: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> RegisterAsync(string username, string password, string color)
    {
        if (!IsConnected) return false;
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrEmpty(password))
        {
            _listener.OnError("Username und Passwort dürfen nicht leer sein");
            return false;
        }

        try
        {
            var registerData = new RegisterData { Username = username, Password = password, Color = color ?? "#FF0000" };
            var request = new ChatRequest
            {
                Type = RequestType.Register,
                Data = JsonSerializer.Serialize(registerData)
            };

            await _requestTransfer.SendOnlyAsync(request);
            var response = await _responseTransfer.ReceiveAsync();

            if (response.Success && response.User != null)
            {
                CurrentUser = response.User;
                _listener.OnRegistrationSuccess(response.User);
                return true;
            }
            else
            {
                _listener.OnRegistrationFailed(response.Message ?? "Unbekannter Registrierungsfehler");
                return false;
            }
        }
        catch (SocketException ex)
        {
            _listener.OnError($"Netzwerkfehler bei der Registrierung: {ex.Message}");
            return false;
        }
        catch (Exception ex)
        {
            _listener.OnError($"Registrierungsfehler: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> SendMessageAsync(int roomId, string content)
    {
        if (!IsConnected || CurrentUser == null) return false;
        if (string.IsNullOrWhiteSpace(content))
        {
            _listener.OnError("Nachrichteninhalt darf nicht leer sein");
            return false;
        }

        try
        {
            var request = new ChatRequest
            {
                Type = RequestType.SendMessage,
                UserId = (int)CurrentUser.Id,
                RoomId = roomId,
                Data = content
            };

            await _requestTransfer.SendOnlyAsync(request);
            var response = await _responseTransfer.ReceiveAsync();

            if (!response.Success && !string.IsNullOrEmpty(response.Message))
            {
                _listener.OnError(response.Message);
            }

            return response.Success;
        }
        catch (SocketException ex)
        {
            _listener.OnError($"Netzwerkfehler beim Senden der Nachricht: {ex.Message}");
            return false;
        }
        catch (Exception ex)
        {
            _listener.OnError($"Fehler beim Senden der Nachricht: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> SendPrivateMessageAsync(int targetUserId, string content)
    {
        if (!IsConnected || CurrentUser == null) return false;

        try
        {
            var request = new ChatRequest
            {
                Type = RequestType.SendPrivateMessage,
                UserId = (int)CurrentUser.Id,
                TargetUserId = targetUserId,
                Data = content
            };

            await _requestTransfer.SendOnlyAsync(request);
            var response = await _responseTransfer.ReceiveAsync();
            return response.Success;
        }
        catch (Exception ex)
        {
            _listener.OnError($"Send private message error: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> JoinRoomAsync(int roomId)
    {
        if (!IsConnected || CurrentUser == null) return false;

        try
        {
            var request = new ChatRequest
            {
                Type = RequestType.JoinRoom,
                UserId = (int)CurrentUser.Id,
                RoomId = roomId
            };

            await _requestTransfer.SendOnlyAsync(request);
            var response = await _responseTransfer.ReceiveAsync();
            return response.Success;
        }
        catch (Exception ex)
        {
            _listener.OnError($"Join room error: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> CreateRoomAsync(string name, string description)
    {
        if (!IsConnected || CurrentUser == null) return false;

        try
        {
            var roomData = new ChatRoom { Name = name, Description = description };
            var request = new ChatRequest
            {
                Type = RequestType.CreateRoom,
                UserId = (int)CurrentUser.Id,
                Data = JsonSerializer.Serialize(roomData)
            };

            await _requestTransfer.SendOnlyAsync(request);
            var response = await _responseTransfer.ReceiveAsync();

            if (response.Success && response.Rooms?.Count > 0)
            {
                _listener.OnRoomCreated(response.Rooms.First());
                return true;
            }

            return response.Success;
        }
        catch (Exception ex)
        {
            _listener.OnError($"Create room error: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> UpdateProfileAsync(string? color = null, string? profileImageBase64 = null)
    {
        if (!IsConnected || CurrentUser == null) return false;

        try
        {
            var profileData = new User
            {
                Id = (int)CurrentUser.Id,
                Color = color ?? CurrentUser.Color,
                ProfileImageBase64 = profileImageBase64 ?? CurrentUser.ProfileImageBase64
            };

            var request = new ChatRequest
            {
                Type = RequestType.UpdateProfile,
                UserId = (int)CurrentUser.Id,
                Data = JsonSerializer.Serialize(profileData)
            };

            await _requestTransfer.SendOnlyAsync(request);
            var response = await _responseTransfer.ReceiveAsync();

            if (response.Success)
            {
                if (color != null) CurrentUser.Color = color;
                if (profileImageBase64 != null) CurrentUser.ProfileImageBase64 = profileImageBase64;
                _listener.OnProfileUpdated(CurrentUser);
            }

            return response.Success;
        }
        catch (Exception ex)
        {
            _listener.OnError($"Update profile error: {ex.Message}");
            return false;
        }
    }

    public async Task<List<ChatMessage>?> GetRoomMessagesAsync(int roomId)
    {
        if (!IsConnected || CurrentUser == null) return null;

        try
        {
            var request = new ChatRequest
            {
                Type = RequestType.GetMessages,
                UserId = (int)CurrentUser.Id,
                RoomId = roomId
            };

            await _requestTransfer.SendOnlyAsync(request);
            var response = await _responseTransfer.ReceiveAsync();
            return response.Success ? response.Messages : null;
        }
        catch (Exception ex)
        {
            _listener.OnError($"Get messages error: {ex.Message}");
            return null;
        }
    }

    private async Task ListenForMessagesAsync(CancellationToken cancellationToken)
    {
        try
        {
            while (!cancellationToken.IsCancellationRequested && IsConnected)
            {
                var response = await _responseTransfer.ReceiveAsync();

                // Handle real-time notifications from server
                if (response.Messages?.Count > 0)
                {
                    foreach (var message in response.Messages)
                    {
                        if (message.PrivateToUserId.HasValue)
                        {
                            _listener.OnPrivateMessageReceived(message);
                        }
                        else
                        {
                            _listener.OnMessageReceived(message);
                        }
                    }
                }

                if (response.Rooms?.Count > 0)
                {
                    foreach (var room in response.Rooms)
                    {
                        _listener.OnRoomCreated(room);
                    }
                }

                if (response.Users?.Count > 0)
                {
                    _listener.OnUserListUpdated(response.Users);
                }

                if (!string.IsNullOrEmpty(response.Message) && !response.Success)
                {
                    _listener.OnError(response.Message);
                }
            }
        }
        catch (Exception ex)
        {
            if (!cancellationToken.IsCancellationRequested)
            {
                _listener.OnError($"Connection lost: {ex.Message}");
                Disconnect();
            }
        }
    }

    public void Disconnect()
    {
        try
        {
            _isConnected = false;
            _cancellationTokenSource?.Cancel();

            if (CurrentUser != null && _requestTransfer != null)
            {
                var disconnectRequest = new ChatRequest
                {
                    Type = RequestType.Disconnect,
                    UserId = (int)CurrentUser.Id
                };
                _requestTransfer.SendOnlyAsync(disconnectRequest).Wait(1000);
            }
        }
        catch
        {
            // Ignore errors during disconnect
        }
        finally
        {
            _requestTransfer?.Dispose();
            _responseTransfer?.Dispose();
            _tcpClient?.Close();
            _cancellationTokenSource?.Dispose();

            CurrentUser = null;
            _listener.OnDisconnected();
        }
    }
}
