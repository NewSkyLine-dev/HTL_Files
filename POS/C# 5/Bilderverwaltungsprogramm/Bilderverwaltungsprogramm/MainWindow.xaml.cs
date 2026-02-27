using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;
using Microsoft.Win32;

namespace Bilderverwaltungsprogramm;

public partial class MainWindow : Window, INotifyPropertyChanged
{
    public static RoutedCommand MoveImagesCommand { get; } = new RoutedCommand();
    public static RoutedCommand RotateClockwiseCommand { get; } = new RoutedCommand();
    public static RoutedCommand RotateCounterClockwiseCommand { get; } = new RoutedCommand();
    public static RoutedCommand Rotate180Command { get; } = new RoutedCommand();

    private const string ImagesRootFolder = "Images";
    private const string DataFileName = "albums.xml";

    private ObservableCollection<string> _albums;
    public ObservableCollection<string> Albums
    {
        get => _albums;
        set
        {
            _albums = value;
            OnPropertyChanged();
        }
    }

    private string _currentAlbumName;
    public string CurrentAlbumName
    {
        get => _currentAlbumName;
        set
        {
            _currentAlbumName = value;
            OnPropertyChanged();
            LoadImagesForCurrentAlbum();
        }
    }

    private ObservableCollection<ImageItem> _currentImages;
    public ObservableCollection<ImageItem> CurrentImages
    {
        get => _currentImages;
        set
        {
            _currentImages = value;
            OnPropertyChanged();
        }
    }

    private List<AlbumData> _albumsData;

    public MainWindow()
    {
        InitializeComponent();
        DataContext = this;

        _albums = [];
        _currentImages = [];
        _albumsData = [];

        SetupCommands();
        InitializeApplication();
    }

    private void SetupCommands()
    {
        MoveImagesCommand.InputGestures.Add(new KeyGesture(Key.M, ModifierKeys.Control));
        RotateClockwiseCommand.InputGestures.Add(new KeyGesture(Key.R, ModifierKeys.Control));
        RotateCounterClockwiseCommand.InputGestures.Add(new KeyGesture(Key.R, ModifierKeys.Control | ModifierKeys.Shift));
        Rotate180Command.InputGestures.Add(new KeyGesture(Key.T, ModifierKeys.Control));
    }

    private void InitializeApplication()
    {
        if (!Directory.Exists(ImagesRootFolder))
        {
            Directory.CreateDirectory(ImagesRootFolder);
        }

        LoadAlbumsFromXml();
    }

    private void LoadAlbumsFromXml()
    {
        if (File.Exists(DataFileName))
        {
            try
            {
                XmlSerializer serializer = new(typeof(List<AlbumData>));
                using FileStream fs = new(DataFileName, FileMode.Open);
                _albumsData = (List<AlbumData>)serializer.Deserialize(fs) ?? [];
            }
            catch
            {
                _albumsData = [];
            }
        }

        Albums.Clear();
        foreach (var album in _albumsData)
        {
            if (!Directory.Exists(album.Path))
            {
                Directory.CreateDirectory(album.Path);
            }
            album.RefreshImagePaths();
            Albums.Add(album.Name);
        }
    }

