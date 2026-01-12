using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Image_Rotator
{
    public partial class MainWindow : Window
    {
        private ObservableCollection<string> _imagePaths = [];
        public ObservableCollection<string> ImagePaths
        {
            get { return _imagePaths; }
            set { _imagePaths = value; }
        }
        private int _quality;
        private int _rotation;
        private string _mirror;

        public MainWindow()
        {
            InitializeComponent();

            DataContext = this;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog dialog = new();
            dialog.ShowDialog();
            string folder = dialog.SelectedPath;

            Directory.GetFiles(folder)
                    .ToList()
                    .ForEach(x => _imagePaths.Add(x));
            ProgressBar.Maximum = _imagePaths.Count;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            _quality = (int)QualitySlider.Value;
            _mirror = (MirrorCB.SelectedItem as ComboBoxItem)?.Content.ToString();
            _rotation = int.Parse((RotationCB.SelectedItem as ComboBoxItem)?.Content.ToString());
            ThreadPool.QueueUserWorkItem(Worker!);
        }

        private void Worker(object _)
        {
            foreach (string path in _imagePaths)
            {
                JpegBitmapEncoder encoder = new()
                {
                    QualityLevel = _quality,
                    Rotation = _rotation switch
                    {
                        0 => Rotation.Rotate0,
                        90 => Rotation.Rotate90,
                        180 => Rotation.Rotate180,
                        270 => Rotation.Rotate270,
                        _ => Rotation.Rotate0
                    },
                    FlipVertical = _mirror == "Vertikal",
                    FlipHorizontal = _mirror == "Horizontal"
                };
                encoder.Frames.Add(BitmapFrame.Create(new Uri(path)));

                string directory = Path.GetDirectoryName(path)!;
                string filename = Path.GetFileNameWithoutExtension(path);
                string extension = Path.GetExtension(path);
                string newPath = Path.Combine(directory, $"{filename}-edited{extension}");

                FileStream stream = new(newPath, FileMode.Create);
                encoder.Save(stream);
                stream.Close();

                Dispatcher.Invoke(() =>
                {
                    ProgressBar.Value += 1;
                });
            }
        }
    }
}