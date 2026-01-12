using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Solitaire
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            game = new SolitairGame();
            CreateBoardUI();
        }

        private SolitairGame game;

        private void CreateBoardUI()
        {
            // Entferne evtl. vorhandene Kinder
            MainGrid.Children.Clear();
            MainGrid.RowDefinitions.Clear();
            MainGrid.ColumnDefinitions.Clear();

            int size = game.Size;
            for (int i = 0; i < size; i++)
            {
                MainGrid.RowDefinitions.Add(new RowDefinition());
                MainGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    // using Solitaire; ist bereits vorhanden, daher FieldState direkt verwenden
                    if (game.Board[x, y] == Solitaire.FieldState.None) continue;
                    Border border = new Border
                    {
                        BorderBrush = Brushes.Black,
                        BorderThickness = new Thickness(1),
                        Background = Brushes.Beige,
                        AllowDrop = true
                    };
                    border.Drop += Border_Drop;
                    Grid.SetRow(border, y);
                    Grid.SetColumn(border, x);
                    MainGrid.Children.Add(border);

                    if (game.Board[x, y] == Solitaire.FieldState.Peg)
                    {
                        Ellipse peg = new Ellipse
                        {
                            Stroke = Brushes.Red,
                            StrokeThickness = 2,
                            Fill = Brushes.Red,
                            Tag = new Point(x, y)
                        };
                        peg.PreviewMouseLeftButtonDown += Ellipse_MouseLeftButtonDown;
                        Grid.SetRow(peg, y);
                        Grid.SetColumn(peg, x);
                        MainGrid.Children.Add(peg);
                    }
                }
            }
        }

        Ellipse moving = null;
        private Point clickPosition;

        private void Ellipse_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            moving = (Ellipse)sender;
            clickPosition = e.GetPosition(this);
            moving.IsHitTestVisible = false;
            DragDrop.DoDragDrop(moving, moving, DragDropEffects.All);
        }

        private void Border_Drop(object sender, DragEventArgs e)
        {
            if (moving != null)
            {
                int col = Grid.GetColumn((UIElement)sender);
                int row = Grid.GetRow((UIElement)sender);
                //check if move is allowed
                Grid.SetColumn(moving, col);
                Grid.SetRow(moving, row);

                moving.RenderTransform = null;
                moving.IsHitTestVisible = true;
                moving = null;
            }
        }

        private void Grid_PreviewDragOver(object sender, DragEventArgs e)
        {
            if (moving != null)
            {
                Point currentPosition = e.GetPosition(this);

                var transform = moving.RenderTransform as TranslateTransform;
                if (transform == null)
                {
                    transform = new TranslateTransform();
                    moving.RenderTransform = transform;
                }

                transform.X = currentPosition.X - clickPosition.X;
                transform.Y = currentPosition.Y - clickPosition.Y;
            }
        }

        private void Grid_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (moving != null)
            {
                moving.RenderTransform = null;
                moving.IsHitTestVisible = true;
                moving = null;
            }
        }
    }
}
