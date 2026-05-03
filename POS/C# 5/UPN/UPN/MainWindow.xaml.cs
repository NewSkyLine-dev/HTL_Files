using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace UPN
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            string[] examples = 
                [
                    "3 4 +",           // 7
                    "3 4 + 2 *",       // 14
                    "2 3 1 + ^",       // 2^4 = 16
                    "5 1 2 + 4 * + 3 -",  // 14
                    "15 7 1 1 + - / 3 * 2 1 1 + + -",  // 5
                ];

            foreach (var expr in examples)
            {
                Console.WriteLine($"UPN   : {expr}");
                try
                {
                    var tokens = Lexer.Tokenize(expr);
                    var ast = Parser.Build(tokens);
                    var result = Parser.Evaluate(ast);
                    var infix = ast.ToInfix();

                    Console.WriteLine($"Infix : {infix}");
                    Console.WriteLine($"Result: {result}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Fehler: {ex.Message}");
                }
                Console.WriteLine();
            }
        }
    }
}