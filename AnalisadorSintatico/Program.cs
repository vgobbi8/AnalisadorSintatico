// See https://aka.ms/new-console-template for more information
using AnalisadorSintatico;

Console.WriteLine("Hello, World!");


//var input = @"

//void main () {
//    int a = 10;
//    int b = 20;
//    int c = a + b;
//}



//";
//var lexer = new Lexer(input);
//var tokens = lexer.Tokenize();
//var parser = new Parser2(lexer);
//parser.Parse();


string source = @"
        var
            int cont, num;
            real cont2;

        num = 0;
        while (cont < 10) {
            cont2 = 3.1415 * cont ^ 2;
            if (cont < 5) {
                num = num + cont2;
            } else {
                cont = 0;
            }
            cont = cont + 1;
        }";

AnalisadorCompleto.Lexer lexer = new AnalisadorCompleto.Lexer(source);
List<AnalisadorCompleto.Token> tokens = lexer.ScanTokens();

foreach (var token in tokens)
{
    Console.WriteLine(token);
}

AnalisadorCompleto.Parser parser = new AnalisadorCompleto.Parser(tokens);
try
{
    parser.Parse();
    Console.WriteLine("Parsing completed successfully.");
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}