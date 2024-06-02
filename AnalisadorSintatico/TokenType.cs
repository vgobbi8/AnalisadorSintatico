using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static AnalisadorSintatico.Enums;

namespace AnalisadorSintatico
{
    public class TokenType
    {
        public TokenTypeEnum TokenTypeEnum { get; set; }
        public string RegexStr { get; set; }


        public TokenType(TokenTypeEnum tokenTypeEnum, string regex)
        {
            TokenTypeEnum = tokenTypeEnum;
            RegexStr = regex;
        }

        public override string ToString()
        {
            return TokenTypeEnum.ToString();
        }

        public TokenType() { }

        public static TokenType TokenADD = new(TokenTypeEnum.ADD, @"\+");
        public static TokenType TokenSUB = new(TokenTypeEnum.SUB, @"\-");
        public static TokenType TokenMUL = new(TokenTypeEnum.MUL, @"\*");
        public static TokenType TokenDIV = new(TokenTypeEnum.DIV, @"\/");
        public static TokenType TokenMOD = new(TokenTypeEnum.MOD, @"\%");
        public static TokenType TokenATTR = new(TokenTypeEnum.ATTR, @"\=");
        public static TokenType TokenID = new(TokenTypeEnum.ID, @"[a-zA-Z_][a-zA-Z0-9_]*");
        public static TokenType TokenNUM = new(TokenTypeEnum.NUM, @"[0-9]+");
        public static TokenType TokenOP = new(TokenTypeEnum.OP, @"[<>=]");
        public static TokenType TokenRESERVED = new(TokenTypeEnum.RESERVED, @"(if|else|while|for|print|return|void)");
        public static TokenType TokenLParen = new(TokenTypeEnum.LParen, @"\(");
        public static TokenType TokenRParen = new(TokenTypeEnum.RParen, @"\)");
        public static TokenType TokenLBrace = new(TokenTypeEnum.LBrace, @"\{");
        public static TokenType TokenRBrace = new(TokenTypeEnum.RBrace, @"\}");
        public static TokenType TokenSColon = new(TokenTypeEnum.SColon, @";");
        public static TokenType TokenLiteral = new(TokenTypeEnum.Literal, @"\"".*?\""");
        public static TokenType TokenEOF = new(TokenTypeEnum.EOF, @"\0");

        public static TokenType CheckTokenType(string input)
        {
            if (Regex.IsMatch(input, TokenADD.RegexStr))
            {
                return TokenADD;
            }
            else if (Regex.IsMatch(input, TokenSUB.RegexStr))
            {
                return TokenSUB;
            }
            else if (Regex.IsMatch(input, TokenMUL.RegexStr))
            {
                return TokenMUL;
            }
            else if (Regex.IsMatch(input, TokenDIV.RegexStr))
            {
                return TokenDIV;
            }
            else if (Regex.IsMatch(input, TokenMOD.RegexStr))
            {
                return TokenMOD;
            }
            else if (Regex.IsMatch(input, TokenATTR.RegexStr))
            {
                return TokenATTR;
            }
            else if (Regex.IsMatch(input, TokenID.RegexStr))
            {
                return TokenID;
            }
            else if (Regex.IsMatch(input, TokenNUM.RegexStr))
            {
                return TokenNUM;
            }
            else if (Regex.IsMatch(input, TokenOP.RegexStr))
            {
                return TokenOP;
            }
            else if (Regex.IsMatch(input, TokenRESERVED.RegexStr))
            {
                return TokenRESERVED;
            }
            else if (Regex.IsMatch(input, TokenLParen.RegexStr))
            {
                return TokenLParen;
            }
            else if (Regex.IsMatch(input, TokenRParen.RegexStr))
            {
                return TokenRParen;
            }
            else if (Regex.IsMatch(input, TokenLBrace.RegexStr))
            {
                return TokenLBrace;
            }
            else if (Regex.IsMatch(input, TokenRBrace.RegexStr))
            {
                return TokenRBrace;
            }
            else if (Regex.IsMatch(input, TokenSColon.RegexStr))
            {
                return TokenSColon;
            }
            else if (Regex.IsMatch(input, TokenLiteral.RegexStr))
            {
                return TokenLiteral;
            }
            else if (Regex.IsMatch(input, TokenEOF.RegexStr))
            {
                return TokenEOF;
            }
            else
            {
                throw new Exception($"Invalid input {input}");
            }
        }
    }
}
