using System.Net.Sockets;

namespace NetworkLib;

/// <summary>
/// TCP client that connects to a remote server and creates a Transfer instance.
/// </summary>
public static class Client
{
    /// <summary>
    /// Connects to a host and returns a Transfer for communicating with the server.
    /// </summary>
    public static async Task<Transfer> ConnectAsync(string host, int port, Receiver receiver)
    {
        var tcpClient = new TcpClient();
        await tcpClient.ConnectAsync(host, port);
        var transfer = new Transfer(tcpClient, receiver);
        transfer.StartListening();
        return transfer;
    }
}
