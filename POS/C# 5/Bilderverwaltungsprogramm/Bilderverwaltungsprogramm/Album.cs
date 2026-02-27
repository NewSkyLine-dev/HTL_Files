using System.Xml.Serialization;

namespace Bilderverwaltungsprogramm;

[XmlRoot("Album")]
public class Album
{
    [XmlElement("Name")]
    public string Name { get; set; }

    [XmlArray("Unterordner")]
    public List<string> Unterordner { get; set; }
    
    public Album(string name, List<string> unterordner)
    {
        Name = name;
        Unterordner = unterordner;
    }

    // Parameterloser Konstruktor für die XML-Serialisierung
    public Album()
    {
        Unterordner = [];
    }
}
