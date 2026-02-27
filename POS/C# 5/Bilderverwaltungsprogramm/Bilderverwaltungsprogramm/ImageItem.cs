using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Media.Imaging;

namespace Bilderverwaltungsprogramm;

public class ImageItem : INotifyPropertyChanged
{
    private string _filePath;
    private string _displayName;
    private BitmapImage? _thumbnail;

    public string FilePath
    {
        get => _filePath;
        set
        {
            _filePath = value;
            OnPropertyChanged();
            DisplayName = Path.GetFileNameWithoutExtension(value);
        }
    }

    public string DisplayName
    {
        get => _displayName;
        set
        {
            _displayName = value;
            OnPropertyChanged();
        }
    }

    public BitmapImage? Thumbnail
    {
        get => _thumbnail;
        set
        {
            _thumbnail = value;
            OnPropertyChanged();
        }
    }

    public ImageItem(string filePath)
    {
        _filePath = filePath;
        _displayName = Path.GetFileNameWithoutExtension(filePath);
        LoadThumbnail();
    }

    private void LoadThumbnail()
    {
        try
        {
            if (File.Exists(FilePath))
            {
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(Directory.GetCurrentDirectory() + "\\" + FilePath, UriKind.Absolute);
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.DecodePixelWidth = 150;
                bitmap.EndInit();
                bitmap.Freeze();
                Thumbnail = bitmap;
            }
        }
        catch
        {
            Thumbnail = null;
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
