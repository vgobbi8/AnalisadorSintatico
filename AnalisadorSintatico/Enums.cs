using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalisadorSintatico
{
    public class Enums
    {
        public enum TokenType
        {
            Var, Int, Real, Identifier, Number, Semicolon, Comma,
            Assign, Plus, Minus, Multiply, Divide, Power, Less, Greater, LessEqual, GreaterEqual, Equal, NotEqual,
            While, If, Else, OpenParen, CloseParen, OpenBrace, CloseBrace, EndOfFile, Unknown
        }

    }
}
