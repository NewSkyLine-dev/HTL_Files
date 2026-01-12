using System.Collections.ObjectModel;
using System.Windows.Controls;
using Schiffe_versenken.ViewModels;

namespace Schiffe_versenken.Views;

public partial class BoardView : UserControl
{
    public BoardView()
    {
        InitializeComponent();
    }
}

public class BoardViewViewModel(ObservableCollection<CellViewModel> cells, string title) : BaseViewModel
{
    public ObservableCollection<CellViewModel> Cells { get; } = cells;
    public ObservableCollection<string> ColumnHeaders { get; } = ["A", "B", "C", "D", "E", "F", "G", "H", "I", "J"];
    public ObservableCollection<string> RowHeaders { get; } = ["1", "2", "3", "4", "5", "6", "7", "8", "9", "10"];
    public string Title { get; } = title;
}
