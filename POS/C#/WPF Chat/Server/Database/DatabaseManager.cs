using DataModel;
using LinqToDB;
using LinqToDB.Configuration;
using LinqToDB.Data;
using System.Security.Cryptography;
using System.Text;

namespace Server.Database;

public class DatabaseManager
{
    private readonly string _connectionString;

    public DatabaseManager(string dbPath = "chat.db")
    {
        _connectionString = $"Data Source={dbPath}";

        // Configure LinqToDB for SQLite
        DataConnection.DefaultSettings = new LinqToDBSettings("Chat", "SQLite", _connectionString);

        InitializeDatabase();
    }

    private static void InitializeDatabase()
    {
        using var db = new ChatDb();

        // Create tables if they don't exist
        db.CreateTable<User>(tableOptions: TableOptions.CreateIfNotExists);
        db.CreateTable<ChatRoom>(tableOptions: TableOptions.CreateIfNotExists);
        db.CreateTable<ChatMessage>(tableOptions: TableOptions.CreateIfNotExists);
        db.CreateTable<RoomMember>(tableOptions: TableOptions.CreateIfNotExists);

        // Create default "General" room if it doesn't exist
        var generalRoom = db.GetTable<ChatRoom>().FirstOrDefault(r => r.Name == "General");
        if (generalRoom == null)
        {
            db.Insert(new ChatRoom
            {
                Name = "General",
                Description = "Default chat room",
                Created = DateTime.Now
            });
        }
    }

    public async Task<User?> AuthenticateUserAsync(string username, string password)
    {
        var passwordHash = HashPassword(password);
        using var db = new ChatDb();

        return await db.GetTable<User>()
            .Where(u => u.Username == username && u.PasswordHash == passwordHash)
            .FirstOrDefaultAsync();
    }

    public async Task<User?> RegisterUserAsync(string username, string password, string color)
    {
        var passwordHash = HashPassword(password);
        using var db = new ChatDb();

        try
        {
            var newUser = new User
            {
                Username = username,
                PasswordHash = passwordHash,
                Color = color
            };

            var userId = await db.InsertWithInt32IdentityAsync(newUser);
            newUser.Id = userId;

            return newUser;
        }
        catch
        {
            return null; // Username already exists
        }
    }

    private static string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(hashedBytes);
    }

    public async Task<List<ChatRoom>> GetUserRoomsAsync(int userId)
    {
        using var db = new ChatDb();

        return await (from cr in db.GetTable<ChatRoom>()
                      join rm in db.GetTable<RoomMember>() on cr.Id equals rm.RoomId
                      where rm.UserId == userId
                      select cr).ToListAsync();
    }

    public async Task<List<ChatMessage>> GetRoomMessagesAsync(int roomId, int limit = 50)
    {
        using var db = new ChatDb();

        var messages = await db.GetTable<ChatMessage>()
            .Join(
                db.GetTable<User>(),
                cm => cm.UserId,
                u => u.Id,
                (cm, u) => new { cm, u }
            )
            .Where(x => x.cm.ChatRoomId == roomId)
            .OrderByDescending(x => x.cm.Timestamp)
            .Select(x => new ChatMessage
            {
                Id = x.cm.Id,
                UserId = x.cm.UserId,
                //Username = x.u.Username,
                ChatRoomId = x.cm.ChatRoomId,
                Content = x.cm.Content,
                Timestamp = x.cm.Timestamp,
                //UserColor = x.u.Color,
                //UserProfileImageBase64 = x.u.ProfileImageBase64
            })
            .Take(limit)
            .ToListAsync();

        messages.Reverse(); // Show oldest first
        return messages;
    }

    public async Task<int> SaveMessageAsync(int userId, int? roomId, int? privateToUserId, string content)
    {
        using var db = new ChatDb();

        var message = new ChatMessage
        {
            UserId = userId,
            ChatRoomId = roomId,
            PrivateToUserId = privateToUserId,
            Content = content
        };

        return await db.InsertWithInt32IdentityAsync(message);
    }

    public async Task<bool> JoinRoomAsync(int userId, int roomId)
    {
        using var db = new ChatDb();

        try
        {
            // Check if already a member
            var existing = await db.GetTable<RoomMember>()
                .Where(rm => rm.UserId == userId && rm.RoomId == roomId)
                .FirstOrDefaultAsync();

            if (existing == null)
            {
                await db.InsertAsync(new RoomMember
                {
                    UserId = userId,
                    RoomId = roomId
                });
                return true;
            }
            return false;
        }
        catch
        {
            return false;
        }
    }

    public async Task<List<ChatRoom>> GetAllRoomsAsync()
    {
        using var db = new ChatDb();
        return await db.GetTable<ChatRoom>().OrderBy(r => r.Name).ToListAsync();
    }

    public async Task<ChatRoom?> CreateRoomAsync(string name, string description)
    {
        using var db = new ChatDb();

        try
        {
            var newRoom = new ChatRoom
            {
                Name = name,
                Description = description
            };

            var roomId = await db.InsertWithInt32IdentityAsync(newRoom);
            newRoom.Id = roomId;

            return newRoom;
        }
        catch
        {
            return null; // Room name already exists
        }
    }

    public async Task<bool> UpdateUserProfileAsync(int userId, string? color = null, string? profileImageBase64 = null)
    {
        using var db = new ChatDb();

        try
        {
            var user = await db.GetTable<User>().Where(u => u.Id == userId).FirstOrDefaultAsync();
            if (user == null) return false;

            if (color != null) user.Color = color;
            if (profileImageBase64 != null) user.ProfileImageBase64 = profileImageBase64;

            await db.UpdateAsync(user);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<List<ChatMessage>> GetPrivateMessagesAsync(int userId1, int userId2, int limit = 50)
    {
        using var db = new ChatDb();

        var messages = await (from cm in db.GetTable<ChatMessage>()
                              join u in db.GetTable<User>() on cm.UserId equals u.Id
                              where cm.ChatRoomId == null &&
                                    ((cm.UserId == userId1 && cm.PrivateToUserId == userId2) ||
                                     (cm.UserId == userId2 && cm.PrivateToUserId == userId1))
                              orderby cm.Timestamp descending
                              select new ChatMessage
                              {
                                  Id = cm.Id,
                                  UserId = cm.UserId,
                                  //Username = u.Username,
                                  PrivateToUserId = cm.PrivateToUserId,
                                  Content = cm.Content,
                                  Timestamp = cm.Timestamp,
                                  //UserColor = u.Color,
                                  //UserProfileImageBase64 = u.ProfileImageBase64
                              })
                           .Take(limit)
                           .ToListAsync();

        messages.Reverse();
        return messages;
    }

    public async Task<List<User>> GetAllUsersAsync()
    {
        using var db = new ChatDb();
        return await db.GetTable<DataModel.User>().OrderBy(u => u.Username).ToListAsync();
    }

    public async Task UpdateUserLastSeenAsync(int userId)
    {
        using var db = new ChatDb();

        await db.GetTable<User>()
            .Where(u => u.Id == userId)
            .Set(u => u.LastSeen, DateTime.Now)
            .UpdateAsync();
    }
}
