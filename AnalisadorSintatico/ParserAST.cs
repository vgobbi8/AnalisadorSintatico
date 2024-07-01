using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using AnalisadorSintatico.Arvore;

namespace AnalisadorSintatico
{
    public class ParserAST
    {
        private readonly List<Token> tokens;
        private int current = 0;

        public ParserAST(List<Token> tokens)
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

        public ProgramNode Parse()
        {
            var program = new ProgramNode();

            if (Match(TokenType.Var))
            {
                while (Check(TokenType.Int) || Check(TokenType.Real))
                {
                    program.Declarations.Add(DVar());
                }
            }

            while (!IsAtEnd())
            {
                program.Statements.Add(Com());
            }

            return program;
        }

        private VarDeclarationNode DVar()
        {
            var type = TIPO();
            var identifiers = new List<string>();
            identifiers.Add(Consume(TokenType.Identifier, "Expect identifier after type.").Lexeme);

            while (Match(TokenType.Comma))
            {
                identifiers.Add(Consume(TokenType.Identifier, "Expect identifier after ','.").Lexeme);
            }

            Consume(TokenType.Semicolon, "Expect ';' after variable declaration.");

            return new VarDeclarationNode(type, identifiers);
        }

        private string TIPO()
        {
            if (Match(TokenType.Int))
            {
                return "int";
            }
            else if (Match(TokenType.Real))
            {
                return "real";
            }
            else
            {
                throw new Exception("Expect type 'int' or 'real'.");
            }
        }

        private ASTNode Com()
        {
            if (Match(TokenType.While))
            {
                return CWhile();
            }
            else if (Match(TokenType.If))
            {
                return CIf();
            }
            else if (Match(TokenType.Identifier))
            {
                return Catr();
            }
            else
            {
                throw new Exception("Unexpected token in command list.");
            }
        }

        private ASTNode CWhile()
        {
            Consume(TokenType.OpenParen, "Expect '(' after 'while'.");
            var condition = ExprR();
            Consume(TokenType.CloseParen, "Expect ')' after while condition.");
            Consume(TokenType.OpenBrace, "Expect '{' before while block.");

            var body = new List<ASTNode>();
            while (!Check(TokenType.CloseBrace) && !IsAtEnd())
            {
                body.Add(Com());
            }

            Consume(TokenType.CloseBrace, "Expect '}' after while block.");

            return new WhileNode(condition, body);
        }

        private ASTNode CIf()
        {
            Consume(TokenType.OpenParen, "Expect '(' after 'if'.");
            var condition = ExprR();
            Consume(TokenType.CloseParen, "Expect ')' after if condition.");
            Consume(TokenType.OpenBrace, "Expect '{' before if block.");

            var thenBranch = new List<ASTNode>();
            while (!Check(TokenType.CloseBrace) && !IsAtEnd())
            {
                thenBranch.Add(Com());
            }

            Consume(TokenType.CloseBrace, "Expect '}' after if block.");
            var elseBranch = new List<ASTNode>();

            if (Match(TokenType.Else))
            {
                Consume(TokenType.OpenBrace, "Expect '{' before else block.");
                while (!Check(TokenType.CloseBrace) && !IsAtEnd())
                {
                    elseBranch.Add(Com());
                }
                Consume(TokenType.CloseBrace, "Expect '}' after else block.");
            }

            return new IfNode(condition, thenBranch, elseBranch);
        }

        private ASTNode Catr()
        {
            var identifier = Previous().Lexeme;
            Consume(TokenType.Assign, "Expect '=' after identifier.");
            var expression = Expr();
            Consume(TokenType.Semicolon, "Expect ';' after expression.");

            return new AssignmentNode(identifier, expression);
        }

        private ExpressionNode ExprR()
        {
            var left = Expr();
            if (Match(TokenType.Equal, TokenType.NotEqual, TokenType.Less, TokenType.Greater, TokenType.LessEqual, TokenType.GreaterEqual))
            {
                var @operator = Previous().Type;
                var right = Expr();
                return new BinaryOperationNode(left, @operator, right);
            }
            return left;
        }

