using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Schiffe_versenken.Models;

public class Ship : INotifyPropertyChanged
{
    private List<(int X, int Y)> _positions;
    private string _name;
    private int _size;
    private bool _isSunk;
    private bool _isPlaced;

    public Ship(string name, int size)
    {
        _name = name;
        _size = size;
        _positions = new List<(int X, int Y)>();
        _isPlaced = false;
        _isSunk = false;
    }

    public Ship(List<(int X, int Y)> positions) : this("Ship", positions.Count)
    {
        _positions = positions;
        _isPlaced = true;
    }

    public string Name
    {
        get => _name;
        set
        {
            if (_name != value)
            {
                _name = value;
                OnPropertyChanged();
            }
        }
    }

    public int Size
    {
        get => _size;
        set
        {
            if (_size != value)
            {
                _size = value;
                OnPropertyChanged();
            }
        }
    }

    public List<(int X, int Y)> Positions
    {
        get => _positions;
        set
        {
            _positions = value;
            OnPropertyChanged();
        }
    }

    public bool IsSunk
    {
        get => _isSunk;
        set
        {
            if (_isSunk != value)
            {
                _isSunk = value;
                OnPropertyChanged();
            }
        }
    }

    public bool IsPlaced
    {
        get => _isPlaced;
        set
        {
            if (_isPlaced != value)
            {
                _isPlaced = value;
                OnPropertyChanged();
            }
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
