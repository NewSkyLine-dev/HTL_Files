using NetworkLib;
using NetworkLib.Models;
using Server.Database;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;
using DataModel;

namespace Server;

public class ChatServer
{
    private readonly DatabaseManager _database;
    private readonly TcpListener _listener;
    private readonly List<ClientHandler> _clients = new();
    private bool _isRunning;

    public event Action<string>? LogActivity;
    public event Action? ClientConnected;
    public event Action? ClientDisconnected;
    public event Action? MessageSent;

    public ChatServer(int port = 8888)
    {
        _database = new DatabaseManager();
        _listener = new TcpListener(IPAddress.Any, port);
    }

    public async Task StartAsync()
    {
        _listener.Start();
        _isRunning = true;
        LogActivity?.Invoke($"Server started on port {((IPEndPoint)_listener.LocalEndpoint).Port}");

        while (_isRunning)
        {
            try
            {
                var tcpClient = await _listener.AcceptTcpClientAsync();
                var clientHandler = new ClientHandler(tcpClient, _database, this);
                _clients.Add(clientHandler);
                LogActivity?.Invoke($"Client connected: {tcpClient.Client.RemoteEndPoint}");
                ClientConnected?.Invoke();

                _ = Task.Run(async () => await clientHandler.HandleClientAsync());
            }
            catch (ObjectDisposedException)
            {
                break;
            }
            catch (Exception ex)
            {
                LogActivity?.Invoke($"Error accepting client: {ex.Message}");
            }
        }
    }

    public void Stop()
    {
        _isRunning = false;
        _listener?.Stop();

        foreach (var client in _clients.ToList())
        {
            client.Disconnect();
        }
        _clients.Clear();

        LogActivity?.Invoke("Server stopped");
    }

    public void RemoveClient(ClientHandler client)
    {
        _clients.Remove(client);
        ClientDisconnected?.Invoke();
    }

    public async Task BroadcastToRoomAsync(int roomId, ChatMessage message, int? excludeUserId = null)
    {
        var roomClients = _clients.Where(c => c.IsInRoom(roomId) && c.UserId != excludeUserId).ToList();

        foreach (var client in roomClients)
        {
            await client.SendMessageNotificationAsync(message);
        }

        MessageSent?.Invoke();
    }

    public void LogServerActivity(string activity)
    {
        LogActivity?.Invoke(activity);
    }
}

public class ClientHandler
{
    private readonly TcpClient _tcpClient;
    private readonly Transfer<ChatRequest> _requestTransfer;
    private readonly Transfer<ChatResponse> _responseTransfer;
    private readonly DatabaseManager _database;
    private readonly ChatServer _server;
    private readonly HashSet<int> _joinedRooms = new();

    public int? UserId { get; private set; }
    public string? Username { get; private set; }

    public ClientHandler(TcpClient tcpClient, DatabaseManager database, ChatServer server)
    {
        _tcpClient = tcpClient;
        _database = database;
        _server = server;
        _requestTransfer = new Transfer<ChatRequest>(tcpClient);
        _responseTransfer = new Transfer<ChatResponse>(tcpClient);
    }

    public async Task HandleClientAsync()
    {
        try
        {
            while (_tcpClient.Connected)
            {
                var request = await _requestTransfer.ReceiveAsync();
                var response = await ProcessRequestAsync(request);
                await _responseTransfer.SendOnlyAsync(response);
            }
        }
        catch (Exception ex)
        {
            _server.LogServerActivity($"Client error: {ex.Message}");
        }
        finally
        {
            Disconnect();
        }
    }

