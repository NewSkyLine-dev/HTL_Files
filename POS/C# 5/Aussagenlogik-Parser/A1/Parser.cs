using System;
using System.Collections.Generic;

namespace A1
{
    public class Parser
    {
        private List<Token> _tokens;

        private int _pos = 0;

        private Token Current
        {
            get { return _pos < _tokens.Count ? _tokens[_pos] : _tokens[_tokens.Count - 1]; }
        }

        public Parser(string raw)
        {
            _tokens = Tokenizer.Tokenize(raw);
        }

        public AbstractExpression Parse()
        {
            if (_tokens.Count == 0)
            {
                throw new Exception("Die Formel ist leer.");
            }

            AbstractExpression.variables.Clear();

            AbstractExpression expression = ParseEquivalence();

            if (Current.Type != TokenType.End)
            {
                throw new Exception(string.Format("Unerwartetes Token '{0}'", Current.Value));
            }

            return expression;
        }

        private AbstractExpression ParseEquivalence()
        {
            AbstractExpression left = ParseImplication();

            if (Match(TokenType.Equivalence))
            {
                AbstractExpression right = ParseEquivalence();
                return new EquivalenceExpression(left, right);
            }

            return left;
        }

        private AbstractExpression ParseImplication()
        {
            AbstractExpression left = ParseOr();

            if (Match(TokenType.Implication))
            {
                AbstractExpression right = ParseImplication();
                return new ImplicationExpression(left, right);
            }

            return left;
        }

        private AbstractExpression ParseOr()
        {
            AbstractExpression left = ParseAnd();

            while (Match(TokenType.Or))
            {
                AbstractExpression right = ParseAnd();
                left = new OrExpression(left, right);
            }

            return left;
        }

        private AbstractExpression ParseAnd()
        {
            AbstractExpression left = ParseNot();

            while (Match(TokenType.And))
            {
                AbstractExpression right = ParseNot();
                left = new AndExpression(left, right);
            }

            return left;
        }

        private AbstractExpression ParseNot()
        {
            if (Match(TokenType.Not))
            {
                return new NotExpression(ParseNot());
            }

            return ParseAtom();
        }

        private AbstractExpression ParseAtom()
        {
            if (Match(TokenType.LeftParen))
            {
                AbstractExpression expression = ParseEquivalence();
                Expect(TokenType.RightParen);
                return expression;
            }

            if (Current.Type == TokenType.Variable)
            {
                Token token = Take();
                char variable = token.Value[0];

                if (!AbstractExpression.variables.Contains(variable))
                {
                    AbstractExpression.variables.Add(variable);
                }

                return new VariableExpression(variable);
            }

            throw new Exception(string.Format("Unerwartetes Token '{0}'", Current.Value));
        }

        private bool Match(TokenType type)
        {
            if (Current.Type != type)
            {
                return false;
            }

            _pos++;
            return true;
        }

        private Token Expect(TokenType type)
        {
            if (Current.Type != type)
            {
                throw new Exception(string.Format("Erwartet '{0}' bekommen '{1}'", type, Current.Value));
            }

            return _tokens[_pos++];
        }

        private Token Take()
        {
            if (Current.Type == TokenType.End)
            {
                throw new Exception("Unerwartetes Ende");
            }

            return _tokens[_pos++];
        }
    }
}
