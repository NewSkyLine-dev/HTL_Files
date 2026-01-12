using DataModel;
using Microsoft.Win32;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

namespace Waldwunder_Verwaltung;

public partial class AddWaldwunderDialog : Window
{
    public Waldwunder NewWaldwunder { get; private set; }
    public List<string> SelectedImagePaths { get; set; } = new();
    private List<Bilder> _bilderToSave = new();

    public AddWaldwunderDialog()
    {
        NewWaldwunder = new Waldwunder();
        InitializeComponent();
        DataContext = this;
    }

    // public System.Windows.Controls.ListBox ImagesListBox => (System.Windows.Controls.ListBox)this.FindName("ImagesListBox");

    private void SelectImages_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new OpenFileDialog
        {
            Multiselect = true,
            Filter = "Bilder (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png"
        };
        if (dialog.ShowDialog() == true)
        {
            foreach (var file in dialog.FileNames)
            {
                if (!SelectedImagePaths.Contains(file))
                    SelectedImagePaths.Add(file);
            }
            UpdateImagesListBox();
        }
    }

    private void RemoveSelectedImage_Click(object sender, RoutedEventArgs e)
    {
        if (ImagesListBox.SelectedItem is string selected)
        {
            SelectedImagePaths.Remove(selected);
            UpdateImagesListBox();
        }
    }

    private void UpdateImagesListBox()
    {
        ImagesListBox.ItemsSource = null;
        ImagesListBox.ItemsSource = SelectedImagePaths.Select(Path.GetFileName).ToList();
    }

    private void OkButton_Click(object sender, RoutedEventArgs e)
    {
        // Bilder kopieren und umbenennen
        var imageFolder = Path.Combine(Directory.GetCurrentDirectory(), "Images");
        if (!Directory.Exists(imageFolder))
            Directory.CreateDirectory(imageFolder);
        _bilderToSave.Clear();
        foreach (var srcPath in SelectedImagePaths)
        {
            var fileName = Path.GetFileName(srcPath);
            var destPath = Path.Combine(imageFolder, fileName);
            int count = 1;
            string nameOnly = Path.GetFileNameWithoutExtension(fileName);
            string ext = Path.GetExtension(fileName);
            while (File.Exists(destPath))
            {
                fileName = $"{nameOnly}_{count}{ext}";
                destPath = Path.Combine(imageFolder, fileName);
                count++;
            }
            File.Copy(srcPath, destPath);
            // Bilddatensatz für spätere Speicherung vorbereiten
            _bilderToSave.Add(new Bilder { Name = fileName });
        }
        DialogResult = true;
    }

    public List<Bilder> GetBilderToSave() => _bilderToSave;

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }
}
