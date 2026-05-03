using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Taschenrechner
{
    class Parser(string raw)
    {
        private List<Token> _tokens = Lexer.Tokenize(raw);
        private int _pos = 0;
        private Token? _current => _pos < _tokens.Count ? _tokens[_pos] : null;

        public IExpression Parse()
        {
            return ParseStrich();
        }

        private IExpression ParseStrich()
        {
            var left = ParsePunkt();

            while (_current != null && (_current.Value == "+" || _current.Value == "-"))
            {
                var op = _current.Value;
                _pos++;
                var right = ParsePunkt();
                left = new BinaryExpression(left, op, right);
            }

            return left;
        }

        private IExpression ParsePunkt()
        {
            var left = ParsePotenz();

            while (_current != null && (_current.Value == "*" || _current.Value == "/"))
            {
                var op = _current.Value;
                _pos++;
                var right = ParsePotenz();
                left = new BinaryExpression(left, op, right);
            }

            return left;
        }

        private IExpression ParsePotenz()
        {
            var left = ParseAtom();

            if (_current != null && _current.Value == "^")
            {
                _pos++;
                var right = ParsePotenz(); 
                return new BinaryExpression(left, "^", right);
            }

            return left;
        }

        private IExpression ParseAtom()
        {
            if (_current.Value == "(")
            {
                _pos++; 
                var expr = ParseStrich();
                _pos++;
                return expr;
            }

            return ParseZahl();
        }

        private IExpression ParseZahl()
        {
            string result = "";
            if (Regex.Match(_current.Value, @"^[a-z]$").Success)
            {
                result += Context.Variables[_current.Value];
                _pos++;
            }
            while (_current != null && int.TryParse(_current.Value, out _))
            { 
                result += _current.Value; 
                _pos++; 
            }
            return new NumberExpression(int.Parse(result));
        }
    }
}
