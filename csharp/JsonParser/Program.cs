// See https://aka.ms/new-console-template for more information
using JsonParser;

var input = """
{
    "key": "value",
    "huh": null,
    "age": 1.55,
    "career": {
        "title": "software engineer",
        "wage": 126000
    }
}
""";

var lexer = new Lexer(input);

var tokens = lexer.Tokenize();
foreach (var token in tokens)
{
    Console.WriteLine($"Type: {token.TType}, Value: {token.Value}");
}

var parser = new Parser(tokens);
var obj = parser.Decode();

Console.WriteLine(((Dictionary<string, object?>)obj).GetValueOrDefault("key"));