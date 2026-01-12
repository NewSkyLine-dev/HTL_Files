using System.Windows.Input;
using System.Windows.Media;
using Schiffe_versenken.Models;

namespace Schiffe_versenken.ViewModels;

public class CellViewModel : BaseViewModel
{
    private readonly Field _field;
    private readonly int _x;
    private readonly int _y;
    private readonly bool _isEnemyBoard;
    private readonly GameViewModel _gameViewModel;

    public CellViewModel(Field field, int x, int y, bool isEnemyBoard, GameViewModel gameViewModel)
    {
        _field = field;
        _x = x;
        _y = y;
        _isEnemyBoard = isEnemyBoard;
        _gameViewModel = gameViewModel;

        _field.PropertyChanged += (s, e) => 
        {
            OnPropertyChanged(nameof(Background));
            OnPropertyChanged(nameof(DisplayText));
        };

        ClickCommand = new RelayCommand(OnClick, CanClick);
    }

    public int X => _x;
    public int Y => _y;

    public ICommand ClickCommand { get; }

    public Brush Background
    {
        get
        {
            if (_field.IsHighlighted)
                return Brushes.LightGreen;

            return _field.State switch
            {
                Field.FieldState.Empty => Brushes.LightBlue,
                Field.FieldState.Ship => _isEnemyBoard ? Brushes.LightBlue : Brushes.Gray,
                Field.FieldState.Hit => Brushes.Red,
                Field.FieldState.Miss => Brushes.LightYellow,
                _ => Brushes.LightBlue
            };
        }
    }

    public string DisplayText
    {
        get
        {
            return _field.State switch
            {
                Field.FieldState.Hit => "X",
                Field.FieldState.Miss => "?",
                _ => ""
            };
        }
    }

    private bool CanClick(object? parameter)
    {
        if (_isEnemyBoard)
        {
            return _gameViewModel.IsMyTurn && 
                   _gameViewModel.GamePhase == GamePhase.Playing &&
                   _field.State != Field.FieldState.Hit && 
                   _field.State != Field.FieldState.Miss;
        }
        else
        {
            return _gameViewModel.GamePhase == GamePhase.PlacingShips;
        }
    }

    private void OnClick(object? parameter)
    {
        if (_isEnemyBoard)
        {
            _gameViewModel.AttackCell(_x, _y);
        }
        else
        {
            _gameViewModel.PlaceShipAtCell(_x, _y);
        }
    }

    public void UpdateBackground()
    {
        OnPropertyChanged(nameof(Background));
    }
}