    private async Task<ChatResponse> ProcessRequestAsync(ChatRequest request)
    {
        try
        {
            return request.Type switch
            {
                RequestType.Login => await HandleLoginAsync(request),
                RequestType.Register => await HandleRegisterAsync(request),
                RequestType.GetRooms => await HandleGetRoomsAsync(request),
                RequestType.JoinRoom => await HandleJoinRoomAsync(request),
                RequestType.SendMessage => await HandleSendMessageAsync(request),
                RequestType.GetMessages => await HandleGetMessagesAsync(request),
                RequestType.CreateRoom => await HandleCreateRoomAsync(request),
                RequestType.UpdateProfile => await HandleUpdateProfileAsync(request),
                RequestType.SendPrivateMessage => await HandleSendPrivateMessageAsync(request),
                RequestType.GetUsers => await HandleGetUsersAsync(request),
                RequestType.Disconnect => HandleDisconnectRequest(request),
                _ => new ChatResponse { Success = false, Message = "Unknown request type" }
            };
        }
        catch (Exception ex)
        {
            _server.LogServerActivity($"Error processing request: {ex.Message}");
            return new ChatResponse { Success = false, Message = "Server error occurred" };
        }
    }

    private async Task<ChatResponse> HandleLoginAsync(ChatRequest request)
    {
        var loginData = JsonSerializer.Deserialize<LoginData>(request.Data);
        if (loginData == null)
            return new ChatResponse { Success = false, Message = "Invalid login data" };

        var user = await _database.AuthenticateUserAsync(loginData.Username, loginData.Password);
        if (user != null)
        {
            UserId = (int)user.Id;
            Username = user.Username;
            await _database.UpdateUserLastSeenAsync((int)user.Id);

            // Get user's rooms
            var rooms = await _database.GetUserRoomsAsync((int)user.Id);

            // Auto-join user to their rooms
            foreach (var room in rooms)
            {
                _joinedRooms.Add((int)room.Id);
            }

            _server.LogServerActivity($"User logged in: {user.Username}");
            return new ChatResponse { Success = true, Message = "Login successful", User = user, Rooms = rooms };
        }

        return new ChatResponse { Success = false, Message = "Invalid username or password" };
    }

    private async Task<ChatResponse> HandleRegisterAsync(ChatRequest request)
    {
        var registerData = JsonSerializer.Deserialize<RegisterData>(request.Data);
        if (registerData == null)
            return new ChatResponse { Success = false, Message = "Invalid registration data" };

        var user = await _database.RegisterUserAsync(registerData.Username, registerData.Password, registerData.Color);
        if (user != null)
        {
            UserId = (int)user.Id;
            Username = user.Username;

            // Auto-join General room
            await _database.JoinRoomAsync((int)user.Id, 1);
            _joinedRooms.Add(1);

            // Get user's rooms (should include General room)
            var rooms = await _database.GetUserRoomsAsync((int)user.Id);

            _server.LogServerActivity($"User registered: {user.Username}");
            return new ChatResponse { Success = true, Message = "Registration successful", User = user, Rooms = rooms };
        }

        return new ChatResponse { Success = false, Message = "Username already exists" };
    }

    private async Task<ChatResponse> HandleGetRoomsAsync(ChatRequest request)
    {
        if (!UserId.HasValue)
            return new ChatResponse { Success = false, Message = "Not authenticated" };

        var rooms = await _database.GetUserRoomsAsync(UserId.Value);
        return new ChatResponse { Success = true, Rooms = rooms };
    }

    private async Task<ChatResponse> HandleJoinRoomAsync(ChatRequest request)
    {
        if (!UserId.HasValue || !request.RoomId.HasValue)
            return new ChatResponse { Success = false, Message = "Invalid request" };

        var success = await _database.JoinRoomAsync(UserId.Value, request.RoomId.Value);
        if (success)
        {
            _joinedRooms.Add(request.RoomId.Value);
            _server.LogServerActivity($"{Username} joined room {request.RoomId}");
        }

        return new ChatResponse { Success = success, Message = success ? "Joined room" : "Already in room" };
    }

