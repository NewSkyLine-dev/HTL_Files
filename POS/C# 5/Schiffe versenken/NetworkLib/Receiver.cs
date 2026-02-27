namespace NetworkLib;

/// <summary>
/// Callback interface for handling incoming network messages and disconnection events.
/// </summary>
public interface Receiver
{
    void ReceiveMessage(MSG message, Transfer transfer);
    void TransferDisconnected(Transfer transfer);
}
