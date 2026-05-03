using AbcRobotCore;
using Microsoft.Win32;
using System.IO;
using System.Windows;

namespace Robotersteuerung
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<IExpression> _program;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoadXml_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog { Filter = "XML Dateien|*.xml" };
            if (dlg.ShowDialog() == true)
            {
                RobotField.LoadField(dlg.FileName);
                StatusBar.Foreground = System.Windows.Media.Brushes.Green;
                StatusBar.Text = "Feld geladen: " + Path.GetFileName(dlg.FileName);
            }
        }

        private void LoadProgram_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog { Filter = "Textdateien|*.txt" };
            if (dlg.ShowDialog() != true) return;

            string code = File.ReadAllText(dlg.FileName);
            CodeEditor.Text = code;

            try
            {
                var parser = new Parser(code);
                _program = parser.Parse();
                StatusBar.Foreground = System.Windows.Media.Brushes.Green;
                StatusBar.Text = "Programm erfolgreich geparst.";
            }
            catch (Exception ex)
            {
                _program = null;
                StatusBar.Foreground = System.Windows.Media.Brushes.Red;
                StatusBar.Text = $"{ex.Message}";
            }
        }

        private async void Run_Click(object sender, RoutedEventArgs e)
        {
            if (_program == null)
            {
                StatusBar.Text = "Kein gültiges Programm geladen.";
                return;
            }
            var ctx = new Context(RobotField);
            await Task.Run(() =>
            {
                foreach (var stmt in _program)
                    stmt.Interpret(ctx);
            });
            StatusBar.Foreground = System.Windows.Media.Brushes.Green;
            StatusBar.Text = "Ausführung abgeschlossen.";
        }
    }
}