// See https://aka.ms/new-console-template for more information
using AnalisadorSintatico;

Console.WriteLine("Hello, World!");

var currentPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
var outputPath = System.IO.Path.Combine(currentPath, "output.txt");
var outputStream = new System.IO.StreamWriter(outputPath);

string source = @"
        var
            int cont, num;
            real cont2;

        num = 0;
        while(cont < 10) {
            cont2 = 3.1415 * contador ^ 2;
            if (cont < 5) {
                num = num + cont2;
            }
            else {
                cont = 0;
            }
            cont = cont + 1;
        }";


Lexer lexer = new Lexer(source);
List<Token> tokens = lexer.ScanTokens();

try
{

    //Árvore sintática
    ParserAST parser = new ParserAST(tokens);
    var ast = parser.Parse();

    //Gerador de código -> Vai escrevendo nesta stream
    GeradorDeCodigo gerador = new GeradorDeCodigo(outputStream);
    ast.GeraC3E(gerador);


    Console.WriteLine("Código C3E gerado com sucesso! Veja o arquivo output.txt");
}
catch (Exception ex)
{
    Console.WriteLine($"Erro de parse: {ex.Message}");
}

outputStream.Close();
