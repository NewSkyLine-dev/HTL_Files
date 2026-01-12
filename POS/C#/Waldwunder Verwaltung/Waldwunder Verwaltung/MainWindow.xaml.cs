using DataModel;
using System.Collections.ObjectModel;
using System.Windows;

namespace Waldwunder_Verwaltung;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public ObservableCollection<Waldwunder> WaldwunderList { get; set; } = [];

    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainViewModel();
    }

    private async void LoadData()
    {
        List<Waldwunder> wonders = await WaldwunderDataService.GetAllWaldwundersAsync();
        WaldwunderList.Clear();
        foreach (Waldwunder wonder in wonders)
        {
            WaldwunderList.Add(wonder);
        }
    }

    private async void NewWaldwunder_Click(object sender, RoutedEventArgs e)
    {
        AddWaldwunderDialog dialog = new();
        dialog.ShowDialog();
        if (dialog.DialogResult == true)
        {
            Waldwunder newWaldwunder = dialog.NewWaldwunder;
            var bilder = dialog.GetBilderToSave();
            try
            {
                // Waldwunder speichern
                await WaldwunderDataService.AddNewWaldwunderWithBilderAsync(newWaldwunder, bilder);
                LoadData();
            }
            catch (Exception error)
            {
                MessageBox.Show($"Error: {error.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    private void SearchItem_Click(object sender, RoutedEventArgs e)
    {

    }

    private void ExitMenu_Click(object sender, RoutedEventArgs e)
    {

    }

    private Waldwunder? _selectedWaldwunder;
    public Waldwunder? SelectedWaldwunder
    {
        get => _selectedWaldwunder;
        set
        {
            _selectedWaldwunder = value;
            // Optional: PropertyChanged-Event, falls MVVM genutzt wird
        }
    }

    private async void ShowSelectedWaldwunderDetails()
    {
        if (SelectedWaldwunder == null)
            return;
        var bilder = await WaldwunderDataService.GetBildersForWaldwunderAsync(SelectedWaldwunder.Id);
        var dialog = new WaldwunderDetailsDialog(SelectedWaldwunder, bilder);
        dialog.ShowDialog();
    }

    private void ShowDetailsButton_Click(object sender, RoutedEventArgs e)
    {
        ShowSelectedWaldwunderDetails();
    }

    protected override void OnContentRendered(EventArgs e)
    {
        base.OnContentRendered(e);
        // Marker-Click-Event verbinden
        this.mapControl.MarkerClicked += MapControl_MarkerClicked;
        // ListBox-Auswahl synchronisiert Marker
        if (DataContext is MainViewModel vm)
        {
            vm.PropertyChanged += (s, args) =>
            {
                if (args.PropertyName == nameof(vm.SelectedWonder) && vm.SelectedWonder != null)
                {
                    this.mapControl.SelectMarkerByWonderId((int)vm.SelectedWonder.Id);
                }
            };
        }
    }

    private void MapControl_MarkerClicked(object? sender, int wonderId)
    {
        if (DataContext is MainViewModel vm)
        {
            vm.SelectWonderByIdOnMap(wonderId);
        }
    }
}