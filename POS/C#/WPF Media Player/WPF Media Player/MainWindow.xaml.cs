using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace WPF_Media_Player;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public ObservableCollection<VideoItem> PlaylistItems { get; set; } = [];
    public ObservableCollection<VideoItem> HistoryItems { get; set; } = [];
    private int currentPlaylistIndex = -1;
    private readonly DispatcherTimer timerVideoProgress;
    private readonly bool isUserSeeking = false;
    private bool isMuted = false;

    public MainWindow()
    {
        InitializeComponent();

        DataContext = this;

        timerVideoProgress = new()
        {
            Interval = TimeSpan.FromSeconds(1)
        };
        timerVideoProgress.Tick += TimerVideoProgress_Tick;
    }

    #region Media Control Methods

    private void PlayVideo(VideoItem? video)
    {
        if (video == null) return;

        try
        {
            MediaElement.Source = new Uri(video.Path);
            MediaElement.Play();
            UpdateCurrentIndex(video);

            if (HistoryItems.Contains(video))
            {
                HistoryItems.Remove(video); 
            }
            HistoryItems.Insert(0, video);
        }
        catch (Exception ex)
        {
            System.Windows.MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void UpdateCurrentIndex(VideoItem video)
    {
        for (int i = 0; i < HistoryItems.Count; i++)
        {
            if (HistoryItems[i].Path == video.Path)
            {
                currentPlaylistIndex = i;
                break;
            }
        }
    }

    private void PlayNext()
    {
        if (PlaylistItems.Count == 0) return;

        currentPlaylistIndex++;
        if (currentPlaylistIndex >= PlaylistItems.Count)
        {
            currentPlaylistIndex = 0;
        }

        PlayVideo(PlaylistItems[currentPlaylistIndex]);
    }

    private void PlayPrevious()
    {
        if (PlaylistItems.Count == 0) return;
        currentPlaylistIndex--;
        if (currentPlaylistIndex < 0)
        {
            currentPlaylistIndex = PlaylistItems.Count - 1;
        }
        PlayVideo(PlaylistItems[currentPlaylistIndex]);
    }

    #endregion

    #region Event Handlers

    private void ProgressSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
        if (isUserSeeking)
        {
            MediaElement.Position = TimeSpan.FromSeconds(ProgressSlider.Value);
            CurrentTimeText.Text = MediaElement.Position.ToString(@"hh\:mm\:ss");
        }
    }

    private void PlaylistListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (PlaylistListBox.SelectedItem is VideoItem selectedItem)
        {
            PlayVideo(selectedItem);
        }
    }

    private void HistoryListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (HistoryListBox.SelectedItem is VideoItem selectedItem)
        {
            PlayVideo(selectedItem);
        }
    }

    private void MediaElement_MediaEnded(object sender, RoutedEventArgs e)
    {
        PlayNext();
    }

    private void MediaElement_MediaOpened(object sender, RoutedEventArgs e)
    {
        if (MediaElement.NaturalDuration.HasTimeSpan)
        {
            var duration = MediaElement.NaturalDuration.TimeSpan;
            ProgressSlider.Maximum = duration.TotalSeconds;
            TotalTimeText.Text = duration.ToString(@"hh\:mm\:ss");

            // Update duration in current playing item
            string formattedDuration = duration.ToString(@"hh\:mm\:ss");
            string sourcePath = MediaElement.Source?.ToString() ?? string.Empty;

            // Find and update the item in PlaylistItems
            for (int i = 0; i < PlaylistItems.Count; i++)
            {
                if (sourcePath.EndsWith(PlaylistItems[i].Path) ||
                    sourcePath == PlaylistItems[i].Path ||
                    PlaylistItems[i].Path == MediaElement.Source?.LocalPath)
                {
                    PlaylistItems[i].Duration = formattedDuration;
                    break;
                }
            }

            // Find and update the item in HistoryItems
            for (int i = 0; i < HistoryItems.Count; i++)
            {
                if (sourcePath.EndsWith(HistoryItems[i].Path) ||
                    sourcePath == HistoryItems[i].Path ||
                    HistoryItems[i].Path == MediaElement.Source?.LocalPath)
                {
                    HistoryItems[i].Duration = formattedDuration;
                    break;
                }
            }
        }
    }


    private void VolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
        if (MediaElement == null) return;

        MediaElement.Volume = VolumeSlider.Value;
        if (MediaElement.Volume == 0)
        {
            muteButtonText.Text = "🔇";
            isMuted = true;
        }
        else
        {
            muteButtonText.Text = "🔊";
            isMuted = false;
        }
    }

    private void TimerVideoProgress_Tick(object? sender, EventArgs e)
    {
        if (!isUserSeeking && MediaElement.NaturalDuration.HasTimeSpan)
        {
            ProgressSlider.Value = MediaElement.Position.TotalSeconds;
            CurrentTimeText.Text = MediaElement.Position.ToString(@"hh\:mm\:ss");
        }
    }

    #endregion

    #region Command Helpers

    private void PlayCommand_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        if (MediaElement.Source != null)
        {
            MediaElement.Play();
            timerVideoProgress.Start();
        }
        else if (PlaylistItems.Count > 0)
        {
            // Spiele erstes Video ab, wenn nichts abgespielt wird
            currentPlaylistIndex = 0;
            PlayVideo(PlaylistItems[currentPlaylistIndex]);
            timerVideoProgress.Start();
        }
    }

    private void PlayCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
        e.CanExecute = (MediaElement != null && MediaElement.Source != null) || PlaylistItems.Count > 0;
    }

    private void PauseCommand_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        MediaElement.Pause();
    }

    private void PauseCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
        e.CanExecute = MediaElement != null && MediaElement.Source != null;
    }

    private void StopCommand_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        MediaElement.Stop();
        timerVideoProgress.Stop();
        ProgressSlider.Value = 0;
        CurrentTimeText.Text = "00:00";
    }

    private void StopCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
        e.CanExecute = MediaElement != null && MediaElement.Source != null;
    }

    private void PreviousCommand_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        PlayPrevious();
    }

    private void PreviousCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
        e.CanExecute = PlaylistItems.Count > 1;
    }

    private void NextCommand_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        PlayNext();
    }

    private void NextCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
        e.CanExecute = PlaylistItems.Count > 1;
    }

    private void MuteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        if (isMuted)
        {
            // Lautstärke wiederherstellen
            MediaElement.Volume = VolumeSlider.Value > 0 ? VolumeSlider.Value : 0.5;
            muteButtonText.Text = "🔊";
        }
        else
        {
            // Stummschalten
            MediaElement.Volume = 0;
            muteButtonText.Text = "🔇";
        }
        isMuted = !isMuted;
    }

    private void AddVideoCommand_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        System.Windows.Forms.OpenFileDialog openFileDialog = new()
        {
            Filter = "Videodateien|*.mp4;*.avi;*.mkv;*.wmv;*.mov;*.mpg;*.mpeg|Alle Dateien|*.*",
            Multiselect = true
        };

        if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
        {
            foreach (string filename in openFileDialog.FileNames)
            {
                AddVideoToPlaylist(filename);
            }
        }
    }

    private void AddFolderCommand_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        using var dialog = new System.Windows.Forms.FolderBrowserDialog();
        System.Windows.Forms.DialogResult result = dialog.ShowDialog();
        if (result == System.Windows.Forms.DialogResult.OK)
        {
            string[] videoExtensions = [".mp4", ".avi", ".mkv", ".wmv", ".mov", ".mpg", ".mpeg"];
            string[] files = Directory.GetFiles(dialog.SelectedPath);

            foreach (string file in files)
            {
                string ext = System.IO.Path.GetExtension(file).ToLower();
                if (Array.Exists(videoExtensions, e => e == ext))
                {
                    AddVideoToPlaylist(file);
                }
            }
        }
    }

    private void TogglePlaylistCommand_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        PlaylistPanel.Visibility = PlaylistPanel.Visibility == Visibility.Visible ?
                                  Visibility.Collapsed : Visibility.Visible;
    }

    private void ToggleHistoryCommand_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        HistoryPanel.Visibility = HistoryPanel.Visibility == Visibility.Visible ?
                                 Visibility.Collapsed : Visibility.Visible;
    }

    #endregion

    #region Helper Methods

    private void AddVideoToPlaylist(string filePath)
    {
        try
        {
            // Erstellen eines neuen VideoItem
            VideoItem newItem = new()
            {
                Path = filePath,
                Name = System.IO.Path.GetFileName(filePath),
                Duration = "??:??" // Dauer wird erst beim Abspielen erkannt
            };

            PlaylistItems.Add(newItem);

            // Wenn es das erste Video in der Playlist ist, setze den Index
            if (PlaylistItems.Count == 1)
            {
                currentPlaylistIndex = 0;
            }
        }
        catch (Exception ex)
        {
            System.Windows.MessageBox.Show($"Fehler beim Hinzufügen des Videos: {ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    #endregion
}