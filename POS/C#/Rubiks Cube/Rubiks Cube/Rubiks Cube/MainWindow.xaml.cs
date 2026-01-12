using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using System.Threading.Tasks;

namespace Rubik_s_Cube
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly List<ModelVisual3D> boxes;
        private AxisAngleRotation3D rotation;
        private RotateTransform3D transform;
        private DoubleAnimation animation;
        private Point3D center;
        private bool isAnimating = false; // Flag für Animation-Status
        private bool isSolving = false; // Flag für Lösungsvorgang
        private List<(Action<int, int>, int, int)> solutionMoves = new(); // Speichert Züge für Rückgängigmachen

        private readonly Random r = new();

        public MainWindow()
        {
            InitializeComponent();

            center = new(0, 0, 2.93);
            boxes =
            [
                Cube001,
                Cube002,
                Cube003,
                Cube004,
                Cube005,
                Cube006,
                Cube007,
                Cube008,
                Cube009,
                Cube010,
                Cube011,
                Cube012,
                Cube013,
                null, //leere Box in der Mitte
                Cube014,
                Cube015,
                Cube016,
                Cube017,
                Cube018,
                Cube019,
                Cube020,
                Cube021,
                Cube022,
                Cube023,
                Cube024,
                Cube025,
                Cube026,
            ];
            foreach (ModelVisual3D m in boxes)
            {
                if (m != null)
                {
                    m.Transform = new Transform3DGroup();
                }
            }
        }
        private void RotateY(int angle, int schicht)
        {
            if (isAnimating) return; // Verhindert neue Animationen während eine läuft

            isAnimating = true;
            rotation = new(new(0, 1, 0), 0);
            transform = new(rotation, center);
            animation = new(angle, TimeSpan.FromMilliseconds(isSolving ? 500 : 1000), FillBehavior.HoldEnd); // Schnellere Animation beim Lösen

            // Event-Handler für Animation-Ende registrieren
            animation.Completed += (s, e) => { isAnimating = false; };

            rotation.BeginAnimation(AxisAngleRotation3D.AngleProperty, animation);
            ModelVisual3D[,] temp = new ModelVisual3D[3, 3];
            for (int i = 0; i < 3; i++) //zeile
            {
                for (int j = 0; j < 3; j++)//spalte
                {
                    temp[i, j] = boxes[schicht * 9 + i + j * 3];
                    if (boxes[schicht * 9 + i + j * 3] != null)
                    {
                        ((Transform3DGroup)boxes[schicht * 9 + i + j * 3].Transform).Children.Add(transform);
                    }
                }

            }
            //90 rotation
            if (angle == -90)
            {
                for (int i = 0; i < 3; i++) //zeile
                {
                    for (int j = 0; j < 3; j++)//spalte
                    {
                        boxes[schicht * 9 + i + j * 3] = temp[2 - j, i];
                    }

                }
            }
            else
            {
                for (int i = 0; i < 3; i++) //zeile
                {
                    for (int j = 0; j < 3; j++)//spalte
                    {
                        boxes[schicht * 9 + i + j * 3] = temp[j, 2 - i];
                    }

                }
            }
        }
        private void RotateX(int angle, int schicht)
        {
            if (isAnimating) return; // Verhindert neue Animationen während eine läuft

            isAnimating = true;
            rotation = new(new Vector3D(1, 0, 0), 0);
            transform = new(rotation, center);
            animation = new(angle, TimeSpan.FromMilliseconds(isSolving ? 500 : 1000), FillBehavior.HoldEnd);

            // Event-Handler für Animation-Ende registrieren
            animation.Completed += (s, e) => { isAnimating = false; };

            rotation.BeginAnimation(AxisAngleRotation3D.AngleProperty, animation);
            ModelVisual3D[,] temp = new ModelVisual3D[3, 3];
            for (int i = 0; i < 3; i++) //zeile
            {
                for (int j = 0; j < 3; j++)//spalte
                {
                    temp[i, j] = boxes[schicht + i * 3 + j * 9];
                    if (boxes[schicht + i * 3 + j * 9] != null)
                    {
                        ((Transform3DGroup)boxes[schicht + i * 3 + j * 9].Transform).Children.Add(transform);
                    }
                }

            }
            //90 rotation
            if (angle == -90)
            {
                for (int i = 0; i < 3; i++) //zeile
                {
                    for (int j = 0; j < 3; j++)//spalte
                    {
                        boxes[schicht + i * 3 + j * 9] = temp[2 - j, i];
                    }

                }
            }
            else
            {
                for (int i = 0; i < 3; i++) //zeile
                {
                    for (int j = 0; j < 3; j++)//spalte
                    {
                        boxes[schicht + i * 3 + j * 9] = temp[j, 2 - i];
                    }

                }
            }
        }
        private void RotateZ(int angle, int schicht)
        {
            if (isAnimating) return; // Verhindert neue Animationen während eine läuft

            isAnimating = true;
            rotation = new(new(0, 0, 1), 0);
            transform = new(rotation, center);
            animation = new(angle, TimeSpan.FromMilliseconds(isSolving ? 500 : 1000), FillBehavior.HoldEnd);

            // Event-Handler für Animation-Ende registrieren
            animation.Completed += (s, e) => { isAnimating = false; };

            rotation.BeginAnimation(AxisAngleRotation3D.AngleProperty, animation);
            ModelVisual3D[,] temp = new ModelVisual3D[3, 3];
            for (int i = 0; i < 3; i++) //zeile
            {
                for (int j = 0; j < 3; j++)//spalte
                {
                    temp[i, j] = boxes[schicht * 3 + i + j * 9];
                    if (boxes[schicht * 3 + i + j * 9] != null)
                    {
                        ((Transform3DGroup)boxes[schicht * 3 + i + j * 9].Transform).Children.Add(transform);
                    }
                }

            }
            //90 rotation
            if (angle == -90)
            {
                for (int i = 0; i < 3; i++) //zeile
                {
                    for (int j = 0; j < 3; j++)//spalte
                    {
                        boxes[schicht * 3 + i + j * 9] = temp[2 - j, i];
                    }

                }
            }
            else
            {
                for (int i = 0; i < 3; i++) //zeile
                {
                    for (int j = 0; j < 3; j++)//spalte
                    {
                        boxes[schicht * 3 + i + j * 9] = temp[j, 2 - i];
                    }

                }
            }
        }

        // Event-Handler für den zufälligen Würfel-Button
        private async void Button1_Click(object sender, RoutedEventArgs e)
        {
            if (isAnimating || isSolving) return; // Verhindert neue Animationen während eine läuft oder gelöst wird

            var amount = r.Next(10, 21); // Zufällige Anzahl von Zügen zwischen 10 und 20

            for (int i = 0; i < amount; i++)
            {
                // Führe eine zufällige Rotation aus
                await Dispatcher.InvokeAsync(() =>
                {
                    // Speichere den Zug für potentielle Rückgängigmachung
                    var moveType = r.Next(3);
                    var angle = r.Next(2) == 0 ? 90 : -90;
                    var layer = r.Next(3);

                    switch (moveType)
                    {
                        case 0:
                            solutionMoves.Add((RotateX, -angle, layer)); // Umgekehrter Winkel für Rückgängigmachen
                            RotateX(angle, layer);
                            break;
                        case 1:
                            solutionMoves.Add((RotateY, -angle, layer));
                            RotateY(angle, layer);
                            break;
                        case 2:
                            solutionMoves.Add((RotateZ, -angle, layer));
                            RotateZ(angle, layer);
                            break;
                    }
                });
                // Warte bis die Animation abgeschlossen ist
                await WaitForAnimationCompleteAsync();
            }
        }

        // Lösungsbutton Event-Handler
        private async void SolveCube_Click(object sender, RoutedEventArgs e)
        {
            if (isAnimating || isSolving) return;

            isSolving = true;
            await SolveCubeAsync();
            isSolving = false;
        }

        // Asynchrone Lösungsmethode
        private async Task SolveCubeAsync()
        {
            // Erstelle eine Kopie der Züge in umgekehrter Reihenfolge
            var movesToSolve = new List<(Action<int, int>, int, int)>(solutionMoves);
            movesToSolve.Reverse();

            foreach (var (moveAction, angle, layer) in movesToSolve)
            {
                // Führe jeden Zug auf dem UI-Thread aus
                await Dispatcher.InvokeAsync(() =>
                {
                    moveAction(angle, layer);
                });

                // Warte bis die Animation abgeschlossen ist
                await WaitForAnimationCompleteAsync();
            }

            // Lösche die gespeicherten Züge nach der Lösung
            solutionMoves.Clear();
        }

        // Hilfsmethode zum Warten auf Animation-Ende
        private async Task WaitForAnimationCompleteAsync()
        {
            while (isAnimating)
            {
                await Task.Delay(50); // Prüfe alle 50ms
            }
        }        // Event-Handler für X-Achsen Rotationen
        private void RotateX0Forward_Click(object sender, RoutedEventArgs e) { if (!isSolving) RotateX(90, 0); }
        private void RotateX0Backward_Click(object sender, RoutedEventArgs e) { if (!isSolving) RotateX(-90, 0); }
        private void RotateX1Forward_Click(object sender, RoutedEventArgs e) { if (!isSolving) RotateX(90, 1); }
        private void RotateX1Backward_Click(object sender, RoutedEventArgs e) { if (!isSolving) RotateX(-90, 1); }
        private void RotateX2Forward_Click(object sender, RoutedEventArgs e) { if (!isSolving) RotateX(90, 2); }
        private void RotateX2Backward_Click(object sender, RoutedEventArgs e) { if (!isSolving) RotateX(-90, 2); }

        // Event-Handler für Y-Achsen Rotationen
        private void RotateY0Forward_Click(object sender, RoutedEventArgs e) { if (!isSolving) RotateY(90, 0); }
        private void RotateY0Backward_Click(object sender, RoutedEventArgs e) { if (!isSolving) RotateY(-90, 0); }
        private void RotateY1Forward_Click(object sender, RoutedEventArgs e) { if (!isSolving) RotateY(90, 1); }
        private void RotateY1Backward_Click(object sender, RoutedEventArgs e) { if (!isSolving) RotateY(-90, 1); }
        private void RotateY2Forward_Click(object sender, RoutedEventArgs e) { if (!isSolving) RotateY(90, 2); }
        private void RotateY2Backward_Click(object sender, RoutedEventArgs e) { if (!isSolving) RotateY(-90, 2); }

        // Event-Handler für Z-Achsen Rotationen
        private void RotateZ0Forward_Click(object sender, RoutedEventArgs e) { if (!isSolving) RotateZ(90, 0); }
        private void RotateZ0Backward_Click(object sender, RoutedEventArgs e) { if (!isSolving) RotateZ(-90, 0); }
        private void RotateZ1Forward_Click(object sender, RoutedEventArgs e) { if (!isSolving) RotateZ(90, 1); }
        private void RotateZ1Backward_Click(object sender, RoutedEventArgs e) { if (!isSolving) RotateZ(-90, 1); }
        private void RotateZ2Forward_Click(object sender, RoutedEventArgs e) { if (!isSolving) RotateZ(90, 2); }
        private void RotateZ2Backward_Click(object sender, RoutedEventArgs e) { if (!isSolving) RotateZ(-90, 2); }

        private void SolveButton_Click(object sender, RoutedEventArgs e)
        {
            if (isAnimating || isSolving) return; // Keine Interaktion während Animation oder Lösung

            isSolving = true;
            List<string> solution = [];

            // Umwandlung der Lösung in Rotationen
            foreach (string move in solution)
            {
                int angle = 90;
                int layer = 0;

                switch (move)
                {
                    case "U":
                        RotateX(angle, layer);
                        solutionMoves.Add((RotateX, angle, layer));
                        break;
                    case "D":
                        RotateX(-angle, layer);
                        solutionMoves.Add((RotateX, -angle, layer));
                        break;
                    case "L":
                        RotateY(-angle, layer);
                        solutionMoves.Add((RotateY, -angle, layer));
                        break;
                    case "R":
                        RotateY(angle, layer);
                        solutionMoves.Add((RotateY, angle, layer));
                        break;
                    case "F":
                        RotateZ(-angle, layer);
                        solutionMoves.Add((RotateZ, -angle, layer));
                        break;
                    case "B":
                        RotateZ(angle, layer);
                        solutionMoves.Add((RotateZ, angle, layer));
                        break;
                }
            }

            // Rückgängigmachen der Züge nach 2 Sekunden
            Task.Delay(2000).ContinueWith(_ =>
            {
                foreach (var (action, angle, layer) in solutionMoves.AsEnumerable().Reverse())
                {
                    Dispatcher.Invoke(() => action(angle, layer));
                }
            });

            isSolving = false;
        }
    }
}