using System.Xml.Serialization;

namespace NetworkLib;

/// <summary>
/// Generic message envelope for XML-serialized network communication.
/// Wraps a typed payload with a message type discriminator.
/// </summary>
[Serializable]
public class MSG
{
    public string Type { get; set; } = "";
    public string Payload { get; set; } = "";

    /// <summary>
    /// Creates a MSG with a typed payload serialized to XML.
    /// </summary>
    public static MSG Create<T>(string type, T data)
    {
        var serializer = new XmlSerializer(typeof(T));
        using var writer = new StringWriter();
        serializer.Serialize(writer, data);
        return new MSG { Type = type, Payload = writer.ToString() };
    }

    /// <summary>
    /// Creates a MSG with no payload (signal-only message).
    /// </summary>
    public static MSG Create(string type)
    {
        return new MSG { Type = type };
    }

    /// <summary>
    /// Deserializes the payload back into the specified type.
    /// </summary>
    public T? GetPayload<T>() where T : class
    {
        if (string.IsNullOrEmpty(Payload)) return null;
        var serializer = new XmlSerializer(typeof(T));
        using var reader = new StringReader(Payload);
        return serializer.Deserialize(reader) as T;
    }
}
