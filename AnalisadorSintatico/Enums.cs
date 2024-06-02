using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalisadorSintatico
{
    public class Enums
    {
        public enum TokenTypeEnum
        {
            ID,
            NUM,
            OP,
            RESERVED,
            EOF,
            ADD,
            SUB,
            MUL,
            DIV,
            MOD,
            ATTR,
            LParen,
            RParen,
            LBrace,
            RBrace,
            SColon,
            Literal
        }
    }
}
