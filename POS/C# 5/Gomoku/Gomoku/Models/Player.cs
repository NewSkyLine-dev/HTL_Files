namespace Gomoku.Models;

public class Player
{
    public bool IsComputer { get; set; } = false;
    public bool IsPlaying { get; set; } = false;
    public int Score { get; set; } = 0;
}
