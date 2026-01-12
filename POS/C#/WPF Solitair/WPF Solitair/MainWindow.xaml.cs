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

namespace WPF_Solitair
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SolitaireGame _game;
        private const int CellSize = 50;
        private Point _dragStart;
        private Position? _dragFrom;

        public MainWindow()
        {
            InitializeComponent();
            _game = new SolitaireGame();
            _game.GameStateChanged += Game_GameStateChanged;
            BoardTypeComboBox.SelectionChanged += BoardTypeComboBox_SelectionChanged;
            InitializeBoard();
            UpdateStatusText();
        }

        private void InitializeBoard()
        {
            BoardGrid.Children.Clear();
            BoardGrid.RowDefinitions.Clear();
            BoardGrid.ColumnDefinitions.Clear();

            const int boardSize = 9;

            // Create grid rows and columns
            for (int i = 0; i < boardSize; i++)
            {
                BoardGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(CellSize) });
                BoardGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(CellSize) });
            }

            // Create cells
            for (int row = 0; row < boardSize; row++)
            {
                for (int col = 0; col < boardSize; col++)
                {
                    CellState state = _game.GetCellState(row, col);
                    if (state == CellState.OutOfBounds)
                        continue;

                    // Cell background
                    Rectangle cellBackground = new()
                    {
                        Fill = state == CellState.Empty ? new SolidColorBrush(Colors.BurlyWood) : new SolidColorBrush(Colors.SandyBrown),
                        Stroke = new SolidColorBrush(Colors.Black),
                        StrokeThickness = 1
                    };
                    Grid.SetRow(cellBackground, row);
                    Grid.SetColumn(cellBackground, col);
                    BoardGrid.Children.Add(cellBackground);

                    // Allow dropping only on empty cells
                    if (state == CellState.Empty)
                    {
                        cellBackground.AllowDrop = true;
                        cellBackground.DragEnter += CellBackground_DragEnter;
                        cellBackground.DragLeave += CellBackground_DragLeave;
                        cellBackground.Drop += CellBackground_Drop;
                    }

                    // Create peg if needed
                    if (state == CellState.Peg)
                    {
                        Ellipse peg = CreatePeg(row, col);
                        Grid.SetRow(peg, row);
                        Grid.SetColumn(peg, col);
                        BoardGrid.Children.Add(peg);
                    }
                }
            }
        }

        private Ellipse CreatePeg(int row, int col)
        {
            Ellipse peg = new Ellipse
            {
                Width = CellSize * 0.8,
                Height = CellSize * 0.8,
                Fill = new SolidColorBrush(Colors.DarkGoldenrod),
                Stroke = new SolidColorBrush(Colors.Black),
                StrokeThickness = 1,
                Margin = new Thickness(CellSize * 0.1),
                Tag = new Position(row, col)
            };
            peg.MouseLeftButtonDown += Peg_MouseLeftButtonDown;
            return peg;
        }

        private void Peg_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Ellipse peg && e.LeftButton == MouseButtonState.Pressed)
            {
                _dragStart = e.GetPosition(null);
                _dragFrom = (Position)peg.Tag;
                DragDrop.DoDragDrop(peg, _dragFrom, DragDropEffects.Move);
            }
        }

        private void CellBackground_DragEnter(object sender, DragEventArgs e)
        {
            if (sender is Rectangle rect)
                rect.Fill = new SolidColorBrush(Colors.LightGreen);
        }

        private void CellBackground_DragLeave(object sender, DragEventArgs e)
        {
            if (sender is Rectangle rect)
                rect.Fill = new SolidColorBrush(Colors.BurlyWood);
        }

        private void CellBackground_Drop(object sender, DragEventArgs e)
        {
            if (sender is Rectangle rect && e.Data.GetDataPresent(typeof(Position)))
            {
                Position from = (Position)e.Data.GetData(typeof(Position));
                int toRow = Grid.GetRow(rect);
                int toCol = Grid.GetColumn(rect);
                if (_game.CanMovePeg(from.Row, from.Col, toRow, toCol))
                {
                    _game.MovePeg(from.Row, from.Col, toRow, toCol);
                    // Board will be redrawn by GameStateChanged
                }
                else
                {
                    // Invalid move, just redraw
                    InitializeBoard();
                }
            }
        }

        private void BoardTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (BoardTypeComboBox.SelectedIndex >= 0)
            {
                BoardType selectedType = (BoardType)BoardTypeComboBox.SelectedIndex;
                _game.SetupBoard(selectedType);
                InitializeBoard();
                UpdateStatusText();
            }
        }

        private void NewGameButton_Click(object sender, RoutedEventArgs e)
        {
            BoardType currentType = (BoardType)BoardTypeComboBox.SelectedIndex;
            _game.SetupBoard(currentType);
            InitializeBoard();
            UpdateStatusText();
        }

        private void Game_GameStateChanged(object? sender, EventArgs e)
        {
            // Refresh the board
            Application.Current.Dispatcher.Invoke(() =>
            {
                InitializeBoard();
                UpdateStatusText();
                CheckGameState();
            });
        }

        private void UpdateStatusText()
        {
            int pegCount = _game.GetPegCount();
            PegCountText.Text = $"Pegs: {pegCount}";
        }

        private void CheckGameState()
        {
            if (_game.IsGameWon())
            {
                StatusText.Text = "Congratulations! You won the game!";
                MessageBox.Show("Congratulations! You've won the game with only one peg remaining!", "Victory", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if (_game.IsGameLost())
            {
                StatusText.Text = "Game over! No more valid moves.";
                MessageBox.Show("Game over! No more valid moves available.", "Game Over", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                StatusText.Text = "Ready to play! Move pegs by dragging them.";
            }
        }
    }
}