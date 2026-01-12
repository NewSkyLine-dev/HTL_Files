using Fluent;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;

namespace WPF_WordPad;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : RibbonWindow
{
    private string currentFilePath = string.Empty;
    private bool documentChanged = false;

    public MainWindow()
    {
        InitializeComponent();
        UserTextBox.AddHandler(System.Windows.Controls.RichTextBox.DragOverEvent, new System.Windows.DragEventHandler(RichTextBox_DragOver), true);
        UserTextBox.AddHandler(System.Windows.Controls.RichTextBox.DropEvent, new System.Windows.DragEventHandler(RichTextBox_Drop), true);
        UserTextBox.TextChanged += RichTextBox_TextChanged;
        UpdateTitle();
    }

    private void RichTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (!documentChanged)
        {
            documentChanged = true;
            UpdateTitle();
        }
    }

    private void UpdateTitle()
    {
        string fileName = string.IsNullOrEmpty(currentFilePath) ? "Unbenannt" : System.IO.Path.GetFileName(currentFilePath);
        this.Title = $"{fileName}{(documentChanged ? "*" : "")} - WPF TextEditor";
    }

    private void RichTextBox_DragOver(object sender, System.Windows.DragEventArgs e)
    {
        if (e.Data.GetDataPresent(System.Windows.DataFormats.FileDrop))
        {
            e.Effects = System.Windows.DragDropEffects.Copy;
        }
        else
        {
            e.Effects = System.Windows.DragDropEffects.None;
        }
        e.Handled = true;
    }

    private void RichTextBox_Drop(object sender, System.Windows.DragEventArgs e)
    {
        if (e.Data.GetDataPresent(System.Windows.DataFormats.FileDrop))
        {
            string[] files = (string[])e.Data.GetData(System.Windows.DataFormats.FileDrop);
            if (files.Length > 0)
            {
                try
                {
                    OpenFile(files[0]);
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show($"Fehler beim Öffnen der Datei: {ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }

    private void NewCommand_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        if (documentChanged)
        {
            MessageBoxResult result = System.Windows.MessageBox.Show(
                "Möchten Sie die Änderungen speichern?",
                "Ungespeicherte Änderungen",
                MessageBoxButton.YesNoCancel,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                SaveCommand_Executed(sender, e);
                if (documentChanged)
                    return;
            }
            else if (result == MessageBoxResult.Cancel)
            {
                return;
            }
        }

        UserTextBox.Document.Blocks.Clear();
        currentFilePath = string.Empty;
        documentChanged = false;
        UpdateTitle();
    }

    private void OpenCommand_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        if (documentChanged)
        {
            MessageBoxResult result = System.Windows.MessageBox.Show(
                "Möchten Sie die Änderungen speichern?",
                "Ungespeicherte Änderungen",
                MessageBoxButton.YesNoCancel,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                SaveCommand_Executed(sender, e);
                if (documentChanged)
                    return;
            }
            else if (result == MessageBoxResult.Cancel)
            {
                return;
            }
        }

        OpenFileDialog openFileDialog = new()
        {
            Filter = "XAML Dokumente (*.xaml)|*.xaml|Alle Dateien (*.*)|*.*",
            Title = "Dokument öffnen"
        };

        if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
        {
            OpenFile(openFileDialog.FileName);
        }
    }

    private void OpenFile(string filePath)
    {
        try
        {
            using FileStream fileStream = new(filePath, FileMode.Open, FileAccess.Read);
            if (XamlReader.Load(fileStream) is FlowDocument document)
            {
                UserTextBox.Document = document;
                currentFilePath = filePath;
                documentChanged = false;
                UpdateTitle();
            }
            else
            {
                System.Windows.MessageBox.Show("Die Datei konnte nicht als FlowDocument geladen werden.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        catch (Exception ex)
        {
            System.Windows.MessageBox.Show($"Fehler beim Öffnen der Datei: {ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void SaveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        if (string.IsNullOrEmpty(currentFilePath))
        {
            SaveAsCommand_Executed(sender, e);
        }
        else
        {
            SaveFileToPath(currentFilePath);
        }
    }

    private void SaveAsCommand_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        SaveFileDialog saveFileDialog = new()
        {
            Filter = "XAML Dokumente (*.xaml)|*.xaml|Alle Dateien (*.*)|*.*",
            Title = "Dokument speichern als",
            DefaultExt = "xaml"
        };

        if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
        {
            SaveFileToPath(saveFileDialog.FileName);
        }
    }

    private void SaveFileToPath(string filePath)
    {
        try
        {
            using (FileStream fileStream = new(filePath, FileMode.Create, FileAccess.Write))
            {
                XamlWriter.Save(UserTextBox.Document, fileStream);
            }
            currentFilePath = filePath;
            documentChanged = false;
            UpdateTitle();
        }
        catch (Exception ex)
        {
            System.Windows.MessageBox.Show($"Fehler beim Speichern der Datei: {ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void PrintCommand_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        System.Windows.Controls.PrintDialog printDialog = new();
        if (printDialog.ShowDialog() == true)
        {
            printDialog.PrintDocument((UserTextBox.Document as IDocumentPaginatorSource).DocumentPaginator, "Dokument drucken");
        }
    }

    private void ExitCommand_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        Close();
    }

    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
        if (documentChanged)
        {
            MessageBoxResult result = System.Windows.MessageBox.Show(
                "Möchten Sie die Änderungen speichern?",
                "Ungespeicherte Änderungen",
                MessageBoxButton.YesNoCancel,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                SaveCommand_Executed(sender, null);
                if (documentChanged)
                    e.Cancel = true;
            }
            else if (result == MessageBoxResult.Cancel)
            {
                e.Cancel = true;
            }
        }
    }

    private void FontColorPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<System.Windows.Media.Color?> e)
    {
        if (UserTextBox != null && e.NewValue.HasValue)
        { 
            if (e.NewValue.HasValue)
            {
                UserTextBox.Selection.ApplyPropertyValue(TextElement.ForegroundProperty, new SolidColorBrush(e.NewValue.Value));
            }
        }
    }

    private void HighlightColorPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<System.Windows.Media.Color?> e)
    {
        if (UserTextBox != null && e.NewValue.HasValue)
        {
            if (e.NewValue.HasValue)
            {
                UserTextBox.Selection.ApplyPropertyValue(TextElement.BackgroundProperty, new SolidColorBrush(e.NewValue.Value));
            }
        }
    }

    private void FontSizeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (UserTextBox != null && fontSizeComboBox.SelectedItem != null)
        {
            if (fontSizeComboBox.SelectedItem is ComboBoxItem selectedItem &&
                double.TryParse(selectedItem.Content.ToString(), out double fontSize))
            {
                UserTextBox.Selection.ApplyPropertyValue(TextElement.FontSizeProperty, fontSize);
            }
        }
    }

    private void FontFamilyComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (UserTextBox != null)
        {
            if (fontFamilyComboBox.SelectedItem != null)
            {
                if (fontFamilyComboBox.SelectedItem is ComboBoxItem selectedItem)
                {
                    var fontFamily = new System.Windows.Media.FontFamily(selectedItem.Content.ToString());
                    UserTextBox.Selection.ApplyPropertyValue(TextElement.FontFamilyProperty, fontFamily);
                }
            }
        }
    }
}

public static class CustomCommands
{
    public static readonly RoutedUICommand SaveAs = new(
        "Speichern unter...",
        "SaveAs",
        typeof(CustomCommands),
        [new KeyGesture(Key.S, ModifierKeys.Control | ModifierKeys.Shift)]
    );

    public static readonly RoutedUICommand Exit = new(
        "Beenden",
        "Exit",
        typeof(CustomCommands),
        [new KeyGesture(Key.F4, ModifierKeys.Alt)]
    );
}