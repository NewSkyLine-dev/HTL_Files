using System.IO;
using System.Windows;
using Microsoft.Win32;
using RoboterWPF.Models;
using RoboterWPF.Parser;
using RoboterWPF.Token;

namespace RoboterWPF;

public partial class MainWindow : Window
{
    private readonly LevelParser _levelParser = new();
    private readonly Tokenizer _tokenizer = new();
    private string _programContent = "";
    private ProgramExpression? _ast;
    private bool _isExecuting = false;

    public MainWindow()
    {
        InitializeComponent();
    }

    private void OpenLevelMenuItem_Click(object sender, RoutedEventArgs e)
    {
        var openFileDialog = new OpenFileDialog
        {
            Filter = "XML Files (*.xml)|*.xml|All Files (*.*)|*.*",
            Title = "Open Level"
        };

        if (openFileDialog.ShowDialog() == true)
        {
            var elements = _levelParser.ParseLevel(openFileDialog.FileName);
            RobotMapControl.SetSize(_levelParser.Width, _levelParser.Height);
            RobotMapControl.SetElements(elements);
        }
    }

    private void OpenProgramMenuItem_Click(object sender, RoutedEventArgs e)
    {
        var openFileDialog = new OpenFileDialog
        {
            Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*",
            Title = "Open Program"
        };

        if (openFileDialog.ShowDialog() == true)
        {
            _programContent = File.ReadAllText(openFileDialog.FileName);
            ProgramInput.Text = _programContent;
            ParseProgram();
        }
    }

    private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
    {
        Application.Current.Shutdown();
    }

    private void ParseProgram()
    {
        string programText = _programContent;

        // Step 1: Tokenize
        var tokens = _tokenizer.Tokenize(programText);

        // Step 2: Parse and build AST (Abstract Syntax Tree of Expression objects)
        var parser = new Parser.Parser(tokens);
        _ast = parser.Parse();

        // Check for parse errors
        if (parser.HasErrors())
        {
            System.Diagnostics.Debug.WriteLine("\n=== Parse Errors ===");
            string errorMsg = "Parse Errors:\n";
            foreach (var error in parser.GetErrors())
            {
                errorMsg += error.ToString() + "\n";
            }
            MessageBox.Show(errorMsg, "Parse Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private async void ExecuteButton_Click(object sender, RoutedEventArgs e)
    {
        // Parse the program first if needed
        if (_ast == null)
        {
            _programContent = ProgramInput.Text;
            ParseProgram();
        }
        else if (_programContent != ProgramInput.Text)
        {
            // Re-parse if text changed
            _programContent = ProgramInput.Text;
            ParseProgram();
        }

        // Check if we have a valid AST
        if (_ast == null)
        {
            MessageBox.Show("No valid program to execute", "Execution Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        // Get the elements from the map
        var elements = RobotMapControl.GetElements();

        if (elements.Count == 0)
        {
            MessageBox.Show("No level loaded. Please load a level first.", "Execution Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        // Prevent multiple simultaneous executions
        if (_isExecuting)
        {
            return;
        }
        _isExecuting = true;
        ExecuteButton.IsEnabled = false;

        // Create the interpreter context - holds execution state
        var context = new InterpreterContext(elements);

        // Set grid boundaries for out-of-bounds checking
        context.SetGridSize(RobotMapControl.GridWidth, RobotMapControl.GridHeight);

        context.SetStepCallback(() =>
        {
            // Update UI on the dispatcher thread
            Dispatcher.Invoke(() =>
            {
                RobotMapControl.Render();
            });
            // Small delay for animation
            Thread.Sleep(300);
        });

        // Run execution on a background thread
        // The AST interprets itself - each Expression node has an Interpret() method
        await Task.Run(() =>
        {
            try
            {
                // Interpreter Pattern: The AST interprets itself
                // Simply call Interpret() on the root expression, and it forwards
                // the request down through all child expressions in the tree
                _ast.Interpret(context);
            }
            catch (Exception ex)
            {
                context.AddError(ex.Message);
            }
        });

        _isExecuting = false;
        ExecuteButton.IsEnabled = true;

        // Check for execution errors
        if (context.HasErrors)
        {
            System.Diagnostics.Debug.WriteLine("\n=== Execution Errors ===");
            string errorMsg = "Execution Errors:\n";
            foreach (var error in context.Errors)
            {
                System.Diagnostics.Debug.WriteLine(error);
                errorMsg += error + "\n";
            }
            MessageBox.Show(errorMsg, "Execution Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        else
        {
            // Show success message with collected letters
            string collectedLetters = context.CollectedLetters;
            string message = "Program executed successfully!\n";
            if (!string.IsNullOrEmpty(collectedLetters))
            {
                message += "Collected letters: " + collectedLetters;
            }
            else
            {
                message += "No letters collected.";
            }
            MessageBox.Show(message, "Execution Complete", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // Final map update
        RobotMapControl.Render();
    }
}
