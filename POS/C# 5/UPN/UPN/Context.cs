using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPN;

public sealed class Context
{
    private readonly Stack<double> _stack = new();

    public void Push(double value) => _stack.Push(value);

    public double Pop()
    {
        if (_stack.Count == 0)
            throw new InvalidOperationException("Stack underflow: zu wenige Operanden.");
        return _stack.Pop();
    }

    public int Count => _stack.Count;
}
