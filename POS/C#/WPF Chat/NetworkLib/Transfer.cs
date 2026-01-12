using System.Collections.Concurrent;
using System.Net.Sockets;
using System.Xml.Serialization;

namespace NetworkLib;

public class Transfer<T> : IDisposable
{
    private readonly TcpClient _client;
    private readonly StreamReader _reader;
    private readonly StreamWriter _writer;
    private readonly XmlSerializer _serializer;
    private readonly ConcurrentDictionary<Guid, TaskCompletionSource<T>> _pendingResponses;
    private readonly Task _listenerTask;
    private readonly CancellationTokenSource _cts;
    private bool _disposed = false;

    public Transfer(TcpClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
        _reader = new StreamReader(client.GetStream());
        _writer = new StreamWriter(client.GetStream());
        _serializer = new XmlSerializer(typeof(T));
        _pendingResponses = new ConcurrentDictionary<Guid, TaskCompletionSource<T>>();
        _cts = new CancellationTokenSource();

        // Start background listener task
        _listenerTask = Task.Run(() => ListenForResponsesAsync(_cts.Token));
    }

    public async Task<T> SendAsync(T data)
    {
        if (_disposed) throw new ObjectDisposedException(nameof(Transfer<T>));

        var requestId = Guid.NewGuid();
        var wrapper = new MessageWrapper<T> { Id = requestId, Data = data };
        var tcs = new TaskCompletionSource<T>();
        _pendingResponses[requestId] = tcs;

        try
        {
            // Serialize and send the wrapped data
            using var stringWriter = new StringWriter();
            new XmlSerializer(typeof(MessageWrapper<T>)).Serialize(stringWriter, wrapper);
            var xmlData = stringWriter.ToString();
            await _writer.WriteLineAsync(xmlData);
            await _writer.FlushAsync();

            // Wait for the response with a timeout
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));
            var completedTask = await Task.WhenAny(tcs.Task, Task.Delay(Timeout.Infinite, cts.Token));

            if (completedTask == tcs.Task)
            {
                return await tcs.Task;
            }
            else
            {
                _pendingResponses.TryRemove(requestId, out _);
                throw new TimeoutException("Response timeout");
            }
        }
        catch (Exception)
        {
            _pendingResponses.TryRemove(requestId, out _);
            throw;
        }
    }

    public async Task SendOnlyAsync(T data)
    {
        if (_disposed) throw new ObjectDisposedException(nameof(Transfer<T>));

        using var stringWriter = new StringWriter();
        var wrapper = new MessageWrapper<T> { Id = Guid.NewGuid(), Data = data, NoResponseExpected = true };
        new XmlSerializer(typeof(MessageWrapper<T>)).Serialize(stringWriter, wrapper);
        var xmlData = stringWriter.ToString();
        await _writer.WriteLineAsync(xmlData);
        await _writer.FlushAsync();
    }

    public async Task<T> ReceiveAsync()
    {
        if (_disposed) throw new ObjectDisposedException(nameof(Transfer<T>));

        var response = await _reader.ReadLineAsync() ?? throw new InvalidOperationException("No data received");
        var stringReader = new StringReader(response);

        // Try to deserialize as a wrapper first
        try
        {
            var wrapperSerializer = new XmlSerializer(typeof(MessageWrapper<T>));
            if (wrapperSerializer.CanDeserialize(new System.Xml.XmlTextReader(stringReader)))
            {
                stringReader.Close();
                stringReader = new StringReader(response);
                var wrapper = (MessageWrapper<T>)wrapperSerializer.Deserialize(stringReader)!;
                return wrapper.Data;
            }
        }
        catch
        {
            // If it's not a wrapper, try deserializing as the original type
            stringReader.Close();
            stringReader = new StringReader(response);
        }

        var result = _serializer.Deserialize(stringReader);
        return result != null ? (T)result : throw new InvalidOperationException("Failed to deserialize data");
    }

    private async Task ListenForResponsesAsync(CancellationToken cancellationToken)
    {
        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var line = await _reader.ReadLineAsync(cancellationToken);
                if (line == null) break;

                var stringReader = new StringReader(line);
                var wrapperSerializer = new XmlSerializer(typeof(MessageWrapper<T>));

                if (wrapperSerializer.CanDeserialize(new System.Xml.XmlTextReader(stringReader)))
                {
                    stringReader.Close();
                    stringReader = new StringReader(line);
                    var wrapper = (MessageWrapper<T>)wrapperSerializer.Deserialize(stringReader)!;

                    // If this is a response to a request we're waiting for, complete the task
                    if (_pendingResponses.TryRemove(wrapper.Id, out var tcs))
                    {
                        tcs.SetResult(wrapper.Data);
                    }
                }
                else
                {
                    // For backward compatibility: handle non-wrapped messages
                    stringReader.Close();
                    stringReader = new StringReader(line);
                    var result = _serializer.Deserialize(stringReader);

                    if (result != null)
                    {
                        // We can't match this to a specific request, so complete the oldest pending request
                        var oldest = _pendingResponses.FirstOrDefault();
                        if (!oldest.Equals(default(KeyValuePair<Guid, TaskCompletionSource<T>>)))
                        {
                            if (_pendingResponses.TryRemove(oldest.Key, out var oldestTcs))
                            {
                                oldestTcs.SetResult((T)result);
                            }
                        }
                    }
                }
                stringReader.Dispose();
            }
        }
        catch (OperationCanceledException) { }
        catch (Exception)
        {
            // Handle exceptions - they'll cause pending requests to time out
        }
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            _cts.Cancel();
            _reader?.Dispose();
            _writer?.Dispose();
            _cts.Dispose();
            _disposed = true;

            // Complete all pending tasks with a failure
            foreach (var key in _pendingResponses.Keys)
            {
                if (_pendingResponses.TryRemove(key, out var tcs))
                {
                    tcs.TrySetException(new ObjectDisposedException(nameof(Transfer<T>)));
                }
            }
        }
    }
}

[Serializable]
public class MessageWrapper<T>
{
    public Guid Id { get; set; }
    public T Data { get; set; }
    public bool NoResponseExpected { get; set; }
}