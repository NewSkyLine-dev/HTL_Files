namespace Taschenrechner;

public interface IExpression
{
    public abstract int Interprete(Context ctx);
}

public class BinaryExpression(IExpression left, string op, IExpression right) : IExpression
{
    public int Interprete(Context ctx)
    {
        return op switch
        {
            "+" => left.Interprete(ctx) + right.Interprete(ctx),
            "-" => left.Interprete(ctx) - right.Interprete(ctx),
            "*" => left.Interprete(ctx) * right.Interprete(ctx),
            "/" => left.Interprete(ctx) / right.Interprete(ctx),
            "^" => (int)Math.Pow(left.Interprete(ctx), right.Interprete(ctx)),
            _ => throw new NotImplementedException()
        };
    }
}

public class NumberExpression(int num) : IExpression
{
    public int Interprete(Context ctx)
    {
        return num;
    }
}