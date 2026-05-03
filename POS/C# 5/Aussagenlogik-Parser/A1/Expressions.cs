using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace A1
{
    public abstract class UnaryExpression : AbstractExpression
    {
        public AbstractExpression Child { get; private set; }

        protected UnaryExpression(AbstractExpression child)
        {
            Child = child;
        }

        public override void CollectVariables(IList<char> variables)
        {
            Child.CollectVariables(variables);
        }
    }

    public abstract class BinaryExpression : AbstractExpression
    {
        public AbstractExpression Left { get; private set; }

        public AbstractExpression Right { get; private set; }

        protected BinaryExpression(AbstractExpression left, AbstractExpression right)
        {
            Left = left;
            Right = right;
        }

        public override void CollectVariables(IList<char> variables)
        {
            Left.CollectVariables(variables);
            Right.CollectVariables(variables);
        }
    }

    public sealed class VariableExpression : AbstractExpression
    {
        public char Name { get; private set; }

        public VariableExpression(char name)
        {
            Name = char.ToLowerInvariant(name);
        }

        public override bool Evaluate(Context context)
        {
            return context[Name];
        }

        public override void CollectVariables(IList<char> variables)
        {
            if (!variables.Contains(Name))
            {
                variables.Add(Name);
            }
        }
    }

    public sealed class NotExpression : UnaryExpression
    {
        public NotExpression(AbstractExpression child)
            : base(child)
        {
        }

        public override bool Evaluate(Context context)
        {
            return !Child.Evaluate(context);
        }
    }

    public sealed class AndExpression : BinaryExpression
    {
        public AndExpression(AbstractExpression left, AbstractExpression right)
            : base(left, right)
        {
        }

        public override bool Evaluate(Context context)
        {
            return Left.Evaluate(context) && Right.Evaluate(context);
        }
    }

    public sealed class OrExpression : BinaryExpression
    {
        public OrExpression(AbstractExpression left, AbstractExpression right)
            : base(left, right)
        {
        }

        public override bool Evaluate(Context context)
        {
            return Left.Evaluate(context) || Right.Evaluate(context);
        }
    }

    public sealed class ImplicationExpression : BinaryExpression
    {
        public ImplicationExpression(AbstractExpression left, AbstractExpression right)
            : base(left, right)
        {
        }

        public override bool Evaluate(Context context)
        {
            return !Left.Evaluate(context) || Right.Evaluate(context);
        }
    }

    public sealed class EquivalenceExpression : BinaryExpression
    {
        public EquivalenceExpression(AbstractExpression left, AbstractExpression right)
            : base(left, right)
        {
        }

        public override bool Evaluate(Context context)
        {
            return Left.Evaluate(context) == Right.Evaluate(context);
        }
    }
}
