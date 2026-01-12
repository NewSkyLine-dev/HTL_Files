using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Xml.Serialization;
using LINQtoCSV;
using Microsoft.Win32;

namespace WPF_Einkaufslistengenerator;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private List<Product> allProducts;
    public ObservableCollection<Product> Products { get; set; } = [];
    public ObservableCollection<ProductGroup> GroupedProducts { get; set; } = [];
    public ObservableCollection<string> Groups { get; set; } = [];

    private int amount = 1;
    public int Amount
    {
        get { return amount; }
        set { amount = value; }
    }

    public MainWindow()
    {
        InitializeComponent();
        DataContext = this;
        
        LoadProducts();
    }

    private void LoadProducts()
    {
        try
        {
            var csvContext = new CsvContext();
            var csvFileDescription = new CsvFileDescription()
            {
                SeparatorChar = ';',
                FirstLineHasColumnNames = false,
                EnforceCsvColumnAttribute = true
            };

            var fileDialog = new OpenFileDialog
            {
                FileName = "Document",
                DefaultExt = ".csv",
                Filter = "CSV Files (*.csv)|*.csv|All files (*.*)|*.*"
            };

            if ((bool)fileDialog.ShowDialog())
            {
                var filepath = fileDialog.FileName;

                allProducts = [.. csvContext.Read<Product>(filepath, csvFileDescription)];

                var groups = allProducts.Select(p => p.Group).Distinct().OrderBy(g => g).ToList();
                Groups = [.. groups];
                cmbProduktgruppe.ItemsSource = Groups;
                cmbProduktgruppe.SelectedIndex = 1;

                if (groups.Count > 0)
                {
                    cmbProduktgruppe.SelectedIndex = 0;
                }
            }

            RefreshProducts();
        }
        catch (Exception)
        {

        }
    }

    private void RefreshTreeView()
    {
        GroupedProducts = [];

        // Gruppiere alle Einträge nach Produktgruppe
        var gruppenListe = Products
            .GroupBy(item => item.Group)
            .OrderBy(g => g.Key);

        foreach (var gruppe in gruppenListe)
        {
            var produktGruppe = new ProductGroup() { Name = gruppe.Key };
            foreach (var item in gruppe.OrderBy(i => i.Name))
            {
                produktGruppe.Products.Add(item);
            }
            GroupedProducts.Add(produktGruppe);
        }

        trvEinkaufsliste.ItemsSource = GroupedProducts;
    }

    private void OpenBinding_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        OpenFileDialog openFileDialog = new()
        {
            Filter = "XML-Dateien (*.xml)|*.xml",
            Title = "Einkaufsliste öffnen"
        };

        if (openFileDialog.ShowDialog() == true)
        {
            try
            {
                XmlSerializer serializer = new(typeof(List<Product>));
                using StreamReader reader = new(openFileDialog.FileName);

                if (serializer.Deserialize(reader) is List<Product> geladeneListe)
                {
                    Products.Clear();
                    foreach (var item in geladeneListe)
                    {
                        Products.Add(item);
                    }

                    RefreshTreeView();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Laden: {ex.Message}", "Ladefehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    private void RefreshProducts()
    {
        if (cmbProduktgruppe.SelectedItem != null)
        {
            string selectedGroup = cmbProduktgruppe.SelectedItem.ToString();
            var products = allProducts
                .Where(p => p.Group == selectedGroup)
                .OrderBy(p => p.Name)
                .ToList();

            cmbProdukt.ItemsSource = products;

            if (products.Count > 0)
            {
                cmbProdukt.SelectedIndex = 0;
            }
        }
        RefreshTreeView();
    }

    private void SaveBinding_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        var saveFileDialog = new SaveFileDialog
        {
            Filter = "XML-Dateien (*.xml)|*.xml",
            Title = "Einkaufsliste speichern"
        };

        if (saveFileDialog.ShowDialog() == true)
        {
            try
            {
                XmlSerializer serializer = new(typeof(List<Product>));
                using (StreamWriter writer = new(saveFileDialog.FileName))
                {
                    serializer.Serialize(writer, Products.ToList());
                }

                MessageBox.Show("Einkaufsliste wurde erfolgreich gespeichert.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Speichern: {ex.Message}", "Speicherfehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    private void CmbProduktgruppe_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        RefreshProducts();
    }

    private void TxtSuche_TextChanged(object sender, TextChangedEventArgs e)
    {
        string searchText = txtSuche.Text.ToLower();

        if (string.IsNullOrWhiteSpace(searchText))
        {
            cmbSuchergebnisse.ItemsSource = null;
            return;
        }

        var resulsts = allProducts
            .Where(p => p.Name.Contains(searchText, StringComparison.CurrentCultureIgnoreCase) || p.Group.Contains(searchText, StringComparison.CurrentCultureIgnoreCase))
            .OrderBy(p => p.Name)
            .ToList();

        cmbSuchergebnisse.ItemsSource = resulsts;

        if (resulsts.Count > 0)
        {
            cmbSuchergebnisse.SelectedIndex = 0;
        }
    }

    private void AddButton_Click(object sender, RoutedEventArgs e)
    {
        string productName;
        string productGroup;

        if (cmbSuchergebnisse.SelectedItem is Product selectedProduct)
        {
            productName = selectedProduct.Name;
            productGroup = selectedProduct.Group;
        }
        else if (!string.IsNullOrWhiteSpace(txtEigenesProdukt.Text))
        {
            productName = txtEigenesProdukt.Text;
            productGroup = "Eigene Gruppe";
        }
        else if (cmbProdukt.SelectedItem is Product ausgewaehltesProdrukt)
        {
            productName = ausgewaehltesProdrukt.Name;
            productGroup = ausgewaehltesProdrukt.Group;
        }
        else
        {
            MessageBox.Show("Bitte wählen Sie ein Produkt aus oder geben Sie ein eigenes Produkt ein.",
                            "Hinweis", MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        productName = productName.Trim();

        var existingProduct = Products.FirstOrDefault(p => p.Name == productName);

        if (existingProduct == null)
        {
            var newProduct = new Product
            {
                Name = productName,
                Group = productGroup,
                Amount = Amount
            };
            Products.Insert(0, newProduct);
        }
        else
        {
            existingProduct.Amount += Amount;

            int index = Products.IndexOf(existingProduct);
            Products.Move(index, 0);
        }

        txtEigenesProdukt.Clear();
        RefreshTreeView();
    }

    private void NewBinding_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        Products.Clear();
        RefreshTreeView();
    }

    private void PrintBinding_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        try
        {
            // Create a PrintDialog
            PrintDialog printDialog = new();

            if (printDialog.ShowDialog() == true)
            {
                FlowDocument document = new()
                {
                    PagePadding = new(50)
                };

                // Titel
                Paragraph titleParagraph = new(new Run("Einkaufsliste"))
                {
                    FontSize = 24,
                    FontWeight = FontWeights.Bold,
                    TextAlignment = TextAlignment.Center
                };
                document.Blocks.Add(titleParagraph);

                // Datum
                Paragraph dateParagraph = new(new Run($"Erstellt am {DateTime.Now.ToShortDateString()}"))
                {
                    TextAlignment = TextAlignment.Center
                };
                document.Blocks.Add(dateParagraph);

                document.Blocks.Add(new Paragraph(new Run("\n")));

                // Einkaufsliste nach Gruppen gruppieren
                var gruppenListe = Products
                    .GroupBy(item => item.Group)
                    .OrderBy(g => g.Key);

                foreach (var gruppe in gruppenListe)
                {
                    // Gruppenname
                    Paragraph groupParagraph = new(new Run(gruppe.Key))
                    {
                        FontWeight = FontWeights.Bold,
                        Margin = new Thickness(0, 10, 0, 5)
                    };
                    document.Blocks.Add(groupParagraph);

                    // Produkte
                    Table table = new()
                    {
                        CellSpacing = 10
                    };

                    // Definiere zwei Spalten
                    table.Columns.Add(new TableColumn() { Width = new GridLength(50) });
                    table.Columns.Add(new TableColumn() { Width = new GridLength(50, GridUnitType.Auto) });

                    // Füge für jedes Produkt eine Zeile hinzu
                    foreach (var item in gruppe.OrderBy(i => i.Name))
                    {
                        TableRow row = new();

                        // Anzahl
                        TableCell anzahlCell = new(new Paragraph(new Run($"{item.Amount}x")))
                        {
                            TextAlignment = TextAlignment.Right
                        };
                        row.Cells.Add(anzahlCell);

                        // Produktname
                        row.Cells.Add(new TableCell(new Paragraph(new Run(item.Name))));

                        table.RowGroups.Add(new TableRowGroup());
                        table.RowGroups[0].Rows.Add(row);
                    }

                    document.Blocks.Add(table);
                }

                // Drucken
                printDialog.PrintDocument(((IDocumentPaginatorSource)document).DocumentPaginator, "Einkaufsliste");
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Fehler beim Drucken: {ex.Message}", "Druckfehler", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void CloseBinding_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        Close();
    }

    private void DeleteBinding_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        if (lstEinkaufsliste.SelectedItem is Product selectedProduct)
        {
            Products.Remove(selectedProduct);
            RefreshTreeView();
        }
    }
}