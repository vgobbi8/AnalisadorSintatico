using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AnalisadorSintatico.Enums;

namespace AnalisadorSintatico
{


    public class Parser
    {
        private readonly List<Token> tokens;
        private int current = 0;

        public Parser(List<Token> tokens)
        {
            this.tokens = tokens;
        }

        private Token Advance()
        {
            if (!IsAtEnd()) current++;
            return Previous();
        }

        private bool IsAtEnd() => Peek().Type == TokenType.EndOfFile;

        private Token Peek() => tokens[current];

        private Token Previous() => tokens[current - 1];

        private bool Match(params TokenType[] types)
        {
            foreach (var type in types)
            {
                if (Check(type))
                {
                    Advance();
                    return true;
                }
            }

            return false;
        }

        private bool Check(TokenType type)
        {
            if (IsAtEnd()) return false;
            return Peek().Type == type;
        }

        private Token Consume(TokenType type, string message)
        {
            if (Check(type)) return Advance();
            throw new Exception(message);
        }

        // Implementação das regras da gramática

        public void Parse()
        {
            Prog();
        }

        private void Prog()
        {
            if (Match(TokenType.Var))
            {
                DVar();
                ComL();
            }
            else
            {
                ComL();
            }
        }

        private void DVar()
        {
         //   Var();
            while (Check(TokenType.Int) || Check(TokenType.Real))
            {
                Var();
            }
        }

        private void Var()
        {
            TIPO();
            Consume(TokenType.Identifier, "Expect identifier after type.");
            VarI();
        }

        private void VarI()
        {
            if (Match(TokenType.Comma))
            {
                Consume(TokenType.Identifier, "Expect identifier after ','.");
                VarI();
            }
            else
            {
                Consume(TokenType.Semicolon, "Expect ';' after variable declaration.");
            }
        }

        private void TIPO()
        {
            if (Match(TokenType.Int, TokenType.Real))
            {
                // Do nothing, just match
            }
            else
            {
                throw new Exception("Expect type 'int' or 'real'.");
            }
        }

        private void ComL()
        {
            while (!IsAtEnd() && !Check(TokenType.CloseBrace))
            {
                Com();
            }
        }

        private void Com()
        {
            if (Match(TokenType.While))
            {
                CWhile();
            }
            else if (Match(TokenType.If))
            {
                CIf();
            }
            else if (Match(TokenType.Identifier))
            {
                Catr();
            }
            else
            {
                throw new Exception("Unexpected token in command list.");
            }
        }

        private void CWhile()
        {
            Consume(TokenType.OpenParen, "Expect '(' after 'while'.");
            ExprR();
            Consume(TokenType.CloseParen, "Expect ')' after while condition.");
            Consume(TokenType.OpenBrace, "Expect '{' before while block.");
            ComL();
            Consume(TokenType.CloseBrace, "Expect '}' after while block.");
        }

        private void CIf()
        {
            Consume(TokenType.OpenParen, "Expect '(' after 'if'.");
            ExprR();
            Consume(TokenType.CloseParen, "Expect ')' after if condition.");
            Consume(TokenType.OpenBrace, "Expect '{' before if block.");
            ComL();
            Consume(TokenType.CloseBrace, "Expect '}' after if block.");
            CElse();
        }

        private void CElse()
        {
            if (Match(TokenType.Else))
            {
                Consume(TokenType.OpenBrace, "Expect '{' before else block.");
                ComL();
                Consume(TokenType.CloseBrace, "Expect '}' after else block.");
            }
        }

        private void Catr()
        {
            Consume(TokenType.Assign, "Expect '=' after identifier.");
            Expr();
            Consume(TokenType.Semicolon, "Expect ';' after expression.");
        }

        private void ExprR()
        {
            Expr();
            ExprI();
        }

        private void ExprI()
        {
            if (Match(TokenType.Equal, TokenType.NotEqual, TokenType.Less, TokenType.Greater, TokenType.LessEqual, TokenType.GreaterEqual))
            {
                Expr();
            }
        }

        private void Expr()
        {
            Term();
            Expl();
        }

        private void Expl()
        {
            while (Match(TokenType.Plus, TokenType.Minus))
            {
                Term();
            }
        }

        private void Term()
        {
            Factor();
            Terml();
        }

        private void Terml()
        {
            while (Match(TokenType.Multiply, TokenType.Divide))
            {
                Factor();
            }
        }

        private void Factor()
        {
            if (Match(TokenType.OpenParen))
            {
                Expr();
                Consume(TokenType.CloseParen, "Expect ')' after expression.");
            }
            else if (Match(TokenType.Identifier, TokenType.Number))
            {
                Factorl();
            }
            else
            {
                throw new Exception("Expect expression.");
            }
        }

        private void Factorl()
        {
            if (Match(TokenType.Power))
            {
                Consume(TokenType.Number, "Expect number after '^'.");
            }
        }
    }
    
}
