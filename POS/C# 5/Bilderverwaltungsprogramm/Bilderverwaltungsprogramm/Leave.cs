namespace Bilderverwaltungsprogramm;

public class Leave(string name, string path)
{
    public List<Leave>? Children { get; private set; } = [];
    public string? Name { get; private set; } = name;
    public string? Path { get; private set; } = path;
    public bool IsAlbum { get; private set; } = false;

    public void AddPicture(string name, string path)
    {
        Children?.Add(new Leave(name, path));
    }

    public void AddAlbum(string name, string path)
    {
        var album = new Leave(name, path) { IsAlbum = true };
        Children?.Add(album);
    }

    public List<string> GetAlbumNames()
    {
        return Children?.Where(c => c.IsAlbum).Select(c => c.Name ?? "").ToList() ?? [];
    }
}
