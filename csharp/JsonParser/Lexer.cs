using System.Text.Json;

namespace JsonParser
{
    public class Lexer
    {
        private readonly char[] _input;
        private int _idx;
        private char _curr;
        public List<Token> _tokens;
        private readonly char[] WhiteSpaceCharacters = { ' ', '\t', '\n', '\r' };

        public Lexer(string input)
        {
            _input = input.ToCharArray();
            _idx = 0;
            _curr = _input[_idx];
            _tokens = new List<Token>();
        }

        public List<Token> Tokenize()
        {
            try
            {
                for (var t = NextToken(); t.Type != TokenType.EOF; t = NextToken())
                {
                    _tokens.Add(t);
                }
            }
            catch
            {
                throw new JsonException();
            }
            return _tokens;
        }
        public Token NextToken()
        {
            SkipWhiteSpace();

            var token = _curr switch
            {
                '{' => new Token(TokenType.LBRACE, "{"),
                '}' => new Token(TokenType.LBRACE, "}"),
                '[' => new Token(TokenType.LBRACK, "["),
                ']' => new Token(TokenType.LBRACK, "]"),
                ',' => new Token(TokenType.QUOTE, ","),
                ':' => new Token(TokenType.COLON, ":"),
                (char)0 => new Token(TokenType.EOF, string.Empty),
                '"' => ReadString(),
                //VALUES
                _ => ReadValue(),
            };
            ReadChar();
            return token;
        }

        private Token ReadValue()
        {
            if (TryReadBoolean(out var value))
            {
                return new Token(TokenType.BOOL, value);
            }
            else if (IsNull())
            {
                return new Token(TokenType.NULL, "null");
            }
            else
            {
                return ReadNumericValue();
            }
        }

        private void ReadChar()
        {
            if (_input.Length - 1 <= _idx)
            {
                _idx++;
                _curr = (char)0;
            }
            else
            {
                _idx++;
                _curr = _input[_idx];
            }
        }

        private char Peek()
        {
            if (_input.Length <= _idx)
            {
                return (char)0;
            }
            else return _input[_idx + 1];
        }

        private Token ReadString()
        {
            var result = string.Empty;
            ReadChar();
            while (true)
            {
                if (_curr == '"' || _curr == 0) break;
                result += _input[_idx];
                ReadChar();
            }
            return new Token(TokenType.STRING, result);
        }

        private void SkipWhiteSpace()
        {
            while (WhiteSpaceCharacters.Contains(_curr))
            {
                ReadChar();
            }
        }

        private bool TryReadBoolean(out string tokenValue)
        {
            tokenValue = string.Empty;
            var curr = _idx;

            while (true)
            {
                if (!IsChar(_input[curr])) break;
                tokenValue += _input[curr];
                curr += 1;
            }
            if (tokenValue == "true" || tokenValue == "false")
            {
                _idx = curr - 1;
                return true;
            }
            else return false;
        }
        private bool IsNull()
        {
            var result = string.Empty;
            var currIdx = _idx;

            while (IsChar(_input[currIdx]))
            {
                result += _input[currIdx];
                currIdx++;
            }
            if (result == "null")
            {
                _idx = currIdx - 1;
                return true;
            }
            else return false;
        }

        private Token ReadNumericValue()
        {
            var value = string.Empty;
            while (IsNumericChar())
            {
                value += _curr;
                ReadChar();
            }
            _idx -= 1;
            return new Token(TokenType.NUMBER, value);
        }
        private bool IsNumericChar()
        {
            return '0' <= _curr && _curr <= '9' || _curr == '.';
        }

        private static bool IsChar(char ch)
        {
            return 'a' <= ch && ch <= 'z'
            || 'A' <= ch && ch <= 'Z'
            || ch == '_';

        }
    }
}