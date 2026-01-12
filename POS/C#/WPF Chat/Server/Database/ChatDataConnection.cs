using LinqToDB;
using LinqToDB.Data;
using DataModel;

namespace Server.Database;

public class ChatDataConnection : DataConnection
{
    public ChatDataConnection() : base("SQLite")
    {
    }

    public ChatDataConnection(string connectionString) : base("SQLite", connectionString)
    {
    }

    public ITable<User> Users => this.GetTable<User>();
    public ITable<ChatRoom> ChatRooms => this.GetTable<ChatRoom>();
    public ITable<ChatMessage> ChatMessages => this.GetTable<ChatMessage>();
    public ITable<RoomMember> RoomMembers => this.GetTable<RoomMember>();
}
