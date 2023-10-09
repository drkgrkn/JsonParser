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
        public TokenType Type { get; init; }
        public string Value { get; init; }

        public Token(TokenType type, string value)
        {
            Type = type;
            Value = value;
        }
    }
}