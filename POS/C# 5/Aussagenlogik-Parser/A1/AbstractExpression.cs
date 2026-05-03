using System;
using System.Collections.Generic;
using System.Text;

namespace A1
{
    public abstract class AbstractExpression
    {
        public static List<char> variables = new List<char>();

        public abstract bool Evaluate(Context context);

        public virtual void CollectVariables(IList<char> variables)
        {
        }
    }
}
