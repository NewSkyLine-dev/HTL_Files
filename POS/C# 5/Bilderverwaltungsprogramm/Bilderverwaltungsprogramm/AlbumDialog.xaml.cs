using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Bilderverwaltungsprogramm
{
    public partial class AlbumDialog : Window
    {
        public Album NewAlbum { get; set; }
        public ObservableCollection<string> DisplayUnterordner { get; set; }

        public AlbumDialog(string question, string defaultAnswer = "")
        {
            InitializeComponent();
            txtAnswer.Text = question;

            NewAlbum = new Album();
            DisplayUnterordner = new ObservableCollection<string>
            {
                "-- Unterordner --"
            };
            DataContext = this;
        }

        private void btnDialogOk_Click(object sender, RoutedEventArgs e)
        {
            string albumName = txtAlbumName.Text?.Trim();

            if (string.IsNullOrWhiteSpace(albumName))
            {
                MessageBox.Show("Bitte geben Sie einen Albumnamen ein.", "Ungültiger Name", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            NewAlbum.Name = albumName;
            DialogResult = true;
        }

        private void btnAddAlbum_Click(object sender, RoutedEventArgs e)
        {
            string newAlbumName = txtAlbumName.Text.Trim();
            if (!string.IsNullOrEmpty(newAlbumName) && !NewAlbum.Unterordner.Contains(newAlbumName))
            {
                NewAlbum.Unterordner.Add(newAlbumName);
                DisplayUnterordner.Add(newAlbumName);
                txtAlbumName.Clear();
            }
            else
            {
                MessageBox.Show("Der Albumname darf nicht leer sein oder bereits existieren.", "Ungültiger Name", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
