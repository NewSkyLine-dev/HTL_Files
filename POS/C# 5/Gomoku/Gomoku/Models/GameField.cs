using CommunityToolkit.Mvvm.ComponentModel;

namespace Gomoku.Models;

public partial class GameField(int numberOfFields) : ObservableObject
{
    [ObservableProperty]
    private int _numberOfFields = numberOfFields;

    [ObservableProperty]
    private List<List<Field>> _field = new(numberOfFields);

    [ObservableProperty]
    private Player _player1 = new();

    [ObservableProperty]
    private Player _player2 = new();

    [ObservableProperty]
    private Player? _currentPlayer = null;

    private int OccupiedFields => _field.Sum(row => row.Count(field => field is not null));
}