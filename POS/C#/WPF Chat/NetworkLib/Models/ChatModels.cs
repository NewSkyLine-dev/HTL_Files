using System.Xml.Serialization;
using DataModel;

namespace NetworkLib.Models;

[XmlRoot]
public enum RequestType
{
    Login,
    Register,
    JoinRoom,
    LeaveRoom,
    SendMessage,
    SendPrivateMessage,
    GetRooms,
    CreateRoom,
    GetMessages,
    UpdateProfile,
    GetUsers,
    Disconnect
}

[XmlRoot]
public class ChatRequest
{
    public ChatRequest() { }
    public RequestType Type { get; set; }
    public string Data { get; set; } = string.Empty;
    public int UserId { get; set; }
    public int? RoomId { get; set; }
    public int? TargetUserId { get; set; }
}

[XmlRoot]
public class ChatResponse
{
    public ChatResponse()
    {
        Rooms = new List<ChatRoom>();
        Messages = new List<ChatMessage>();
        Users = new List<User>();
    }

    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public string Data { get; set; } = string.Empty;
    public List<ChatRoom> Rooms { get; set; }
    public List<ChatMessage> Messages { get; set; }
    public List<User> Users { get; set; }
    public User? User { get; set; }
}

[XmlRoot]
public class LoginData
{
    public LoginData() { }

    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

[XmlRoot]
public class RegisterData
{
    public RegisterData() { }

    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Color { get; set; } = "#FF000000";
}
