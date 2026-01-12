using DataModel;
using LinqToDB.Configuration;
using LinqToDB.Data;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;

namespace PA4_Oppermann_Fabian
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<string> MediaTypes { get; set; } = [];
        public ObservableCollection<Medium> Medias { get; set; } = [];

        public MainWindow()
        {
            DataContext = this;

            InitializeComponent();

            DataConnection.DefaultSettings = new LinqToDBSettings("PA4", "SQLite", "Data Source=database.db;");

            _ = GetAllTypes();
            _ = GetAllMediums();
        }

        private async Task GetAllTypes()
        {
            List<MediaType> types = await DataService.GetAllTypes();

            foreach (MediaType type in types)
            {
                MediaTypes.Add(type.Name);
            }
        }

        private async Task GetAllMediums()
        {
            var mediums = await DataService.GetMediaAsync();
            Medias.Clear();

            foreach (var medium in mediums)
            {
                Medias.Add(new Medium()
                {
                    Id = medium.Id,
                    Title = medium.Title,
                    Year = medium.Year,
                    Publisher = medium.Publisher,
                    MediaType = medium.MediaType,
                    Image = medium.Image
                });
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string? mediaType = this.SelMediaType.SelectedItem as string;

            if (string.IsNullOrEmpty(mediaType))
            {
                MessageBox.Show("Please select a media type.");
                return;
            }

            if (string.IsNullOrEmpty(this.Title.Text) || string.IsNullOrEmpty(this.Publisher.Text))
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            if (string.IsNullOrEmpty(this.Publisher.Text))
            {
                MessageBox.Show("Publisher cannot be empty.");
                return;
            }

            if (!string.IsNullOrEmpty(mediaType))
            {
                if (!MediaTypes.Contains(mediaType))
                {
                    MessageBox.Show("Invalid media type selected.");
                    return;
                }
            }

            if (!int.TryParse(this.Year.Text, out _))
            {
                MessageBox.Show("Year must be a valid integer.");
                return;
            }

            using DatabaseDb db = new DatabaseDb();
            long? mediaTypeId = db.MediaTypes.FirstOrDefault(m => m.Name == mediaType)?.Id;

            Medium newMedium = new Medium()
            {
                Title = this.Title.Text,
                Year = int.TryParse(this.Year.Text, out int year) ? year : (int?)null,
                Publisher = this.Publisher.Text,
                MediaType = (int)mediaTypeId,
            };

            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif";
            if (dialog.ShowDialog() == true)
            {
                string destFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images");
                if (!System.IO.Directory.Exists("Images"))
                {
                    System.IO.Directory.CreateDirectory("Images");
                }
                string destFile = Path.Combine(destFolder, Path.GetFileName(dialog.FileName));
                File.Copy(dialog.FileName, destFile, false);
                newMedium.Image = destFile; // Store the path to the image
            }
            else
            {
                MessageBox.Show("No image selected.");
                return;
            }

            _ = DataService.AddNewMedium(newMedium);

            _ = GetAllMediums();
        }

        private void ComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (this.SearchMediaType.SelectedItem is string selectedType)
            {
                using DatabaseDb db = new DatabaseDb();
                long? mediaTypeId = db.MediaTypes.FirstOrDefault(m => m.Name == selectedType)?.Id;

                Task<List<Medium>> mediaType = DataService.GetAllMediumByType((int)mediaTypeId);
                mediaType.ContinueWith(task =>
                {
                    if (task.IsCompletedSuccessfully)
                    {
                        Dispatcher.Invoke(() =>
                        {
                            Medias.Clear();

                            foreach (Medium medium in task.Result)
                            {
                                Medias.Add(medium);
                            }
                        });
                    }
                    else
                    {
                        MessageBox.Show("Error loading media: " + task.Exception?.Message);
                    }
                });
            }
            else
            {
                MessageBox.Show("Please select a valid media type.");
            }
        }

        private void SearchTxt_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            string searchText = this.SearchTxt.Text;
            if (string.IsNullOrEmpty(searchText))
            {
                _ = GetAllMediums();
                return;
            }
            Task<List<Medium>> searchResults = DataService.SearchAfterName(searchText);
            searchResults.ContinueWith(task =>
            {
                if (task.IsCompletedSuccessfully)
                {
                    Dispatcher.Invoke(() =>
                    {
                        Medias.Clear();
                        foreach (Medium medium in task.Result)
                        {
                            Medias.Add(medium);
                        }
                    });
                }
                else
                {
                    MessageBox.Show("Error searching media: " + task.Exception?.Message);
                }
            });
        }
    }
}