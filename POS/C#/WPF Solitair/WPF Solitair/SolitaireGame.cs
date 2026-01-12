using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Solitair;

public enum CellState
{
    OutOfBounds,
    Empty,
    Peg
}

public enum BoardType
{
    Cross,
    Plus,
    Diamond,
    Triangle,
    Star
}

public class Position
{
    public int Row { get; set; }
    public int Col { get; set; }

    public Position(int row, int col)
    {
        Row = row;
        Col = col;
    }

    public override bool Equals(object? obj)
    {
        if (obj is Position other)
        {
            return Row == other.Row && Col == other.Col;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return Row * 1000 + Col;
    }
}

class SolitaireGame
{
    private const int BoardSize = 9;
    private readonly CellState[,] _board;
    public BoardType CurrentBoardType { get; private set; }
    public event EventHandler GameStateChanged;

    public SolitaireGame()
    {
        _board = new CellState[BoardSize, BoardSize];
        SetupBoard(BoardType.Cross);
    }

    public CellState GetCellState(int row, int col)
    {
        if (row < 0 || row >= BoardSize || col < 0 || col >= BoardSize)
        {
            return CellState.OutOfBounds;
        }
        return _board[row, col];
    }

    public bool IsValidPosition(int row, int col)
    {
        return row >= 0 && row < BoardSize && col >= 0 && col < BoardSize;
    }

    public void SetupBoard(BoardType boardType)
    {
        CurrentBoardType = boardType;

        for (int r = 0; r < BoardSize; r++)
        {
            for (int c = 0; c < BoardSize; c++)
            {
                _board[r, c] = CellState.OutOfBounds;
            }
        }

        switch (boardType)
        {
            case BoardType.Cross:
                SetupCrossBoard();
                break;
            case BoardType.Plus:
                SetupPlusBoard();
                break;
            case BoardType.Diamond:
                SetupDiamondBoard();
                break;
            case BoardType.Triangle:
                SetupTriangleBoard();
                break;
            case BoardType.Star:
                SetupStarBoard();
                break;
        }

        GameStateChanged?.Invoke(this, EventArgs.Empty);
    }

    private void SetupCrossBoard()
    {
        // Classic English cross (standard)
        for (int r = 0; r < 9; r++)
        {
            for (int c = 0; c < 9; c++)
            {
                if ((r >= 2 && r <= 6) || (c >= 2 && c <= 6))
                    _board[r, c] = CellState.Peg;
            }
        }
        _board[4, 4] = CellState.Empty;
    }

    private void SetupPlusBoard()
    {
        // Plus: only the central row and column
        for (int i = 0; i < 9; i++)
        {
            _board[4, i] = CellState.Peg;
            _board[i, 4] = CellState.Peg;
        }
        _board[4, 4] = CellState.Empty;
    }

    private void SetupDiamondBoard()
    {
        // Diamond: pegs form a diamond shape
        for (int r = 0; r < 9; r++)
        {
            for (int c = 0; c < 9; c++)
            {
                if (Math.Abs(r - 4) + Math.Abs(c - 4) <= 4)
                    _board[r, c] = CellState.Peg;
            }
        }
        _board[4, 4] = CellState.Empty;
    }

    private void SetupTriangleBoard()
    {
        // Triangle: lower left triangle (like French solitaire)
        for (int r = 0; r < 9; r++)
        {
            for (int c = 0; c <= r && c < 9; c++)
            {
                _board[r, c] = CellState.Peg;
            }
        }
        _board[0, 0] = CellState.Empty;
    }

    private void SetupStarBoard()
    {
        // Star: a 6-pointed star (hexagram) approximation
        // Use the diamond as a base, then remove the corners
        for (int r = 0; r < 9; r++)
        {
            for (int c = 0; c < 9; c++)
            {
                if (Math.Abs(r - 4) + Math.Abs(c - 4) <= 4)
                    _board[r, c] = CellState.Peg;
            }
        }
        // Remove the four corners to make a star
        _board[0, 0] = _board[0, 8] = _board[8, 0] = _board[8, 8] = CellState.OutOfBounds;
        _board[1, 1] = _board[1, 7] = _board[7, 1] = _board[7, 7] = CellState.OutOfBounds;
        _board[4, 4] = CellState.Empty;
    }

    public bool CanMovePeg(int fromRow, int fromCol, int toRow, int toCol)
    {
        // Check if positions are valid
        if (!IsValidPosition(fromRow, fromCol) || !IsValidPosition(toRow, toCol))
            return false;

        // Check if start has a peg and end is empty
        if (_board[fromRow, fromCol] != CellState.Peg || _board[toRow, toCol] != CellState.Empty)
            return false;

        // Check if the move is exactly 2 cells horizontally or vertically
        int rowDiff = Math.Abs(toRow - fromRow);
        int colDiff = Math.Abs(toCol - fromCol);

        if ((rowDiff == 2 && colDiff == 0) || (rowDiff == 0 && colDiff == 2))
        {
            // Calculate the position of the cell being jumped over
            int midRow = (fromRow + toRow) / 2;
            int midCol = (fromCol + toCol) / 2;

            // Check if there is a peg in the middle to jump over
            return _board[midRow, midCol] == CellState.Peg;
        }

        return false;
    }

    public bool MovePeg(int fromRow, int fromCol, int toRow, int toCol)
    {
        if (!CanMovePeg(fromRow, fromCol, toRow, toCol))
            return false;

        // Calculate the position of the cell being jumped over
        int midRow = (fromRow + toRow) / 2;
        int midCol = (fromCol + toCol) / 2;

        // Move the peg
        _board[fromRow, fromCol] = CellState.Empty;
        _board[midRow, midCol] = CellState.Empty;
        _board[toRow, toCol] = CellState.Peg;

        GameStateChanged?.Invoke(this, EventArgs.Empty);
        return true;
    }

    public bool IsGameOver()
    {
        return IsGameWon() || IsGameLost();
    }

    public bool IsGameWon()
    {
        int pegCount = 0;
        for (int r = 0; r < BoardSize; r++)
        {
            for (int c = 0; c < BoardSize; c++)
            {
                if (_board[r, c] == CellState.Peg)
                    pegCount++;
            }
        }
        return pegCount == 1;
    }

    public bool IsGameLost()
    {
        // Check if there are no valid moves left
        for (int fromRow = 0; fromRow < BoardSize; fromRow++)
        {
            for (int fromCol = 0; fromCol < BoardSize; fromCol++)
            {
                if (_board[fromRow, fromCol] == CellState.Peg)
                {
                    // Check possible moves in four directions
                    if (HasValidMove(fromRow, fromCol))
                        return false;
                }
            }
        }
        return true;
    }

    private bool HasValidMove(int row, int col)
    {
        // Check all four directions
        int[][] directions =
        [
                [-2, 0], // Up
                [2, 0],  // Down
                [0, -2], // Left
                [0, 2]   // Right
        ];

        foreach (var dir in directions)
        {
            int toRow = row + dir[0];
            int toCol = col + dir[1];
            if (CanMovePeg(row, col, toRow, toCol))
                return true;
        }
        return false;
    }

    public int GetPegCount()
    {
        int count = 0;
        for (int r = 0; r < BoardSize; r++)
        {
            for (int c = 0; c < BoardSize; c++)
            {
                if (_board[r, c] == CellState.Peg)
                    count++;
            }
        }
        return count;
    }
}
