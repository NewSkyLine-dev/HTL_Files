namespace A1
{
    public enum TokenType
    {
        Variable,
        Not,
        And,
        Or,
        Implication,
        Equivalence,
        LeftParen,
        RightParen,
        End
    }

    public class Token
    {
        public TokenType Type { get; private set; }

        public string Value { get; private set; }

        public Token(TokenType type, string value)
        {
            Type = type;
            Value = value;
        }
    }
}