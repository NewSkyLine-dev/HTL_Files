using NetworkLib;
using Schiffe_versenken.Models;

namespace Schiffe_versenken.Controller;

/// <summary>
/// Bridges the NetworkLib with game-specific packet types.
/// Implements Receiver to handle incoming messages and exposes typed events.
/// </summary>
public class NetworkController : Receiver, IDisposable
{
    private Transfer? _transfer;
    private Server? _server;

    public event Action<StartPlayerPacket>? OnStartPlayerReceived;
    public event Action<TurnPacket>? OnTurnReceived;
    public event Action<AnswerPacket>? OnAnswerReceived;
    public event Action? OnReadyReceived;
    public event Action? OnConnectionLost;

    public async Task StartHostAsync(int port)
    {
        _server = new Server();
        _transfer = await _server.AcceptAsync(port, this);
    }

    public async Task ConnectToHostAsync(string host, int port)
    {
        _transfer = await Client.ConnectAsync(host, port, this);
    }

    public async Task SendStartPlayerAsync(StartPlayerPacket packet)
    {
        if (_transfer == null) return;
        var msg = MSG.Create(nameof(MessageType.StartPlayer), packet);
        await _transfer.SendAsync(msg);
    }

    public async Task SendTurnAsync(TurnPacket packet)
    {
        if (_transfer == null) return;
        var msg = MSG.Create(nameof(MessageType.Turn), packet);
        await _transfer.SendAsync(msg);
    }

    public async Task SendAnswerAsync(AnswerPacket packet)
    {
        if (_transfer == null) return;
        var msg = MSG.Create(nameof(MessageType.Answer), packet);
        await _transfer.SendAsync(msg);
    }

    public async Task SendReadyAsync()
    {
        if (_transfer == null) return;
        var msg = MSG.Create(nameof(MessageType.Ready));
        await _transfer.SendAsync(msg);
    }

    public void ReceiveMessage(MSG message, Transfer transfer)
    {
        switch (message.Type)
        {
            case nameof(MessageType.StartPlayer):
                var startPlayer = message.GetPayload<StartPlayerPacket>();
                if (startPlayer != null) OnStartPlayerReceived?.Invoke(startPlayer);
                break;
            case nameof(MessageType.Turn):
                var turn = message.GetPayload<TurnPacket>();
                if (turn != null) OnTurnReceived?.Invoke(turn);
                break;
            case nameof(MessageType.Answer):
                var answer = message.GetPayload<AnswerPacket>();
                if (answer != null) OnAnswerReceived?.Invoke(answer);
                break;
            case nameof(MessageType.Ready):
                OnReadyReceived?.Invoke();
                break;
        }
    }

    public void TransferDisconnected(Transfer transfer)
    {
        OnConnectionLost?.Invoke();
    }

    public void Dispose()
    {
        _transfer?.Dispose();
        _server?.Dispose();
    }
}
