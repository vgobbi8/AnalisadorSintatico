using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AnalisadorSintatico.Enums;
namespace AnalisadorSintatico
{
    public class Token
    {
        public TokenType Type { get; set; }
        public string Lexeme { get; set; }

        public Token(TokenType type, string lexeme)
        {
            Type = type;
            Lexeme = lexeme;
        }
    }
}
