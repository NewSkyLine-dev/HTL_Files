using System.Xml.Serialization;

namespace Client;

[XmlRoot]
class Saver
{
    public string LastRoomIP { get; set; } = string.Empty;
}
