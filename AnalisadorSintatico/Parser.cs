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
        private Lexer _lexer;
        private Token _currentToken;

        public Parser(Lexer lexer)
        {
            _lexer = lexer;
            _currentToken = _lexer.NextToken();
        }

        public void Parse()
        {
            Expression();
            if (_currentToken.Type.TokenTypeEnum != TokenTypeEnum.EOF)
            {
                throw new Exception("Unexpected token: " + _currentToken.Lexeme);
            }
        }

        private void Expression()
        {
            Term();
            while (_currentToken.Type.TokenTypeEnum == TokenTypeEnum.ADD || _currentToken.Type.TokenTypeEnum == TokenTypeEnum.SUB)
            {
                if (_currentToken.Type.TokenTypeEnum == TokenTypeEnum.ADD)
                {
                    Consume(TokenTypeEnum.ADD);
                    Term();
                }
                else if (_currentToken.Type.TokenTypeEnum == TokenTypeEnum.SUB)
                {
                    Consume(TokenTypeEnum.SUB);
                    Term();
                }
            }
        }

        private void Term()
        {
            Factor();
            while (_currentToken.Type.TokenTypeEnum == TokenTypeEnum.MUL || _currentToken.Type.TokenTypeEnum == TokenTypeEnum.DIV)
            {
                if (_currentToken.Type.TokenTypeEnum == TokenTypeEnum.MUL)
                {
                    Consume(TokenTypeEnum.MUL);
                    Factor();
                }
                else if (_currentToken.Type.TokenTypeEnum == TokenTypeEnum.DIV)
                {
                    Consume(TokenTypeEnum.DIV);
                    Factor();
                }
            }
        }

        private void Factor()
        {
            if (_currentToken.Type.TokenTypeEnum == TokenTypeEnum.NUM)
            {
                Consume(TokenTypeEnum.NUM);
            }
            else if (_currentToken.Type.TokenTypeEnum == TokenTypeEnum.LParen)
            {
                Consume(TokenTypeEnum.LParen);
                Expression();
                Consume(TokenTypeEnum.RParen);
            }
            else if (_currentToken.Type.TokenTypeEnum == TokenTypeEnum.RESERVED)
            {
                Consume(TokenTypeEnum.RESERVED);
            }
            else
            {
                throw new Exception("Unexpected token: " + _currentToken.Lexeme);
            }
        }

        private void Consume(TokenTypeEnum type)
        {
            if (_currentToken.Type.TokenTypeEnum == type)
            {
                _currentToken = _lexer.NextToken();
            }
            else
            {
                throw new Exception("Unexpected token: " + _currentToken.Lexeme);
            }
        }
    }

    public class Parser2
    {
        private Lexer _lexer;
        private Token _currentToken;

        public Parser2(Lexer lexer)
        {
            _lexer = lexer;
            _currentToken = _lexer.NextToken();
        }

        public void Parse()
        {
            Program();
            if (_currentToken.Type.TokenTypeEnum != TokenTypeEnum.EOF)
            {
                throw new Exception("Unexpected token: " + _currentToken.Lexeme);
            }
        }
        public void Consume(TokenTypeEnum type) {
            if (_currentToken.Type.TokenTypeEnum == type)
            {
                _currentToken = _lexer.NextToken();
            }
            else
            {
                throw new Exception("Unexpected token: " + _currentToken.Lexeme);
            }
        }

        public void Program()
        {
            FunctionDeclaration();
            MainFunction();
        }

        public void FunctionDeclaration()
        {
            if (_currentToken.Type.TokenTypeEnum == TokenTypeEnum.RESERVED)
            {
                Consume(TokenTypeEnum.RESERVED);
                if (_currentToken.Type.TokenTypeEnum == TokenTypeEnum.ID)
                {
                    Consume(TokenTypeEnum.ID);
                    Consume(TokenTypeEnum.LParen);
                    Consume(TokenTypeEnum.RParen);
                    Block();
                    FunctionDeclaration();
                }
                else
                {
                    throw new Exception("Unexpected token: " + _currentToken.Lexeme);
                }
            }
        }

        public void MainFunction()
        {
            if (_currentToken.Type.TokenTypeEnum == TokenTypeEnum.RESERVED)
            {
                Consume(TokenTypeEnum.RESERVED);
                if (_currentToken.Type.TokenTypeEnum == TokenTypeEnum.ID)
                {
                    Consume(TokenTypeEnum.ID);
                    Consume(TokenTypeEnum.LParen);
                    Consume(TokenTypeEnum.RParen);
                    Block();
                }
                else
                {
                    throw new Exception("Unexpected token: " + _currentToken.Lexeme);
                }
            }
        }

        public void Block()
        {
            Consume(TokenTypeEnum.LBrace);
            StatementList();
            Consume(TokenTypeEnum.RBrace);
        }

        public void StatementList()
        {
            Statement();
            StatementList();
        }

        public void Statement()
        {
            Consume(TokenTypeEnum.RESERVED);
            Consume(TokenTypeEnum.ID);
            Consume(TokenTypeEnum.ATTR);
            Expression();
            Consume(TokenTypeEnum.SColon);
        }

        public void Expression()
        {
            Term();
            while (_currentToken.Type.TokenTypeEnum == TokenTypeEnum.ADD || _currentToken.Type.TokenTypeEnum == TokenTypeEnum.SUB)
            {
                if (_currentToken.Type.TokenTypeEnum == TokenTypeEnum.ADD)
                {
                    Consume(TokenTypeEnum.ADD);
                    Term();
                }
                else if (_currentToken.Type.TokenTypeEnum == TokenTypeEnum.SUB)
                {
                    Consume(TokenTypeEnum.SUB);
                    Term();
                }
            }
        }

        public void Term()
        {
            Factor();
            while (_currentToken.Type.TokenTypeEnum == TokenTypeEnum.MUL || _currentToken.Type.TokenTypeEnum == TokenTypeEnum.DIV)
            {
                if (_currentToken.Type.TokenTypeEnum == TokenTypeEnum.MUL)
                {
                    Consume(TokenTypeEnum.MUL);
                    Factor();
                }
                else if (_currentToken.Type.TokenTypeEnum == TokenTypeEnum.DIV)
                {
                    Consume(TokenTypeEnum.DIV);
                    Factor();
                }
            }
        }

        public void Factor()
        {
            if (_currentToken.Type.TokenTypeEnum == TokenTypeEnum.NUM)
            {
                Consume(TokenTypeEnum.NUM);
            }
            else if (_currentToken.Type.TokenTypeEnum == TokenTypeEnum.LParen)
            {
                Consume(TokenTypeEnum.LParen);
                Expression();
                Consume(TokenTypeEnum.RParen);
            }
            else if (_currentToken.Type.TokenTypeEnum == TokenTypeEnum.ID)
            {
                Consume(TokenTypeEnum.ID);
            }
            else
            {
                throw new Exception("Unexpected token: " + _currentToken.Lexeme);
            }
        }


    }
}
