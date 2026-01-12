using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Serialization;
using AbcRobotCore;

namespace Robotersteuerung
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoadLevel_Click(object sender, RoutedEventArgs e)
        {
            // Load XML file
            var fileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*",
                Title = "Select a Level XML File"
            };
            if (fileDialog.ShowDialog() == true)
            {
                try
                {
                    // Serialize XML
                    XmlSerializer serializer = new(typeof(RobotField.XML_Field));
                    using var stream = File.OpenRead(fileDialog.FileName);
                    var seField = (RobotField.XML_Field)serializer.Deserialize(stream);

                    if (seField == null)
                    {
                        MessageBox.Show("The selected file is not a valid level file.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    // Add Row and Col definitions to the Grid Playground
                    Playground.RowDefinitions.Clear();
                    Playground.ColumnDefinitions.Clear();

                    for (int i = 0; i < seField.Width; i++)
                    {
                        Playground.ColumnDefinitions.Add(new ColumnDefinition());
                    }

                    for (int i = 0; i < seField.Height; i++)
                    {
                        Playground.RowDefinitions.Add(new RowDefinition());
                    }

                    Playground.Children.Clear();

                    foreach (var field in seField.Fields)
                    {
                        var image = new TextBlock()
                        {
                            Text = field.Type.ToString(),
                            HorizontalAlignment = HorizontalAlignment.Center,
                            VerticalAlignment = VerticalAlignment.Center
                        };
                        image.SetValue(Grid.RowProperty, field.X);
                        image.SetValue(Grid.ColumnProperty, field.Y);

                        Playground.Children.Add(image);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading file: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}