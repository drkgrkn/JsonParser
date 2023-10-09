using System.Net.Http.Headers;

namespace JsonParser
{
    public enum TokenType
    {
        LBRACE, //{
        RBRACE, //}
        LBRACK, //[
        RBRACK, //]
        QUOTE, //"
        COMMA, //,
        COLON, //:
        //VALUES
        BOOL, //true,false
        NUMBER, //0,1,2 or 0.001...
        STRING, //"*"
        NULL, //null
        EOF,
    }

    public class Token
    {
        public TokenType TType { get; init; }
        public string Value { get; init; }
        public TokenType[] ValueTypes =
        {
            TokenType.LBRACK,
            TokenType.LBRACE,
            TokenType.NULL,
            TokenType.NUMBER,
            TokenType.STRING,
            TokenType.BOOL,
        };

        public Token(TokenType type, string value)
        {
            TType = type;
            Value = value;
        }

        public bool IsValueType() => ValueTypes.Contains(TType);
    }
}