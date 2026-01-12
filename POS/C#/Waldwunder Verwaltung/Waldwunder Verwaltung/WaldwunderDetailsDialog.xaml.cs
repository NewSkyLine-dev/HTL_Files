using DataModel;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Waldwunder_Verwaltung;

public partial class WaldwunderDetailsDialog : Window
{
    public WaldwunderDetailsDialog(Waldwunder waldwunder, List<Bilder> bilder)
    {
        InitializeComponent();
        Title = waldwunder.Name;
        NameText.Text = waldwunder.Name;
        DescriptionText.Text = waldwunder.Description;
        ProvinceText.Text = waldwunder.Province;
        TypeText.Text = waldwunder.Type;
        LatLonText.Text = $"{waldwunder.Latitude}, {waldwunder.Longitude}";
        ImagesList.ItemsSource = bilder.Select(b => b.Name).ToList();
        ImagesList.SelectionChanged += (s, e) => ShowImage(bilder, ImagesList.SelectedItem as string);
        if (bilder.Count > 0)
            ImagesList.SelectedIndex = 0;
    }

    private void ShowImage(List<Bilder> bilder, string? fileName)
    {
        if (string.IsNullOrEmpty(fileName))
        {
            ImagePreview.Source = null;
            return;
        }
        var path = Path.Combine(Directory.GetCurrentDirectory(), "Images", fileName);
        if (File.Exists(path))
        {
            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.UriSource = new System.Uri(path);
            bitmap.EndInit();
            bitmap.Freeze();
            ImagePreview.Source = bitmap;
        }
        else
        {
            ImagePreview.Source = null;
        }
    }

    private void Close_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }
}