        private ExpressionNode Expr()
        {
            var left = Term();
            while (Match(TokenType.Plus, TokenType.Minus))
            {
                var @operator = Previous().Type;
                var right = Term();
                left = new BinaryOperationNode(left, @operator, right);
            }
            return left;
        }

        private ExpressionNode Term()
        {
            var left = Factor();
            while (Match(TokenType.Multiply, TokenType.Divide))
            {
                var @operator = Previous().Type;
                var right = Factor();
                left = new BinaryOperationNode(left, @operator, right);
            }
            return left;
        }

        private ExpressionNode Factor()
        {
            var left = Power();
            while (Match(TokenType.Power))
            {
                var @operator = Previous().Type;
                var right = Power();
                left = new BinaryOperationNode(left, @operator, right);
            }
            return left;
        }

        private ExpressionNode Power()
        {
            if (Match(TokenType.Number))
            {
                return new LiteralNode(Previous().Lexeme);
            }
            else if (Match(TokenType.Identifier))
            {
                return new VariableNode(Previous().Lexeme);
            }
            else if (Match(TokenType.OpenParen))
            {
                var expr = Expr();
                Consume(TokenType.CloseParen, "Expect ')' after expression.");
                return expr;
            }
            else
            {
                throw new Exception("Expect expression.");
            }
        }
    }

    //public class ParserAST
    //{
    //    private readonly List<Token> tokens;
    //    private int current = 0;

    //    public ParserAST(List<Token> tokens)
    //    {
    //        this.tokens = tokens;
    //    }

    //    private Token Advance()
    //    {
    //        if (!IsAtEnd()) current++;
    //        return Previous();
    //    }

    //    private bool IsAtEnd() => Peek().Type == TokenType.EndOfFile;

    //    private Token Peek() => tokens[current];

    //    private Token Previous() => tokens[current - 1];

    //    private bool Match(params TokenType[] types)
    //    {
    //        foreach (var type in types)
    //        {
    //            if (Check(type))
    //            {
    //                Advance();
    //                return true;
    //            }
    //        }

    //        return false;
    //    }

    //    private bool Check(TokenType type)
    //    {
    //        if (IsAtEnd()) return false;
    //        return Peek().Type == type;
    //    }

    //    private Token Consume(TokenType type, string message)
    //    {
    //        if (Check(type)) return Advance();
    //        throw new Exception(message);
    //    }

    //    public ProgramNode Parse()
    //    {
    //        var program = new ProgramNode();
    //        if (Match(TokenType.Var))
    //        {
    //            program.Declarations.AddRange(DVar());
    //        }
    //        program.Statements.AddRange(ComL());
    //        return program;
    //    }

    //    private List<VarDeclarationNode> DVar()
    //    {
    //        var declarations = new List<VarDeclarationNode>();
    //        while (Check(TokenType.Int) || Check(TokenType.Real))
    //        {
    //            declarations.Add(Var());
    //        }
    //        return declarations;
    //    }

    //    private VarDeclarationNode Var()
    //    {
    //        var type = TIPO();
    //        var identifiers = new List<string>();
    //        do
    //        {
    //            identifiers.Add(Consume(TokenType.Identifier, "Expect identifier after type.").Lexeme);
    //        } while (Match(TokenType.Comma));
    //        Consume(TokenType.Semicolon, "Expect ';' after variable declaration.");
    //        return new VarDeclarationNode(type, identifiers);
    //    }

    //    private string TIPO()
    //    {
    //        if (Match(TokenType.Int))
    //        {
    //            return "int";
    //        }
    //        if (Match(TokenType.Real))
    //        {
    //            return "real";
    //        }
    //        throw new Exception("Expect type 'int' or 'real'.");
    //    }

    //    private List<ASTNode> ComL()
    //    {
    //        var statements = new List<ASTNode>();
    //        while (!IsAtEnd() && !Check(TokenType.CloseBrace))
    //        {
    //            statements.Add(Com());
    //        }
    //        return statements;
    //    }

