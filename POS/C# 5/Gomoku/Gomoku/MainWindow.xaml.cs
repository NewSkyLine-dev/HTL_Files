using Gomoku.Controller;
using Gomoku.Models;
using System.Windows;
using System.Windows.Controls;

namespace Gomoku;

public partial class MainWindow : Window
{
    private GameController gameFieldController;

    public MainWindow()
    {
        InitializeComponent();
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        if (!int.TryParse(tfNumberOfTiles.Text, out int tileCount))
        {
            MessageBox.Show("Number of tiles missing!");
            return;
        }

        GameField gameModel = new(tileCount)
        {
            NumberOfFields = tileCount
        };
        string player1 = ((ComboBoxItem)cbPlayer1.SelectedItem).Content.ToString(); // Netzwerk, Lokal
        string player2 = ((ComboBoxItem)cbPlayer2.SelectedItem).Content.ToString(); // Computer, Mensch

        gameModel.Player1 = new Player();
        gameModel.Player2 = new Player();

        switch ((player1, player2))
        {
            case ("Lokal", "Mensch"):
                gameModel.Player1.IsPlaying = true;
                gameFieldController = new HotSeatController(in gameModel);
                break;
            case ("Lokal", "Computer"):
                gameModel.Player2.IsComputer = true;
                gameModel.Player1.IsPlaying = true;
                gameFieldController = new PlayerVsAiController(in gameModel);
                break;
            case ("Netzwerk", "Computer"):
                break;
            case ("Netzwerk", "Mensch"):
                break;
        }
        DataContext = gameModel;

        GameGrid.Children.Clear();
        for (int i = 0; i < tileCount * tileCount; i++)
        {
            Button btn = new();
            Field buttonField = new();
            btn.DataContext = buttonField;
            btn.Click += (s, e) =>
            {
                gameFieldController.HandleClick(((Button)s).DataContext as Field);
            };

            GameGrid.Children.Add(btn);
        }
    }
}