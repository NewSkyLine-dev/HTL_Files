using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace Bilderverwaltungsprogramm;

public partial class AlbumSelectionDialog : Window, INotifyPropertyChanged
{
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

    private string _selectedAlbum;
    public string SelectedAlbum
    {
        get => _selectedAlbum;
        set
        {
            _selectedAlbum = value;
            OnPropertyChanged();
        }
    }

    public AlbumSelectionDialog(List<string> albums, string currentAlbum)
    {
        InitializeComponent();
        DataContext = this;

        Albums = new ObservableCollection<string>(albums.Where(a => a != currentAlbum));
    }

    private void BtnOk_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrEmpty(SelectedAlbum))
        {
            MessageBox.Show("Bitte wählen Sie ein Album aus.", "Keine Auswahl", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        DialogResult = true;
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
