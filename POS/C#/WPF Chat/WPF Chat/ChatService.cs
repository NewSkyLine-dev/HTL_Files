using DataModel;

namespace Server;

public class ChatService
{
    public void FetchChats()
    {
        using var db = new ChatdbDb("DefaultConnection"); ;

        var chats = db.Chats.ToList();

        foreach (var chat in chats)
        {
            Console.WriteLine($"Chat ID: {chat.ChatId}, User ID: {chat.UserId}, Message: {chat.Content}");
        }
    }
}
