using LINQtoCSV;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WPF_Einkaufslistengenerator;

public class Product : INotifyPropertyChanged
{

    private string _Name = string.Empty;
    [CsvColumn(Name = "Name", FieldIndex = 2)]
    public string Name
    {
        get => _Name;
        set
        {
            if (_Name != value)
            {
                _Name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
    }


    private string _Group = string.Empty;
    [CsvColumn(Name = "Group", FieldIndex = 1)]
    public string Group
    {
        get => _Group;
        set
        {
            if (_Group != value)
            {
                _Group = value;
                OnPropertyChanged(nameof(Group));
            }
        }
    }


    private int _Amount = 0;
    public int Amount
    {
        get => _Amount;
        set
        {
            if (_Amount != value)
            {
                _Amount = value;
                OnPropertyChanged(nameof(Amount));
            }
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public override string ToString()
    {
        return $"{Name}x {Amount}";
    }
}
