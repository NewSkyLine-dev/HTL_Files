using AbcRobotCore;

namespace Robotersteuerung;

public interface IExpression
{
    void Interpret(Context context);
}

public class CollectExpression : IExpression
{
    public void Interpret(Context context) => context.Field.Collect();
}

public class MoveExpression(RobotField.Direction dir) : IExpression
{
    private readonly RobotField.Direction _dir = dir;

    public void Interpret(Context context) => context.Field.Move(_dir);
}

public class BlockExpression(List<IExpression> stmts) : IExpression
{
    private readonly List<IExpression> _stmts = stmts;

    public void Interpret(Context context) { foreach (var s in _stmts) s.Interpret(context); }
}

public class RepeatExpression(int count, BlockExpression body) : IExpression
{
    private readonly int _count = count;
    private readonly BlockExpression _body = body;

    public void Interpret(Context ctx)
    {
        for (int i = 0; i < _count; i++) _body.Interpret(ctx);
    }
}

public class IfExpression(RobotField.Direction dir, string cond, BlockExpression body) : IExpression
{
    private readonly RobotField.Direction _dir = dir;
    private readonly string _cond = cond; // OBSTACLE | LETTER
    private readonly BlockExpression _body = body;

    public void Interpret(Context ctx)
    {
        bool result = _cond == "OBSTACLE"
            ? ctx.Field.IsObstacle(_dir)
            : ctx.Field.IsLetter(_cond, _dir);
        if (result) _body.Interpret(ctx);
    }
}

public class UntilExpression(RobotField.Direction dir, string cond, BlockExpression body) : IExpression
{
    private readonly RobotField.Direction _dir = dir;
    private readonly string _cond = cond; // OBSTACLE | LETTER
    private readonly BlockExpression _body = body;

    public void Interpret(Context ctx)
    {
        while (true)
        {
            bool condMet = _cond == "OBSTACLE"
                ? ctx.Field.IsObstacle(_dir)
                : ctx.Field.IsLetter(_cond, _dir);
            if (condMet) break;
            _body.Interpret(ctx);
        }
    }
}