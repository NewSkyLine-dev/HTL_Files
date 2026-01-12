
using DataModel;

namespace NetworkLib;

public interface IListener
{
    void OnConnected();
    void OnConnectionFailed(string reason);
    void OnDisconnected();

    void OnLoginSuccess(User user, List<ChatRoom> rooms);
    void OnLoginFailed(string reason);
    void OnRegistrationSuccess(User user);
    void OnRegistrationFailed(string reason);

    void OnMessageReceived(ChatMessage message);
    void OnPrivateMessageReceived(ChatMessage message);
    void OnUserJoinedRoom(int roomId, string username);
    void OnUserLeftRoom(int roomId, string username);
    void OnRoomCreated(ChatRoom room);
    void OnUserListUpdated(List<User> users);
    void OnProfileUpdated(User user);

    void OnError(string error);
}
