using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPN;

public interface IExpression
{
    void Interpret(Context context);
    string ToInfix();
}



public sealed class NumberExpression(double value) : IExpression
{
    private readonly double _value = value;

    public void Interpret(Context context) => context.Push(_value);

    public string ToInfix() =>
        _value.ToString(CultureInfo.InvariantCulture);
}


public abstract class BinaryExpression(IExpression left, IExpression right, string op) : IExpression
{
    protected readonly IExpression Left = left;
    protected readonly IExpression Right = right;
    private readonly string _op = op;

    public abstract void Interpret(Context context);

    public string ToInfix() => $"({Left.ToInfix()} {_op} {Right.ToInfix()})";
}

public sealed class AddExpression(IExpression left, IExpression right) : BinaryExpression(left, right, "+")
{
    public override void Interpret(Context context)
    {
        Left.Interpret(context);
        Right.Interpret(context);
        double r = context.Pop();
        double l = context.Pop();
        context.Push(l + r);
    }
}

public sealed class SubtractExpression(IExpression left, IExpression right) : BinaryExpression(left, right, "-")
{
    public override void Interpret(Context context)
    {
        Left.Interpret(context);
        Right.Interpret(context);
        double r = context.Pop();
        double l = context.Pop();
        context.Push(l - r);
    }
}

public sealed class MultiplyExpression(IExpression left, IExpression right) : BinaryExpression(left, right, "*")
{
    public override void Interpret(Context context)
    {
        Left.Interpret(context);
        Right.Interpret(context);
        double r = context.Pop();
        double l = context.Pop();
        context.Push(l * r);
    }
}

public sealed class DivideExpression(IExpression left, IExpression right) : BinaryExpression(left, right, "/")
{
    public override void Interpret(Context context)
    {
        Left.Interpret(context);
        Right.Interpret(context);
        double r = context.Pop();
        double l = context.Pop();
        if (r == 0) throw new DivideByZeroException("Division durch Null.");
        context.Push(l / r);
    }
}

public sealed class PowerExpression(IExpression left, IExpression right) : BinaryExpression(left, right, "^")
{
    public override void Interpret(Context context)
    {
        Left.Interpret(context);
        Right.Interpret(context);
        double r = context.Pop();
        double l = context.Pop();
        context.Push(Math.Pow(l, r));
    }
}
