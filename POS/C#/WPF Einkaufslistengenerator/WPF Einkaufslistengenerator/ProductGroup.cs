using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Einkaufslistengenerator;

public class ProductGroup : INotifyPropertyChanged
{

    private string _Name = string.Empty;
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

    public ObservableCollection<Product> Products { get; set; } = [];

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
