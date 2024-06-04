// See https://aka.ms/new-console-template for more information
using AnalisadorSintatico;

Console.WriteLine("Hello, World!");


var input = @"

void main () {
    int a = 10;
    int b = 20;
    int c = a + b;
}



";
var lexer = new Lexer(input);
var tokens = lexer.Tokenize();
var parser = new Parser2(lexer);
parser.Parse();