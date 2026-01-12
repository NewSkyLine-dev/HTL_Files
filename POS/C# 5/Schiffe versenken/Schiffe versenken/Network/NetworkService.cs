using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Schiffe_versenken.Network;

public class NetworkService : IDisposable
{
    private TcpListener? _listener;
    private TcpClient? _client;
    private NetworkStream? _stream;
    private CancellationTokenSource? _cancellationTokenSource;
    private bool _isDisposed;

    public event Action<string>? OnMessageReceived;
    public event Action? OnConnectionLost;

    public bool IsConnected => _client?.Connected ?? false;

    public async Task StartHostAsync(int port)
    {
        _listener = new TcpListener(IPAddress.Any, port);
        _listener.Start();

        _client = await _listener.AcceptTcpClientAsync();
        _stream = _client.GetStream();

        StartListening();
    }

    public async Task ConnectToHostAsync(string host, int port)
    {
        _client = new TcpClient();
        await _client.ConnectAsync(host, port);
        _stream = _client.GetStream();

        StartListening();
    }

    private void StartListening()
    {
        _cancellationTokenSource = new CancellationTokenSource();
        Task.Run(async () => await ListenForMessagesAsync(_cancellationTokenSource.Token));
    }

    private async Task ListenForMessagesAsync(CancellationToken cancellationToken)
    {
        var buffer = new byte[1024];

        try
        {
            while (!cancellationToken.IsCancellationRequested && _stream != null)
            {
                int bytesRead = await _stream.ReadAsync(buffer, 0, buffer.Length, cancellationToken);

                if (bytesRead == 0)
                {
                    OnConnectionLost?.Invoke();
                    break;
                }

                string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                OnMessageReceived?.Invoke(message);
            }
        }
        catch (Exception)
        {
            OnConnectionLost?.Invoke();
        }
    }

    public void SendMessage(string message)
    {
        if (_stream == null || !IsConnected)
            return;

        var buffer = Encoding.UTF8.GetBytes(message);
        _stream.Write(buffer, 0, buffer.Length);
        _stream.Flush();
    }

    public void Dispose()
    {
        if (_isDisposed)
            return;

        _isDisposed = true;
        _cancellationTokenSource?.Cancel();
        _stream?.Dispose();
        _client?.Dispose();
        _listener?.Stop();
    }
}
