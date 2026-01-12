using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace PA3_Oppermann_Fabian;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public ObservableCollection<Appointment> Appointments { get; set; } = [];
    private readonly Stack<List<Appointment>> undoStack = [];
    private readonly Stack<List<Appointment>> redoStack = [];

    public MainWindow()
    {
        InitializeComponent();
        DataContext = this;

        ICollectionView appointmentView = CollectionViewSource.GetDefaultView(Appointments);

        appointmentView.SortDescriptions.Add(new SortDescription("Datum", ListSortDirection.Ascending));

        if (appointmentView is ListCollectionView listCollectionView)
        {
            listCollectionView.LiveSortingProperties.Add("Datum");
            listCollectionView.IsLiveSorting = true;
        }

        AppointmentListBox.ItemsSource = appointmentView;
        AppointmentListBox.Items.IsLiveSorting = true;
        AppointmentListBox.Items.SortDescriptions.Add(new("Datum", ListSortDirection.Ascending));
    }

    private void NewCommand_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        SaveStateForUndo();
        var newAppointment = new Appointment();
        newAppointment.PropertyChanged += Appointment_PropertyChanged;
        Appointments.Add(newAppointment);
    }

    private void Appointment_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        CollectionViewSource.GetDefaultView(Appointments).Refresh();
    }

    private void DeleteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        SaveStateForUndo();
        var selectedItems = new List<Appointment>(AppointmentListBox.SelectedItems.Cast<Appointment>());
        foreach (var item in selectedItems)
        {
            Appointments.Remove(item);
        }
    }

    private void CloseCommand_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        Close();
    }

    private void UndoCommand_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        if (undoStack.Count > 0)
        {
            redoStack.Push([.. Appointments]);
            Appointments.Clear();
            foreach (var item in undoStack.Pop())
            {
                Appointments.Add(item);
            }
        }
    }

    private void RedoCommand_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        if (redoStack.Count > 0)
        {
            undoStack.Push([.. Appointments]);
            Appointments.Clear();
            foreach (var item in redoStack.Pop())
            {
                Appointments.Add(item);
            }
        }
    }

    private void SaveStateForUndo()
    {
        undoStack.Push([.. Appointments]);
        redoStack.Clear();
    }
}