namespace Schiffe_versenken.Models;

[Serializable]
public class AnswerPacket
{
    public bool Hit { get; set; }
    public bool Sunk { get; set; }
    public bool GameOver { get; set; }
}
