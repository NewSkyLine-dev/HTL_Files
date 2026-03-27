using CommunityToolkit.Mvvm.ComponentModel;

namespace Gomoku.Models;

public partial class Field : ObservableObject
{
    [ObservableProperty]
    private Player? _occupied = null;
}
