using System.Net;
using System.Net.Sockets;

namespace NetworkLib;

/// <summary>
/// TCP server that listens for incoming connections and creates Transfer instances.
/// </summary>
public class Server : IDisposable
{
    private TcpListener? _listener;
    private bool _disposed;

    /// <summary>
    /// Starts listening on the specified port and waits for a single client to connect.
    /// Returns a Transfer for communicating with the connected client.
    /// </summary>
    public async Task<Transfer> AcceptAsync(int port, Receiver receiver)
    {
        _listener = new TcpListener(IPAddress.Any, port);
        _listener.Start();
        var client = await _listener.AcceptTcpClientAsync();
        var transfer = new Transfer(client, receiver);
        transfer.StartListening();
        return transfer;
    }

    public void Stop()
    {
        _listener?.Stop();
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            _disposed = true;
            _listener?.Stop();
        }
    }
}
