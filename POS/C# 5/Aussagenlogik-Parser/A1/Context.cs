using System;
using System.Collections.Generic;

namespace A1
{
    public sealed class Context
    {
        private readonly Dictionary<char, bool> _values;

        public Context(Dictionary<char, bool> values)
        {
            _values = values ?? new Dictionary<char, bool>();
        }

        public bool this[char variable]
        {
            get { return _values[variable]; }
        }

        public bool TryGetValue(char variable, out bool value)
        {
            return _values.TryGetValue(variable, out value);
        }

        public Dictionary<char, bool> Values
        {
            get { return _values; }
        }
    }
}
