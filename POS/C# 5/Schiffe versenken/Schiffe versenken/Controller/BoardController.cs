using Schiffe_versenken.Models;

namespace Schiffe_versenken.Controller;

public class BoardController
{
    public Board Board { get; private set; }

    public BoardController()
    {
        Board = new Board();
    }

    public BoardController(Board board)
    {
        Board = board;
    }

    /// <summary>
    /// Prüft ob ein Schiff an der gegebenen Position platziert werden kann
    /// </summary>
    public bool CanPlaceShip(int startX, int startY, int size, bool isHorizontal)
    {
        for (int i = 0; i < size; i++)
        {
            int x = isHorizontal ? startX + i : startX;
            int y = isHorizontal ? startY : startY + i;

            // Prüfe Grenzen
            if (x < 0 || x >= Board.BoardSize || y < 0 || y >= Board.BoardSize)
                return false;

            // Prüfe ob Feld frei ist
            if (Board.Fields[x, y].State != Field.FieldState.Empty)
                return false;

            // Prüfe umliegende Felder (Schiffe dürfen sich nicht berühren)
            for (int dx = -1; dx <= 1; dx++)
            {
                for (int dy = -1; dy <= 1; dy++)
                {
                    int checkX = x + dx;
                    int checkY = y + dy;

                    if (checkX >= 0 && checkX < Board.BoardSize && checkY >= 0 && checkY < Board.BoardSize)
                    {
                        if (Board.Fields[checkX, checkY].State == Field.FieldState.Ship)
                            return false;
                    }
                }
            }
        }
        return true;
    }

    /// <summary>
    /// Platziert ein Schiff auf dem Board
    /// </summary>
    public bool PlaceShip(Ship ship, int startX, int startY, bool isHorizontal)
    {
        if (!CanPlaceShip(startX, startY, ship.Size, isHorizontal))
            return false;

        var positions = new List<(int X, int Y)>();
        for (int i = 0; i < ship.Size; i++)
        {
            int x = isHorizontal ? startX + i : startX;
            int y = isHorizontal ? startY : startY + i;

            positions.Add((x, y));
            Board.Fields[x, y].State = Field.FieldState.Ship;
        }

        ship.Positions = positions;
        ship.IsPlaced = true;
        Board.Ships.Add(ship);
        return true;
    }

    /// <summary>
    /// Entfernt ein Schiff vom Board
    /// </summary>
    public void RemoveShip(Ship ship)
    {
        foreach (var pos in ship.Positions)
        {
            Board.Fields[pos.X, pos.Y].State = Field.FieldState.Empty;
        }
        ship.Positions.Clear();
        ship.IsPlaced = false;
        Board.Ships.Remove(ship);
    }

    /// <summary>
    /// Versucht einen Treffer zu landen
    /// </summary>
    public (bool isHit, bool isSunk, Ship? ship) TryHit(int x, int y)
    {
        if (x < 0 || x >= Board.BoardSize || y < 0 || y >= Board.BoardSize)
            return (false, false, null);

        var field = Board.Fields[x, y];

        if (field.State == Field.FieldState.Hit || field.State == Field.FieldState.Miss)
            return (false, false, null); // Bereits beschossen

        if (field.State == Field.FieldState.Ship)
        {
            field.State = Field.FieldState.Hit;

            // Finde das getroffene Schiff
            var hitShip = Board.Ships.FirstOrDefault(s => s.Positions.Contains((x, y)));
            if (hitShip != null)
            {
                // Prüfe ob Schiff versenkt
                bool isSunk = hitShip.Positions.All(p => Board.Fields[p.X, p.Y].State == Field.FieldState.Hit);
                if (isSunk)
                {
                    hitShip.IsSunk = true;
                }
                return (true, isSunk, hitShip);
            }
            return (true, false, null);
        }

        field.State = Field.FieldState.Miss;
        return (false, false, null);
    }

    /// <summary>
    /// Prüft ob alle Schiffe versenkt wurden
    /// </summary>
    public bool AllShipsSunk()
    {
        return Board.Ships.All(s => s.IsSunk);
    }

    /// <summary>
    /// Erstellt die Standard-Schiffe laut Wikipedia Regeln
    /// </summary>
    public static List<Ship> CreateStandardShips()
    {
        return
        [
            new Ship("Schlachtschiff", 5),
            new Ship("Kreuzer", 4),
            new Ship("Kreuzer", 4),
            new Ship("Zerstörer", 3),
            new Ship("Zerstörer", 3),
            new Ship("Zerstörer", 3),
            new Ship("U-Boot", 2),
            new Ship("U-Boot", 2),
            new Ship("U-Boot", 2),
            new Ship("U-Boot", 2)
        ];
    }
}
