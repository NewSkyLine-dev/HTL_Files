namespace RoboterWPF.Parser;

// Direction enum for MOVE commands
public enum Direction
{
    UP,
    DOWN,
    LEFT,
    RIGHT
}

public static class DirectionExtensions
{
    public static string ToDirectionString(this Direction dir)
    {
        return dir switch
        {
            Direction.UP => "UP",
            Direction.DOWN => "DOWN",
            Direction.LEFT => "LEFT",
            Direction.RIGHT => "RIGHT",
            _ => "UNKNOWN"
        };
    }

    public static Direction FromString(string str)
    {
        return str.ToUpper() switch
        {
            "UP" => Direction.UP,
            "DOWN" => Direction.DOWN,
            "LEFT" => Direction.LEFT,
            "RIGHT" => Direction.RIGHT,
            _ => Direction.UP // default
        };
    }
}

/// <summary>
/// Abstract Expression - Base class for all AST nodes in the Interpreter Pattern.
/// Each node knows how to interpret itself given a context.
/// </summary>
public abstract class Expression
{
    /// <summary>
    /// Interprets this expression within the given context.
    /// This is the core method of the Interpreter Pattern.
    /// </summary>
    public abstract void Interpret(InterpreterContext context);

    /// <summary>
    /// Returns a string representation of this expression for debugging.
    /// </summary>
    public abstract string ToString(int indent = 0);

    protected static string GetIndent(int level)
    {
        return new string(' ', level * 2);
    }
}

/// <summary>
/// Abstract class for statement expressions (non-terminal expressions that represent commands).
/// </summary>
public abstract class StatementExpression : Expression
{
}

/// <summary>
/// Terminal Expression - COLLECT command.
/// Directly interprets the collect action without delegating to child expressions.
/// </summary>
public class CollectExpression : StatementExpression
{
    public override void Interpret(InterpreterContext context)
    {
        try
        {
            context.CollectLetter();
        }
        catch (Exception e)
        {
            context.AddError(e.Message);
            throw;
        }
    }

    public override string ToString(int indent = 0)
    {
        return GetIndent(indent) + "COLLECT";
    }
}

/// <summary>
/// Terminal Expression - MOVE command.
/// Directly interprets the move action without delegating to child expressions.
/// </summary>
public class MoveExpression : StatementExpression
{
    public Direction Direction { get; set; }

    public MoveExpression(Direction dir)
    {
        Direction = dir;
    }

    public override void Interpret(InterpreterContext context)
    {
        try
        {
            context.MoveRobot(Direction);
        }
        catch (Exception e)
        {
            context.AddError(e.Message);
            throw;
        }
    }

    public override string ToString(int indent = 0)
    {
        return GetIndent(indent) + "MOVE " + Direction.ToDirectionString();
    }
}

/// <summary>
/// Non-Terminal Expression - REPEAT block.
/// Contains child expressions and forwards interpret requests to them.
/// Implements the Composite pattern as part of the Interpreter pattern.
/// </summary>
public class RepeatExpression(int count) : StatementExpression
{
    public int Count { get; set; } = count;
    public List<StatementExpression> Body { get; } = [];

    public void AddStatement(StatementExpression stmt)
    {
        Body.Add(stmt);
    }

    public override void Interpret(InterpreterContext context)
    {
        for (int i = 0; i < Count; i++)
        {
            // Forward interpret to all child expressions
            foreach (var statement in Body)
            {
                statement.Interpret(context);
            }
        }
    }

    public override string ToString(int indent = 0)
    {
        var result = GetIndent(indent) + $"REPEAT {Count} {{\n";
        foreach (var stmt in Body)
        {
            result += stmt.ToString(indent + 1) + "\n";
        }
        result += GetIndent(indent) + "}";
        return result;
    }
}

/// <summary>
/// Condition for UNTIL and IF expressions.
/// This is a terminal expression that evaluates to a boolean.
/// </summary>
public class ConditionExpression(Direction dir, string target, bool isObstacle)
{
    public Direction Direction { get; set; } = dir;
    public string Target { get; set; } = target;
    public bool IsObstacle { get; set; } = isObstacle;

    /// <summary>
    /// Interprets the condition and returns true/false.
    /// </summary>
    public bool Interpret(InterpreterContext context)
    {
        return context.CheckCondition(Direction, Target, IsObstacle);
    }

    public override string ToString()
    {
        return Direction.ToDirectionString() + " IS-A " + Target;
    }
}

/// <summary>
/// Non-Terminal Expression - UNTIL loop.
/// Contains a condition expression and child statement expressions.
/// Forwards interpret requests to children while condition is not met.
/// </summary>
public class UntilExpression(ConditionExpression condition) : StatementExpression
{
    public ConditionExpression Condition { get; set; } = condition;
    public List<StatementExpression> Body { get; } = new();

    public void AddStatement(StatementExpression stmt)
    {
        Body.Add(stmt);
    }

    public override void Interpret(InterpreterContext context)
    {
        int iteration = 0;

        // Execute body while condition is NOT true
        while (!Condition.Interpret(context))
        {
            iteration++;

            // Forward interpret to all child expressions
            foreach (var statement in Body)
            {
                statement.Interpret(context);
            }
        }
    }

    public override string ToString(int indent = 0)
    {
        var result = GetIndent(indent) + $"UNTIL {Condition} {{\n";
        foreach (var stmt in Body)
        {
            result += stmt.ToString(indent + 1) + "\n";
        }
        result += GetIndent(indent) + "}";
        return result;
    }
}

/// <summary>
/// Non-Terminal Expression - IF conditional.
/// Contains a condition expression and child statement expressions.
/// Forwards interpret requests to children only if condition is true.
/// </summary>
public class IfExpression : StatementExpression
{
    public ConditionExpression Condition { get; set; }
    public List<StatementExpression> Body { get; } = new();

    public IfExpression(ConditionExpression condition)
    {
        Condition = condition;
    }

    public void AddStatement(StatementExpression stmt)
    {
        Body.Add(stmt);
    }

    public override void Interpret(InterpreterContext context)
    {
        // Interpret condition and forward to children if true
        if (Condition.Interpret(context))
        {
            // Forward interpret to all child expressions
            foreach (var statement in Body)
            {
                statement.Interpret(context);
            }
        }
    }

    public override string ToString(int indent = 0)
    {
        var result = GetIndent(indent) + $"IF {Condition} {{\n";
        foreach (var stmt in Body)
        {
            result += stmt.ToString(indent + 1) + "\n";
        }
        result += GetIndent(indent) + "}";
        return result;
    }
}

/// <summary>
/// Non-Terminal Expression - Program (root of AST).
/// Contains all top-level statement expressions.
/// The entry point for interpretation - forwards to all child expressions.
/// </summary>
public class ProgramExpression : Expression
{
    public List<StatementExpression> Statements { get; } = new();

    public void AddStatement(StatementExpression stmt)
    {
        Statements.Add(stmt);
    }

    public override void Interpret(InterpreterContext context)
    {
        // Forward interpret to all child statement expressions
        foreach (var statement in Statements)
        {
            statement.Interpret(context);
        }
    }

    public override string ToString(int indent = 0)
    {
        var result = GetIndent(indent) + "Program {\n";
        foreach (var stmt in Statements)
        {
            result += stmt.ToString(indent + 1) + "\n";
        }
        result += GetIndent(indent) + "}";
        return result;
    }
}
