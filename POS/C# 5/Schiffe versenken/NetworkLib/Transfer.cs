using System.Net.Sockets;
using System.Text;
using System.Xml.Serialization;

namespace NetworkLib;

/// <summary>
/// Manages a single TCP connection. Sends and receives XML-serialized MSG objects
/// using Base64-encoded line framing for reliable transport.
/// </summary>
public class Transfer : IDisposable
{
    private readonly TcpClient _client;
    private readonly NetworkStream _stream;
    private readonly XmlSerializer _msgSerializer = new(typeof(MSG));
    private readonly Receiver _receiver;
    private readonly CancellationTokenSource _cts = new();
    private readonly SemaphoreSlim _writeLock = new(1, 1);
    private bool _disposed;

    public Transfer(TcpClient client, Receiver receiver)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
        _receiver = receiver ?? throw new ArgumentNullException(nameof(receiver));
        _stream = client.GetStream();
    }

    /// <summary>
    /// Starts the background listener that reads incoming messages.
    /// </summary>
    public void StartListening()
    {
        Task.Run(() => ListenAsync(_cts.Token));
    }

    /// <summary>
    /// Serializes and sends a MSG to the remote endpoint.
    /// </summary>
    public async Task SendAsync(MSG message)
    {
        if (_disposed) throw new ObjectDisposedException(nameof(Transfer));

        await _writeLock.WaitAsync();
        try
        {
            using var sw = new StringWriter();
            _msgSerializer.Serialize(sw, message);
            var base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(sw.ToString()));
            var lineBytes = Encoding.UTF8.GetBytes(base64 + "\n");
            await _stream.WriteAsync(lineBytes);
            await _stream.FlushAsync();
        }
        finally
        {
            _writeLock.Release();
        }
    }

    private async Task ListenAsync(CancellationToken ct)
    {
        var reader = new StreamReader(_stream, Encoding.UTF8, false, 1024, leaveOpen: true);
        try
        {
            while (!ct.IsCancellationRequested)
            {
                var line = await reader.ReadLineAsync(ct);
                if (line == null) break;

                var xml = Encoding.UTF8.GetString(Convert.FromBase64String(line));
                using var sr = new StringReader(xml);
                if (_msgSerializer.Deserialize(sr) is MSG msg)
                {
                    _receiver.ReceiveMessage(msg, this);
                }
            }
        }
        catch (OperationCanceledException) { }
        catch (IOException) { }
        catch (ObjectDisposedException) { }
        finally
        {
            reader.Dispose();
        }

        if (!_disposed)
        {
            _receiver.TransferDisconnected(this);
        }
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            _disposed = true;
            _cts.Cancel();
            _stream.Dispose();
            _client.Dispose();
            _cts.Dispose();
            _writeLock.Dispose();
        }
    }
}
