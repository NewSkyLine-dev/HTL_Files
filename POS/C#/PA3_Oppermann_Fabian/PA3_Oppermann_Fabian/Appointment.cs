using System.ComponentModel;

namespace PA3_Oppermann_Fabian;

public class Appointment : INotifyPropertyChanged
{
    private string _Titel = "Neuer Termin";
    private string _Beschreibung = string.Empty;
    private DateTime _Datum = DateTime.Now;

    public string Titel
    {
        get => _Titel;
        set { _Titel = value; OnPropertyChanged(nameof(Titel)); }
    }

    public string Beschreibung
    {
        get => _Beschreibung;
        set { _Beschreibung = value; OnPropertyChanged(nameof(Beschreibung)); }
    }

    public DateTime Datum
    {
        get => _Datum;
        set { _Datum = value; OnPropertyChanged(nameof(Datum)); }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
