namespace RoboterWPF.Models;

public enum ElementType
{
    Obstacle,
    Wall,
    Letter,
    Robot
}

public class RobotElement
{
    public int X { get; set; } = 0;
    public int Y { get; set; } = 0;
    public string Letter { get; set; } = "";
    public ElementType Type { get; set; } = ElementType.Wall;

    public RobotElement()
    {
    }

    public RobotElement(int x, int y, ElementType type, string letter = "")
    {
        X = x;
        Y = y;
        Type = type;
        Letter = letter;
    }

    public RobotElement Clone()
    {
        return new RobotElement
        {
            X = X,
            Y = Y,
            Letter = Letter,
            Type = Type
        };
    }
}
