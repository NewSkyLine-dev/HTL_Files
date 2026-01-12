using System.Windows;
using LinqToDB.Configuration;
using LinqToDB.Data;

namespace Waldwunder_Verwaltung;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        DataConnection.DefaultSettings = new LinqToDBSettings("Waldwunder", "SQLite", "Data Source=Waldwunder.db;");
    }
}
