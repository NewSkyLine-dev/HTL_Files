using System.IO;
using System.Xml.Serialization;

namespace Bilderverwaltungsprogramm;

[XmlRoot("Album")]
public class AlbumData
{
    [XmlElement("Name")]
    public string Name { get; set; }

    [XmlElement("Path")]
    public string Path { get; set; }

    [XmlArray("Images")]
    [XmlArrayItem("Image")]
    public List<string> ImagePaths { get; set; }

    public AlbumData()
    {
        Name = string.Empty;
        Path = string.Empty;
        ImagePaths = new List<string>();
    }

    public AlbumData(string name, string path)
    {
        Name = name;
        Path = path;
        ImagePaths = new List<string>();
    }

    public void RefreshImagePaths()
    {
        ImagePaths.Clear();
        if (Directory.Exists(Path))
        {
            var supportedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var files = Directory.GetFiles(Path)
                .Where(f => supportedExtensions.Contains(System.IO.Path.GetExtension(f).ToLower()))
                .ToList();
            ImagePaths.AddRange(files);
        }
    }
}
