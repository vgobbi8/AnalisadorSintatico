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
        private static readonly Dictionary<string, TokenType> keywords = new Dictionary<string, TokenType>
    {
        { "var", TokenType.Var },
        { "int", TokenType.Int },
        { "real", TokenType.Real },
        { "while", TokenType.While },
        { "if", TokenType.If },
        { "else", TokenType.Else }
    };

        private static readonly List<(Regex, TokenType)> tokenDefinitions = new List<(Regex, TokenType)>
    {
        (new Regex(@"^var\b"), TokenType.Var),
        (new Regex(@"^int\b"), TokenType.Int),
        (new Regex(@"^real\b"), TokenType.Real),
        (new Regex(@"^while\b"), TokenType.While),
        (new Regex(@"^if\b"), TokenType.If),
        (new Regex(@"^else\b"), TokenType.Else),
        (new Regex(@"^[a-zA-Z_]\w*"), TokenType.Identifier),
        (new Regex(@"^\d+(\.\d+)?"), TokenType.Number),
        (new Regex(@"^;"), TokenType.Semicolon),
        (new Regex(@"^,"), TokenType.Comma),
        (new Regex(@"^="), TokenType.Assign),
        (new Regex(@"^\+"), TokenType.Plus),
        (new Regex(@"^-"), TokenType.Minus),
        (new Regex(@"^\*"), TokenType.Multiply),
        (new Regex(@"^/"), TokenType.Divide),
        (new Regex(@"^\^"), TokenType.Power),
        (new Regex(@"^<="), TokenType.LessEqual),
        (new Regex(@"^>="), TokenType.GreaterEqual),
        (new Regex(@"^=="), TokenType.Equal),
        (new Regex(@"^!="), TokenType.NotEqual),
        (new Regex(@"^<"), TokenType.Less),
        (new Regex(@"^>"), TokenType.Greater),
        (new Regex(@"^\("), TokenType.OpenParen),
        (new Regex(@"^\)"), TokenType.CloseParen),
        (new Regex(@"^\{"), TokenType.OpenBrace),
        (new Regex(@"^\}"), TokenType.CloseBrace),
    };

        private readonly string source;
        private int current = 0;

        public Lexer(string source)
        {
            this.source = source;
        }

        public List<Token> ScanTokens()
        {
            List<Token> tokens = new List<Token>();
            while (!IsAtEnd())
            {
                SkipWhitespaceAndComments();
                if (IsAtEnd()) break;
                Token token = ScanToken();
                if (token.Type != TokenType.Unknown)
                {
                    tokens.Add(token);
                }
                else
                {
                    Console.WriteLine($"Unexpected token: {token.Lexeme}");
                    current++;
                }
            }
            tokens.Add(new Token(TokenType.EndOfFile, ""));
            return tokens;
        }

        private void SkipWhitespaceAndComments()
        {
            while (!IsAtEnd() && char.IsWhiteSpace(Peek()))
            {
                current++;
            }
        }

        private Token ScanToken()
        {
            foreach (var (regex, type) in tokenDefinitions)
            {
                var match = regex.Match(source.Substring(current));
                if (match.Success)
                {
                    current += match.Length;
                    if (type == TokenType.Identifier && keywords.ContainsKey(match.Value))
                    {
                        return new Token(keywords[match.Value], match.Value);
                    }
                    if (type != TokenType.Unknown)
                    {
                        return new Token(type, match.Value);
                    }
                    break;
                }
            }

            return new Token(TokenType.Unknown, source[current].ToString());
        }

        private bool IsAtEnd() => current >= source.Length;

        private char Peek() => IsAtEnd() ? '\0' : source[current];
    }
}
