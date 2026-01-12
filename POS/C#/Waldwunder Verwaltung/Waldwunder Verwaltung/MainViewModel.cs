using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace Waldwunder_Verwaltung;

public class MainViewModel : INotifyPropertyChanged
{
    private readonly WaldwunderDataService _wonderService = new();
    private readonly ImageService _imageService = new();

    private ObservableCollection<DataModel.Waldwunder> _wonders = new();
    private DataModel.Waldwunder? _selectedWonder;
    private string _searchKeyword = string.Empty;
    private string _searchType = string.Empty;
    private float _searchLatitude;
    private float _searchLongitude;

    public ObservableCollection<DataModel.Waldwunder> Wonders
    {
        get => _wonders;
        set { _wonders = value; OnPropertyChanged(); }
    }

    public DataModel.Waldwunder? SelectedWonder
    {
        get => _selectedWonder;
        set { _selectedWonder = value; OnPropertyChanged(); CommandManager.InvalidateRequerySuggested(); }
    }

    public string SearchKeyword
    {
        get => _searchKeyword;
        set { _searchKeyword = value; OnPropertyChanged(); }
    }

    public string SearchType
    {
        get => _searchType;
        set { _searchType = value; OnPropertyChanged(); }
    }

    public float SearchLatitude
    {
        get => _searchLatitude;
        set { _searchLatitude = value; OnPropertyChanged(); }
    }

    public float SearchLongitude
    {
        get => _searchLongitude;
        set { _searchLongitude = value; OnPropertyChanged(); }
    }

    public ObservableCollection<string> WonderTypes { get; } = new();

    public ICommand NewWonderCommand { get; }
    public ICommand SearchByKeywordCommand { get; }
    public ICommand SearchByTypeCommand { get; }
    public ICommand SearchByLocationCommand { get; }
    public ICommand ShowWonderDetailsCommand { get; }

    public MainViewModel()
    {
        NewWonderCommand = new RelayCommand(_ => ShowNewWonderDialog());
        SearchByKeywordCommand = new RelayCommand(async _ => await SearchByKeywordAsync());
        SearchByTypeCommand = new RelayCommand(async _ => await SearchByTypeAsync());
        SearchByLocationCommand = new RelayCommand(async _ => await SearchByLocationAsync());
        ShowWonderDetailsCommand = new RelayCommand(_ => ShowWonderDetails(), _ => SelectedWonder != null);
        _ = LoadTypesAsync();
        _ = LoadAllWondersAsync();
        _selectedWonder = null;
    }

    public void SelectWonderByIdOnMap(int id)
    {
        SelectedWonder = Wonders.FirstOrDefault(w => w.Id == id);
    }

    private async Task LoadTypesAsync()
    {
        var types = await WaldwunderDataService.GetAllTypes();
        WonderTypes.Clear();
        foreach (var type in types)
            if (!string.IsNullOrWhiteSpace(type)) WonderTypes.Add(type);
    }

    private async Task LoadAllWondersAsync()
    {
        var wonders = await WaldwunderDataService.GetAllWaldwundersAsync();
        Wonders.Clear();
        foreach (var w in wonders) Wonders.Add(w);
    }

    private void ShowNewWonderDialog()
    {
        var dialog = new AddWaldwunderDialog();
        if (dialog.ShowDialog() == true)
        {
            var newWaldwunder = dialog.NewWaldwunder;
            var bilder = dialog.GetBilderToSave();
            // Save to DB
            WaldwunderDataService.AddNewWaldwunderWithBilderAsync(newWaldwunder, bilder).Wait();
            // Refresh Wonders
            _ = LoadAllWondersAsync();
        }
    }

    private async Task SearchByKeywordAsync()
    {
        if (string.IsNullOrWhiteSpace(SearchKeyword)) return;
        var results = await WaldwunderDataService.SearchByKeyword(SearchKeyword);
        UpdateWonders(results);
    }

    private async Task SearchByTypeAsync()
    {
        if (string.IsNullOrWhiteSpace(SearchType)) return;
        var results = await WaldwunderDataService.SearchByType(SearchType);
        UpdateWonders(results);
    }

    private async Task SearchByLocationAsync()
    {
        var results = await WaldwunderDataService.SearchByLocation(SearchLatitude, SearchLongitude);
        UpdateWonders(results);
    }

    private void UpdateWonders(List<DataModel.Waldwunder> results)
    {
        Wonders.Clear();
        foreach (var wonder in results) Wonders.Add(wonder);
    }

    private void ShowWonderDetails()
    {
        if (SelectedWonder == null) return;
        var bilder = WaldwunderDataService.GetBildersForWaldwunderAsync(SelectedWonder.Id).Result;
        var dialog = new WaldwunderDetailsDialog(SelectedWonder, bilder);
        dialog.ShowDialog();
    }

    #region INotifyPropertyChanged
    public event PropertyChangedEventHandler? PropertyChanged;
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    #endregion
}

public class RelayCommand : ICommand
{
    private readonly Action<object?> _execute;
    private readonly Predicate<object?>? _canExecute;
    public RelayCommand(Action<object?> execute, Predicate<object?>? canExecute = null)
    {
        _execute = execute;
        _canExecute = canExecute;
    }
    public bool CanExecute(object? parameter) => _canExecute == null || _canExecute(parameter);
    public void Execute(object? parameter) => _execute(parameter);
    public event EventHandler? CanExecuteChanged
    {
        add { CommandManager.RequerySuggested += value; }
        remove { CommandManager.RequerySuggested -= value; }
    }
}
