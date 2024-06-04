using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static AnalisadorSintatico.Enums;
namespace AnalisadorSintatico
{
    public class Lexer
    {
        private string _input;
        private int _position;

        public Lexer(string input)
        {
            _input = input;
            _position = 0;
        }
        //Entendendo o Lexer
        // 1 - O lexer irá obter um input 
        // 2 - O lexer irá buscar os tokens no input
        // 3 - O Token é uma classe que irá representar o token encontrado
        // 4 - O token possui um tipo e o seu lexema. Por exemplo, o token de um número possui o tipo NUM e o lexema é o número encontrado
        // Outro exemplo mais complexo é o token de um identificador, que possui o tipo ID e o lexema é o identificador encontrado (ex: x, y, z)
        // Pode ser também um token de um operador, que possui o tipo OP e o lexema é o operador encontrado (ex: +, -, *, /)



        //Entendendo a lógica do parser
        // 1 - O parser depende do lexer, que irá fornecer os tokens
        // 2 - O parser irá analisar os tokens fornecidos pelo lexer
        // 3 - O parser irá verificar se a sequência de tokens é válida
        // 4 - O parser irá gerar uma árvore sintática ou executar ações com base nos tokens fornecidos


        public Token NextToken()
        {
            if (_position >= _input.Length)
            {
                return new Token(TokenType.TokenEOF, null);
            }



            string[] cleanTokens = Regex.Split(_input, @"(\s+)");
            string newString = String.Join("", cleanTokens);
            string[] tokens = Regex.Split(newString, @"(\s+)|([{};=+\-*/%()])");
            tokens = tokens.Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
            string currentToken = tokens[_position].ToString();
            _position++;

            //It's necessary to clear the empty spaces
            if (currentToken == "")
            {
                return NextToken();
            }

            if (Regex.IsMatch(currentToken, @"//"))
            {
                while (currentToken != "\n")
                {
                    currentToken = _input[_position].ToString();
                    _position++;
                }
                return NextToken();
            }
            var tokenTypeEnum = TokenType.CheckTokenType(currentToken);
            return (new Token(tokenTypeEnum, currentToken));

            //if (Regex.IsMatch(currentToken, TokenType.TokenRESERVED.RegexStr.ToString()))
            //{
            //    return new Token(TokenType.TokenRESERVED, currentToken);
            //}
            //else if (Regex.IsMatch(currentToken, @"[a-zA-Z_]"))
            //{
            //    return new Token(TokenType.TokenID, currentToken);

            //}
            //else if (Regex.IsMatch(currentToken, @"[0-9]"))
            //{
            //    return new Token(TokenType.TokenNUM, currentToken);
            //}
            //else if (Regex.IsMatch(currentToken, @"[<>=]"))
            //{
            //    return new Token(TokenType.TokenOP, currentToken);
            //}
            //else if (Regex.IsMatch(currentToken, @"[+*/%-]"))
            //{
            //    return new Token(TokenType.TokenADD, currentToken);
            //}
            //else if (Regex.IsMatch(currentToken, @"="))
            //{
            //    return new Token(TokenType.TokenATTR, currentToken);
            //}
            //else if (Regex.IsMatch(currentToken, @"[(]"))
            //{
            //    return new Token(TokenType.TokenLParen, currentToken);
            //}
            //else if (Regex.IsMatch(currentToken, @"[)]"))
            //{
            //    return new Token(TokenType.TokenRParen, currentToken);
            //}
            //else if (Regex.IsMatch(currentToken, @"[{]"))
            //{
            //    return new Token(TokenType.TokenLBrace, currentToken);
            //}
            //else if (Regex.IsMatch(currentToken, @"[}]"))
            //{
            //    return new Token(TokenType.TokenRBrace, currentToken);
            //}
            //else if (Regex.IsMatch(currentToken, @";"))
            //{
            //    return new Token(TokenType.TokenSColon, currentToken);
            //}
            //else if (Regex.IsMatch(currentToken, @"\"".*?\"""))
            //{
            //    return new Token(TokenType.TokenLiteral, currentToken);
            //}

            throw new Exception("Invalid character");
        }

        public List<Token> Tokenize()
        {

            //It's necessary to clear the empty spaces, so we will split the input by the spaces
            string[] cleanTokens = Regex.Split(_input, @"(\s+)");
            string newString = String.Join("", cleanTokens);
            string[] tokens = Regex.Split(newString, @"(\s+)|([{};=+\-*/%()])");
            tokens = tokens.Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
            var lstTokens = new List<Token>();
            foreach (var token in tokens)
            {
                if (token == "")
                {
                    continue;
                }
                var tokenTypeEnum = TokenType.CheckTokenType(token);
                lstTokens.Add(new Token(tokenTypeEnum, token));
            }
            return lstTokens;
        }
    }
}
