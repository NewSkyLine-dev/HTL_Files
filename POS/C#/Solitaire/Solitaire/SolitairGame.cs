using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solitaire
{
    public enum FieldState
    {
        None,   // Kein Feld (außerhalb des Bretts)
        Empty,  // Feld ohne Stein
        Peg     // Feld mit Stein
    }

    public class SolitairGame
    {
        public readonly int Size = 7; // Englisches Kreuz: 7x7
        public FieldState[,] Board { get; private set; }

        public SolitairGame()
        {
            Board = new FieldState[Size, Size];
            InitializeBoard();
        }

        private void InitializeBoard()
        {
            // Englisches Kreuz: Ecken sind keine Felder
            for (int y = 0; y < Size; y++)
            {
                for (int x = 0; x < Size; x++)
                {
                    if ((x < 2 || x > 4) && (y < 2 || y > 4))
                        Board[x, y] = FieldState.None;
                    else
                        Board[x, y] = FieldState.Peg;
                }
            }
            // Mittleres Feld leer
            Board[3, 3] = FieldState.Empty;
        }

        // Prüft, ob ein Zug gültig ist (von x1,y1 nach x2,y2)
        public bool IsValidMove(int x1, int y1, int x2, int y2)
        {
            // Muss innerhalb des Bretts sein
            if (!IsInBounds(x1, y1) || !IsInBounds(x2, y2)) return false;
            // Startfeld muss Peg, Zielfeld muss Empty sein
            if (Board[x1, y1] != FieldState.Peg || Board[x2, y2] != FieldState.Empty) return false;
            // Muss genau 2 Felder entfernt sein (horizontal oder vertikal)
            int dx = x2 - x1;
            int dy = y2 - y1;
            if (Math.Abs(dx) == 2 && dy == 0)
            {
                int mx = x1 + dx / 2;
                if (Board[mx, y1] == FieldState.Peg) return true;
            }
            else if (Math.Abs(dy) == 2 && dx == 0)
            {
                int my = y1 + dy / 2;
                if (Board[x1, my] == FieldState.Peg) return true;
            }
            return false;
        }

        // Führt einen gültigen Zug aus (von x1,y1 nach x2,y2)
        public bool MakeMove(int x1, int y1, int x2, int y2)
        {
            if (!IsValidMove(x1, y1, x2, y2)) return false;
            int mx = (x1 + x2) / 2;
            int my = (y1 + y2) / 2;
            Board[x1, y1] = FieldState.Empty;
            Board[mx, my] = FieldState.Empty;
            Board[x2, y2] = FieldState.Peg;
            return true;
        }

        // Prüft, ob das Spiel gewonnen ist (nur noch ein Stein)
        public bool IsWon()
        {
            int count = 0;
            foreach (var f in Board)
                if (f == FieldState.Peg) count++;
            return count == 1;
        }

        // Prüft, ob noch ein Zug möglich ist (sonst verloren)
        public bool HasValidMove()
        {
            for (int y = 0; y < Size; y++)
            {
                for (int x = 0; x < Size; x++)
                {
                    if (Board[x, y] == FieldState.Peg)
                    {
                        // Prüfe alle 4 Richtungen
                        if (IsValidMove(x, y, x + 2, y) || IsValidMove(x, y, x - 2, y) ||
                            IsValidMove(x, y, x, y + 2) || IsValidMove(x, y, x, y - 2))
                            return true;
                    }
                }
            }
            return false;
        }

        // Prüft, ob Koordinaten im Brett liegen und Feld existiert
        private bool IsInBounds(int x, int y)
        {
            return x >= 0 && x < Size && y >= 0 && y < Size && Board[x, y] != FieldState.None;
        }
    }
}
