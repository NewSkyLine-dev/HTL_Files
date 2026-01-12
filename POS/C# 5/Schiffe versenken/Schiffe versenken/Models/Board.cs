using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Schiffe_versenken.Models;

public class Board : INotifyPropertyChanged
{
    public const int BoardSize = 10;

    public ObservableCollection<Ship> Ships { get; private set; } = new();
    public Field[,] Fields { get; private set; }

    public Board()
    {
        Fields = new Field[BoardSize, BoardSize];
        InitializeFields();
    }

    private void InitializeFields()
    {
        for (int x = 0; x < BoardSize; x++)
        {
            for (int y = 0; y < BoardSize; y++)
            {
                Fields[x, y] = new Field();
            }
        }
    }

    public void Reset()
    {
        Ships.Clear();
        InitializeFields();
        OnPropertyChanged(nameof(Fields));
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
