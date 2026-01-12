using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace TerminControlLibrary;

public class TerminControl : Control
{

    public static readonly DependencyProperty DatumProperty =
        DependencyProperty.Register(nameof(Datum), typeof(DateTime), typeof(TerminControl),
        new PropertyMetadata(DateTime.Now));
    public DateTime Datum
    {
        get => (DateTime)GetValue(DatumProperty);
        set => SetValue(DatumProperty, value);
    }

    public static readonly DependencyProperty TitelProperty =
        DependencyProperty.Register(nameof(Titel), typeof(string), typeof(TerminControl),
        new PropertyMetadata("Neuer Termin"));
    public string Titel
    {
        get => (string)GetValue(TitelProperty);
        set => SetValue(TitelProperty, value);
    }

    public static readonly DependencyProperty BeschreibungProperty =
        DependencyProperty.Register(nameof(Beschreibung), typeof(string), typeof(TerminControl),
        new PropertyMetadata(""));
    public string Beschreibung
    {
        get => (string)GetValue(BeschreibungProperty);
        set => SetValue(BeschreibungProperty, value);
    }

    static TerminControl()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(TerminControl), new FrameworkPropertyMetadata(typeof(TerminControl)));
    }

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        if (GetTemplateChild("PART_ContentGrid") is Grid contentGrid)
        {
            contentGrid.MouseLeftButtonDown += ContentGrid_MouseLeftButton;
        }
    }

    private void ContentGrid_MouseLeftButton(object sender, MouseButtonEventArgs e)
    {
        ChangeDialogNew dialog = new()
        {
            DataContext = this
        };
        dialog.ShowDialog();
    }
}