// See https://aka.ms/new-console-template for more information
using JsonParser;

var input = """
{
    "key": "value",
    "huh": null,
    "age": 1.55,
    "career": {
        "title": "software engineer",
        "wage": 126000,
    }
}
""";

var lexer = new Lexer(input);

var output = lexer.Tokenize();
foreach (var token in output)
{
    Console.WriteLine($"Type: {token.Type}, Value: {token.Value}");
}