﻿// See https://aka.ms/new-console-template for more information
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
    var parser1 = new Parser(tokens);
    parser1.Parse();

    ParserAST parser = new ParserAST(tokens);
    var ast = parser.Parse();

    CodeGenerator generator = new CodeGenerator(outputStream);
    ast.GenerateCode(generator);


    Console.WriteLine("Code generation completed successfully.");
}
catch (Exception ex)
{
    Console.WriteLine($"Parsing error: {ex.Message}");
}

outputStream.Close();

//try
//{
//    Parser parser = new Parser(tokens);
//    parser.Parse();
//    Console.WriteLine("Parsing completed successfully.");
//}
//catch (Exception ex)
//{
//    Console.WriteLine($"Parsing error: {ex.Message}");
//}