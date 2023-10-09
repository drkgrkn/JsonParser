using System.Text.Json;

namespace JsonParser
{
    public class Parser
    {
        private readonly List<Token> _tokens;
        private int _idx;
        private Token _curr;
        private object? _result;

        public Parser(List<Token> tokens)
        {
            _tokens = tokens;
            _idx = 0;
            _curr = _tokens[_idx];
            _result = null;
        }


        public object? Decode()
        {
            var currentTokenType = _curr.TType;
            var obj = currentTokenType switch
            {
                TokenType.LBRACE => ParseObject(),
                TokenType.LBRACK => ParseList(),
                TokenType.STRING => _curr.Value,
                TokenType.NUMBER => ParseNumber(),
                TokenType.BOOL => bool.Parse(_curr.Value),
                TokenType.NULL => null,
                _ => throw new JsonException(),
            };

            ReadToken();
            if (_curr.TType == TokenType.EOF) return obj;
            else throw new JsonException();
        }
        private object? Parse()
        {
            var currentTokenType = _curr.TType;
            var obj = currentTokenType switch
            {
                TokenType.LBRACE => ParseObject(),
                TokenType.LBRACK => ParseList(),
                TokenType.STRING => _curr.Value,
                TokenType.NUMBER => ParseNumber(),
                TokenType.BOOL => bool.Parse(_curr.Value),
                TokenType.NULL => null,
                _ => throw new JsonException(),
            };

            ReadToken();
            return obj;
        }

        private void ReadToken()
        {
            if (_tokens.Count - 1 <= _idx)
            {
                _idx++;
                _curr = new Token(TokenType.EOF, string.Empty);
            }
            else
            {
                _idx++;
                _curr = _tokens[_idx];
            }
        }

        private Token? Peek()
        {
            if (_idx < _tokens.Count - 1) return _tokens[_idx + 1];
            else return null;
        }

        private object ParseNumber()
        {
            // Implement a number parser :-)
            return 1;
        }

        private Dictionary<string, object?> ParseObject()
        {
            var dict = new Dictionary<string, object?>();
            ReadToken();

            while (true)
            {
                var key = _curr.TType == TokenType.STRING ? _curr.Value : throw new JsonException();
                ReadToken();

                if (_curr.TType != TokenType.COLON) throw new JsonException();
                ReadToken();

                var value = _curr.IsValueType() ? Parse() : throw new JsonException();
                dict.Add(key, value);

                if (_curr.TType == TokenType.COMMA)
                {
                    ReadToken();
                    continue;
                }
                else if (_curr.TType == TokenType.RBRACE)
                {
                    break;
                }
                else throw new JsonException();
            }

            return dict;
        }

        private List<object?> ParseList()
        {
            var list = new List<object?>();
            ReadToken();

            while (true)
            {
                var value = _curr.IsValueType() ? Parse() : throw new JsonException();
                list.Add(value);

                if (_curr.TType == TokenType.COMMA)
                {
                    ReadToken();
                    continue;
                }
                else if (_curr.TType == TokenType.RBRACK)
                {
                    break;
                }
                else throw new JsonException();
            }

            return list;
        }
    }
}