using System;
using System.Data;
using System.Windows;

namespace A1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Parser parser = new Parser(textBox.Text);
                AbstractExpression expression = parser.Parse();

                List<char> variables = TruthTableGenerator.GetVariables(expression);
                DataTable truthTable = TruthTableGenerator.Build(expression, variables);
            }
            catch (Exception ex)
            {
                truthTableGrid.ItemsSource = null;
                treeBox.Text = string.Empty;
                statusText.Text = ex.Message;
            }
        }
    }
}
