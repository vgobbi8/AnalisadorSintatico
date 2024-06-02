// See https://aka.ms/new-console-template for more information
using AnalisadorSintatico;

Console.WriteLine("Hello, World!");


var input = @"

void funcaoTeste() {
    int a = 10;
    int b = 20;
    int c = a + b;
    print(c);
}

void main() {
    funcaoTeste();
}



";
var lexer = new Lexer(input);
var tokens = lexer.Tokenize();
var parser = new Parser2(lexer);
parser.Parse();