    private void SaveAlbumsToXml()
    {
        try
        {
            foreach (var album in _albumsData)
            {
                album.RefreshImagePaths();
            }

            XmlSerializer serializer = new(typeof(List<AlbumData>));
            using FileStream fs = new(DataFileName, FileMode.Create);
            serializer.Serialize(fs, _albumsData);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Fehler beim Speichern: {ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void NewAlbum_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        AlbumDialog dialog = new("Name des Albums eingeben")
        {
            Owner = this
        };

        if (dialog.ShowDialog() == true && dialog.NewAlbum != null)
        {
            string albumName = dialog.NewAlbum.Name?.Trim();
            if (!string.IsNullOrWhiteSpace(albumName))
            {
                if (_albumsData.Any(a => a.Name.Equals(albumName, StringComparison.OrdinalIgnoreCase)))
                {
                    MessageBox.Show("Ein Album mit diesem Namen existiert bereits!", "Fehler", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                string albumPath = Path.Combine(ImagesRootFolder, albumName);
                Directory.CreateDirectory(albumPath);

                AlbumData newAlbum = new(albumName, albumPath);
                _albumsData.Add(newAlbum);
                Albums.Add(albumName);

                MessageBox.Show($"Album '{albumName}' wurde erfolgreich erstellt.", "Erfolg", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }

    private void AddImages_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        if (string.IsNullOrEmpty(CurrentAlbumName))
        {
            MessageBox.Show("Bitte wählen Sie zuerst ein Album aus.", "Kein Album ausgewählt", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        OpenFileDialog openFileDialog = new()
        {
            Filter = "ZIP-Dateien (*.zip)|*.zip",
            Title = "ZIP-Datei mit Bildern auswählen"
        };

        if (openFileDialog.ShowDialog() == true)
        {
            try
            {
                var currentAlbum = _albumsData.FirstOrDefault(a => a.Name == CurrentAlbumName);
                if (currentAlbum != null)
                {
                    using ZipArchive archive = ZipFile.OpenRead(openFileDialog.FileName);
                    var supportedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                    var imageEntries = archive.Entries.Where(e =>
                        supportedExtensions.Contains(Path.GetExtension(e.Name).ToLower()) &&
                        !string.IsNullOrEmpty(e.Name));

                    int count = 0;
                    foreach (var entry in imageEntries)
                    {
                        string destinationPath = Path.Combine(currentAlbum.Path, entry.Name);
                        entry.ExtractToFile(destinationPath, true);
                        count++;
                    }

                    LoadImagesForCurrentAlbum();
                    MessageBox.Show($"{count} Bild(er) wurden erfolgreich hinzugefügt.", "Erfolg", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Hinzufügen der Bilder: {ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    private void MoveImages_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        if (lstImages.SelectedItems.Count == 0)
        {
            MessageBox.Show("Bitte wählen Sie mindestens ein Bild aus.", "Keine Auswahl", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        var albumSelection = new AlbumSelectionDialog(_albumsData.Select(a => a.Name).ToList(), CurrentAlbumName)
        {
            Owner = this
        };

        if (albumSelection.ShowDialog() == true && !string.IsNullOrEmpty(albumSelection.SelectedAlbum))
        {
            var targetAlbum = _albumsData.FirstOrDefault(a => a.Name == albumSelection.SelectedAlbum);
            if (targetAlbum != null)
            {
                try
                {
                    var selectedImages = lstImages.SelectedItems.Cast<ImageItem>().ToList();
                    foreach (var image in selectedImages)
                    {
                        string fileName = Path.GetFileName(image.FilePath);
                        string newPath = Path.Combine(targetAlbum.Path, fileName);
                        File.Move(image.FilePath, newPath, true);
                    }

                    LoadImagesForCurrentAlbum();
                    MessageBox.Show($"{selectedImages.Count} Bild(er) wurden verschoben.", "Erfolg", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Fehler beim Verschieben: {ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }

    private void DeleteImages_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        if (lstImages.SelectedItems.Count == 0)
        {
            MessageBox.Show("Bitte wählen Sie mindestens ein Bild aus.", "Keine Auswahl", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        var result = MessageBox.Show($"Möchten Sie {lstImages.SelectedItems.Count} Bild(er) wirklich löschen?",
            "Löschen bestätigen", MessageBoxButton.YesNo, MessageBoxImage.Question);

        if (result == MessageBoxResult.Yes)
        {
            try
            {
                var selectedImages = lstImages.SelectedItems.Cast<ImageItem>().ToList();
                foreach (var image in selectedImages)
                {
                    if (File.Exists(image.FilePath))
                    {
                        File.Delete(image.FilePath);
                    }
                }

                LoadImagesForCurrentAlbum();
                MessageBox.Show($"{selectedImages.Count} Bild(er) wurden gelöscht.", "Erfolg", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Löschen: {ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    private void RotateClockwise_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        RotateSelectedImages(RotateFlipType.Rotate90FlipNone);
    }

    private void RotateCounterClockwise_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        RotateSelectedImages(RotateFlipType.Rotate270FlipNone);
    }

    private void Rotate180_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        RotateSelectedImages(RotateFlipType.Rotate180FlipNone);
    }

    private void RotateSelectedImages(RotateFlipType rotateFlipType)
    {
        if (lstImages.SelectedItems.Count == 0)
        {
            MessageBox.Show("Bitte wählen Sie mindestens ein Bild aus.", "Keine Auswahl", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        try
        {
            var selectedImages = lstImages.SelectedItems.Cast<ImageItem>().ToList();
            foreach (var imageItem in selectedImages)
            {
                using var image = System.Drawing.Image.FromFile(imageItem.FilePath);
                image.RotateFlip(rotateFlipType);
                image.Save(imageItem.FilePath);
            }

            LoadImagesForCurrentAlbum();
            MessageBox.Show($"{selectedImages.Count} Bild(er) wurden rotiert.", "Erfolg", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Fehler beim Rotieren: {ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void LoadImagesForCurrentAlbum()
    {
        CurrentImages.Clear();

        if (string.IsNullOrEmpty(CurrentAlbumName))
            return;

        var currentAlbum = _albumsData.FirstOrDefault(a => a.Name == CurrentAlbumName);
        if (currentAlbum != null)
        {
            currentAlbum.RefreshImagePaths();
            foreach (var imagePath in currentAlbum.ImagePaths)
            {
                CurrentImages.Add(new ImageItem(imagePath));
            }
        }
    }

    private void CmbAlbums_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        LoadImagesForCurrentAlbum();
    }

    private void Window_Closing(object sender, CancelEventArgs e)
    {
        SaveAlbumsToXml();
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}