    private async Task<ChatResponse> HandleSendMessageAsync(ChatRequest request)
    {
        if (!UserId.HasValue || !request.RoomId.HasValue)
            return new ChatResponse { Success = false, Message = "Invalid request" };

        var messageId = await _database.SaveMessageAsync(UserId.Value, request.RoomId.Value, null, request.Data);

        // Get the complete message with user info for broadcasting
        var messages = await _database.GetRoomMessagesAsync(request.RoomId.Value, 1);
        if (messages.Any())
        {
            var message = messages.First();
            await _server.BroadcastToRoomAsync(request.RoomId.Value, message, UserId.Value);
            _server.LogServerActivity($"{Username} sent message to room {request.RoomId}");
        }

        return new ChatResponse { Success = true, Message = "Message sent" };
    }

    private async Task<ChatResponse> HandleGetMessagesAsync(ChatRequest request)
    {
        if (!request.RoomId.HasValue)
            return new ChatResponse { Success = false, Message = "Room ID required" };

        var messages = await _database.GetRoomMessagesAsync(request.RoomId.Value);
        return new ChatResponse { Success = true, Messages = messages };
    }

    private async Task<ChatResponse> HandleCreateRoomAsync(ChatRequest request)
    {
        if (!UserId.HasValue)
            return new ChatResponse { Success = false, Message = "Not authenticated" };

        var roomData = JsonSerializer.Deserialize<ChatRoom>(request.Data);
        if (roomData == null)
            return new ChatResponse { Success = false, Message = "Invalid room data" };

        var room = await _database.CreateRoomAsync(roomData.Name, roomData.Description);
        if (room != null)
        {
            await _database.JoinRoomAsync(UserId.Value, (int)room.Id);
            _joinedRooms.Add((int)room.Id);
            _server.LogServerActivity($"{Username} created room: {room.Name}");
            return new ChatResponse { Success = true, Message = "Room created", Rooms = new List<ChatRoom> { room } };
        }

        return new ChatResponse { Success = false, Message = "Room name already exists" };
    }

    private async Task<ChatResponse> HandleUpdateProfileAsync(ChatRequest request)
    {
        if (!UserId.HasValue)
            return new ChatResponse { Success = false, Message = "Not authenticated" };

        var profileData = JsonSerializer.Deserialize<User>(request.Data);
        if (profileData == null)
            return new ChatResponse { Success = false, Message = "Invalid profile data" };

        var success = await _database.UpdateUserProfileAsync(UserId.Value, profileData.Color, profileData.ProfileImageBase64);
        if (success)
        {
            _server.LogServerActivity($"{Username} updated profile");
        }

        return new ChatResponse { Success = success, Message = success ? "Profile updated" : "Update failed" };
    }

    private async Task<ChatResponse> HandleSendPrivateMessageAsync(ChatRequest request)
    {
        if (!UserId.HasValue || !request.TargetUserId.HasValue)
            return new ChatResponse { Success = false, Message = "Invalid request" };

        await _database.SaveMessageAsync(UserId.Value, null, request.TargetUserId.Value, request.Data);
        _server.LogServerActivity($"{Username} sent private message to user {request.TargetUserId}");

        return new ChatResponse { Success = true, Message = "Private message sent" };
    }

    private async Task<ChatResponse> HandleGetUsersAsync(ChatRequest request)
    {
        var users = await _database.GetAllUsersAsync();
        return new ChatResponse { Success = true, Users = users };
    }

    private ChatResponse HandleDisconnectRequest(ChatRequest request)
    {
        _server.LogServerActivity($"{Username} disconnecting");
        return new ChatResponse { Success = true, Message = "Disconnect acknowledged" };
    }

    public bool IsInRoom(int roomId) => _joinedRooms.Contains(roomId);

    public async Task SendMessageNotificationAsync(ChatMessage message)
    {
        try
        {
            var notification = new ChatResponse
            {
                Success = true,
                Messages = new List<ChatMessage> { message }
            };
            await _responseTransfer.SendOnlyAsync(notification);
        }
        catch
        {
            // Client disconnected
        }
    }

    public void Disconnect()
    {
        if (UserId.HasValue)
        {
            _server.LogServerActivity($"User disconnected: {Username}");
        }

        _server.RemoveClient(this);
        _requestTransfer?.Dispose();
        _responseTransfer?.Dispose();
        _tcpClient?.Close();
    }
}