    //    private ASTNode Com()
    //    {
    //        if (Match(TokenType.While))
    //        {
    //            return CWhile();
    //        }
    //        if (Match(TokenType.If))
    //        {
    //            return CIf();
    //        }
    //        if (Check(TokenType.Identifier))
    //        {
    //            return Catr();
    //        }
    //        throw new Exception("Unexpected token in command list.");
    //    }

    //    private ASTNode CWhile()
    //    {
    //        Consume(TokenType.OpenParen, "Expect '(' after 'while'.");
    //        var condition = ExprR();
    //        Consume(TokenType.CloseParen, "Expect ')' after while condition.");
    //        Consume(TokenType.OpenBrace, "Expect '{' before while block.");
    //        var body = ComL();
    //        Consume(TokenType.CloseBrace, "Expect '}' after while block.");
    //        return new WhileNode(condition, body);
    //    }

    //    private ASTNode CIf()
    //    {
    //        Consume(TokenType.OpenParen, "Expect '(' after 'if'.");
    //        var condition = ExprR();
    //        Consume(TokenType.CloseParen, "Expect ')' after if condition.");
    //        Consume(TokenType.OpenBrace, "Expect '{' before if block.");
    //        var ifBody = ComL();
    //        Consume(TokenType.CloseBrace, "Expect '}' after if block.");
    //        List<ASTNode>? elseBody = null;
    //        if (Match(TokenType.Else))
    //        {
    //            Consume(TokenType.OpenBrace, "Expect '{' before else block.");
    //            elseBody = ComL();
    //            Consume(TokenType.CloseBrace, "Expect '}' after else block.");
    //        }
    //        return new IfNode(condition, ifBody, elseBody);
    //    }

    //    private ASTNode Catr()
    //    {
    //        var variable = Consume(TokenType.Identifier, "Expect identifier.").Lexeme;
    //        Consume(TokenType.Assign, "Expect '=' after identifier.");
    //        var expression = Expr();
    //        Consume(TokenType.Semicolon, "Expect ';' after expression.");
    //        return new AssignmentNode(variable, expression);
    //    }

    //    private ExpressionNode ExprR()
    //    {
    //        var expr = Expr();
    //        return ExprI(expr);
    //    }

    //    private ExpressionNode ExprI(ExpressionNode left)
    //    {
    //        if (Match(TokenType.Equal, TokenType.NotEqual, TokenType.Less, TokenType.Greater, TokenType.LessEqual, TokenType.GreaterEqual))
    //        {
    //            var operatorToken = Previous().Type;
    //            var right = Expr();
    //            return new BinaryExpressionNode(left, operatorToken, right);
    //        }
    //        return left;
    //    }

    //    private ExpressionNode Expr()
    //    {
    //        var left = Term();
    //        return Expl(left);
    //    }

    //    private ExpressionNode Expl(ExpressionNode left)
    //    {
    //        while (Match(TokenType.Plus, TokenType.Minus))
    //        {
    //            var operatorToken = Previous().Type;
    //            var right = Term();
    //            left = new BinaryExpressionNode(left, operatorToken, right);
    //        }
    //        return left;
    //    }

    //    private ExpressionNode Term()
    //    {
    //        var left = Factor();
    //        return Terml(left);
    //    }

    //    private ExpressionNode Terml(ExpressionNode left)
    //    {
    //        while (Match(TokenType.Multiply, TokenType.Divide))
    //        {
    //            var operatorToken = Previous().Type;
    //            var right = Factor();
    //            left = new BinaryExpressionNode(left, operatorToken, right);
    //        }
    //        return left;
    //    }

    //    private ExpressionNode Factor()
    //    {
    //        if (Match(TokenType.OpenParen))
    //        {
    //            var expr = Expr();
    //            Consume(TokenType.CloseParen, "Expect ')' after expression.");
    //            return expr;
    //        }
    //        if (Match(TokenType.Identifier))
    //        {
    //            return new VariableNode(Previous().Lexeme);
    //        }
    //        if (Match(TokenType.Number))
    //        {
    //            return new LiteralNode(Previous().Lexeme);
    //        }
    //        throw new Exception("Expect expression.");
    //    }
    //}

}
