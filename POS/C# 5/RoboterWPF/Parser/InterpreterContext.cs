using RoboterWPF.Models;

namespace RoboterWPF.Parser;

/// <summary>
/// Context class for the Interpreter Pattern.
/// Holds the execution state and provides operations for the robot.
/// </summary>
public class InterpreterContext
{
    private readonly List<RobotElement> _elements;
    private RobotElement? _robot;
    private string _collectedLetters = "";
    private readonly List<string> _errors = new();
    private Action? _stepCallback;

    // Grid boundaries
    private int _gridWidth = 10;
    private int _gridHeight = 10;

    public InterpreterContext(List<RobotElement> elements)
    {
        _elements = elements;
        FindRobot();
    }

    // Properties for results
    public string CollectedLetters => _collectedLetters;
    public bool HasErrors => _errors.Count > 0;
    public List<string> Errors => _errors;
    public RobotElement? Robot => _robot;

    /// <summary>
    /// Sets the grid size for boundary checking.
    /// </summary>
    public void SetGridSize(int width, int height)
    {
        _gridWidth = width;
        _gridHeight = height;
    }

    public void SetStepCallback(Action callback)
    {
        _stepCallback = callback;
    }

    public void AddError(string message)
    {
        _errors.Add(message);
    }

    private void FindRobot()
    {
        _robot = null;
        foreach (var element in _elements)
        {
            if (element.Type == ElementType.Robot)
            {
                _robot = element;
                break;
            }
        }
    }

    /// <summary>
    /// Checks if a position is within grid boundaries.
    /// </summary>
    private bool IsWithinBounds(int x, int y)
    {
        return x >= 0 && x < _gridWidth && y >= 0 && y < _gridHeight;
    }

    /// <summary>
    /// Checks if the robot can move to the specified position.
    /// Returns false if out of bounds or blocked by obstacle/wall.
    /// </summary>
    public bool CanMoveTo(int x, int y)
    {
        // Check grid boundaries first
        if (!IsWithinBounds(x, y))
        {
            return false;
        }

        // Check for walls and obstacles
        foreach (var element in _elements)
        {
            if (element.X == x && element.Y == y)
            {
                if (element.Type == ElementType.Wall ||
                    element.Type == ElementType.Obstacle)
                {
                    return false;
                }
            }
        }
        return true;
    }

    public void MoveRobot(Direction direction)
    {
        if (_robot == null)
        {
            throw new InvalidOperationException("Cannot move: robot not found");
        }

        int newX = _robot.X;
        int newY = _robot.Y;

        switch (direction)
        {
            case Direction.UP:
                newY--;
                break;
            case Direction.DOWN:
                newY++;
                break;
            case Direction.LEFT:
                newX--;
                break;
            case Direction.RIGHT:
                newX++;
                break;
        }

        // Check boundaries
        if (!IsWithinBounds(newX, newY))
        {
            throw new InvalidOperationException(
                $"Cannot move {direction.ToDirectionString()}: position ({newX}, {newY}) is outside the grid boundaries (0-{_gridWidth - 1}, 0-{_gridHeight - 1})");
        }

        if (!CanMoveTo(newX, newY))
        {
            throw new InvalidOperationException(
                $"Cannot move {direction.ToDirectionString()}: obstacle or wall at position ({newX}, {newY})");
        }

        // Move the robot
        _robot.X = newX;
        _robot.Y = newY;

        // Trigger callback for UI update
        _stepCallback?.Invoke();
    }

    public void CollectLetter()
    {
        if (_robot == null)
        {
            throw new InvalidOperationException("Cannot collect: robot not found");
        }

        // Find letter at robot's position
        var letter = GetElementAt(_robot.X, _robot.Y, ElementType.Letter);

        if (letter == null)
        {
            throw new InvalidOperationException(
                $"No letter to collect at position ({_robot.X}, {_robot.Y})");
        }

        _collectedLetters += letter.Letter;

        // Remove the letter from the map
        _elements.Remove(letter);

        // Trigger callback for UI update
        _stepCallback?.Invoke();
    }

    private RobotElement? GetElementAt(int x, int y, ElementType type)
    {
        foreach (var element in _elements)
        {
            if (element.X == x && element.Y == y && element.Type == type)
            {
                return element;
            }
        }
        return null;
    }

    /// <summary>
    /// Checks if there is an obstacle or wall at the specified position.
    /// Also returns true if the position is out of bounds (treated as obstacle).
    /// </summary>
    public bool HasObstacleAt(int x, int y)
    {
        // Out of bounds is treated as an obstacle
        if (!IsWithinBounds(x, y))
        {
            return true;
        }

        foreach (var element in _elements)
        {
            if (element.X == x && element.Y == y)
            {
                if (element.Type == ElementType.Wall ||
                    element.Type == ElementType.Obstacle)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public bool HasLetterAt(int x, int y, string letter)
    {
        // Out of bounds cannot have a letter
        if (!IsWithinBounds(x, y))
        {
            return false;
        }

        foreach (var element in _elements)
        {
            if (element.X == x && element.Y == y &&
                element.Type == ElementType.Letter &&
                element.Letter == letter)
            {
                return true;
            }
        }
        return false;
    }

    public bool CheckCondition(Direction direction, string target, bool isObstacle)
    {
        if (_robot == null)
        {
            throw new InvalidOperationException("Cannot check condition: robot not found");
        }

        // Calculate position to check based on direction
        int checkX = _robot.X;
        int checkY = _robot.Y;

        switch (direction)
        {
            case Direction.UP:
                checkY--;
                break;
            case Direction.DOWN:
                checkY++;
                break;
            case Direction.LEFT:
                checkX--;
                break;
            case Direction.RIGHT:
                checkX++;
                break;
        }

        // Check the condition
        if (isObstacle)
        {
            return HasObstacleAt(checkX, checkY);
        }
        else
        {
            return HasLetterAt(checkX, checkY, target);
        }
    }
}
