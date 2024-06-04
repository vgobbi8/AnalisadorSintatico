using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AnalisadorSintatico
{
    public class AnalisadorCompleto
    {

        public enum TokenType
        {
            Var, Int, Real, Identifier, Number, Semicolon, Comma,
            Assign, Plus, Minus, Multiply, Divide, Power, Less, Greater, LessEqual, GreaterEqual, Equal, NotEqual,
            While, If, Else, OpenParen, CloseParen, OpenBrace, CloseBrace, EndOfFile, Unknown
        }

        public class Token
        {
            public TokenType Type { get; }
            public string Lexeme { get; }

            public Token(TokenType type, string lexeme)
            {
                Type = type;
                Lexeme = lexeme;
            }

            public override string ToString() => $"{Type} ({Lexeme})";
        }

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
        (new Regex(@"^\s+"), TokenType.Unknown), // Whitespace
        (new Regex(@"^var"), TokenType.Var),
        (new Regex(@"^int"), TokenType.Int),
        (new Regex(@"^real"), TokenType.Real),
        (new Regex(@"^while"), TokenType.While),
        (new Regex(@"^if"), TokenType.If),
        (new Regex(@"^else"), TokenType.Else),
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
                    Token token = ScanToken();
                    if (token.Type != TokenType.Unknown)
                    {
                        tokens.Add(token);
                    }
                }
                tokens.Add(new Token(TokenType.EndOfFile, ""));
                return tokens;
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

                return new Token(TokenType.Unknown, source[current++].ToString());
            }

            private bool IsAtEnd() => current >= source.Length;
        }



        public class Parser
        {
            private List<Token> tokens;
            private int current = 0;

            public Parser(List<Token> tokens)
            {
                this.tokens = tokens;
            }

            public void Parse()
            {
                Program();
            }

            private void Program()
            {
                Match(TokenType.Var);
                VarDeclList();
                StmtList();
            }

            private void VarDeclList()
            {
                while (Check(TokenType.Int) || Check(TokenType.Real))
                {
                    VarDecl();
                }
            }

            private void VarDecl()
            {
                if (Match(TokenType.Int) || Match(TokenType.Real))
                {
                    IdList();
                    Match(TokenType.Semicolon);
                }
                else
                {
                    Error("Expected variable type (int or real).");
                }
            }

            private void IdList()
            {
                Match(TokenType.Identifier);
                while (Match(TokenType.Comma))
                {
                    Match(TokenType.Identifier);
                }
            }

            private void StmtList()
            {
                while (!IsAtEnd() && !Check(TokenType.CloseBrace))
                {
                    Stmt();
                }
            }

            private void Stmt()
            {
                if (Check(TokenType.Identifier))
                {
                    AssignStmt();
                }
                else if (Check(TokenType.While))
                {
                    WhileStmt();
                }
                else if (Check(TokenType.If))
                {
                    IfStmt();
                }
                else
                {
                    Error("Expected statement.");
                }
            }

            private void AssignStmt()
            {
                Match(TokenType.Identifier);
                Match(TokenType.Assign);
                Expr();
                Match(TokenType.Semicolon);
            }

            private void WhileStmt()
            {
                Match(TokenType.While);
                Match(TokenType.OpenParen);
                RelExpr();
                Match(TokenType.CloseParen);
                Match(TokenType.OpenBrace);
                StmtList();
                Match(TokenType.CloseBrace);
            }

            private void IfStmt()
            {
                Match(TokenType.If);
                Match(TokenType.OpenParen);
                RelExpr();
                Match(TokenType.CloseParen);
                Match(TokenType.OpenBrace);
                StmtList();
                Match(TokenType.CloseBrace);
                if (Match(TokenType.Else))
                {
                    Match(TokenType.OpenBrace);
                    StmtList();
                    Match(TokenType.CloseBrace);
                }
            }

            private void Expr()
            {
                Term();
                while (Match(TokenType.Plus) || Match(TokenType.Minus))
                {
                    Term();
                }
            }

            private void Term()
            {
                Factor();
                while (Match(TokenType.Multiply) || Match(TokenType.Divide) || Match(TokenType.Power))
                {
                    Factor();
                }
            }

            private void Factor()
            {
                if (Match(TokenType.Identifier) || Match(TokenType.Number))
                {
                    return;
                }
                if (Match(TokenType.OpenParen))
                {
                    Expr();
                    Match(TokenType.CloseParen);
                }
                else
                {
                    Error("Expected identifier, number, or '('");
                }
            }

            private void RelExpr()
            {
                Expr();
                if (Match(TokenType.Less) || Match(TokenType.Greater) || Match(TokenType.LessEqual) ||
                    Match(TokenType.GreaterEqual) || Match(TokenType.Equal) || Match(TokenType.NotEqual))
                {
                    Expr();
                }
                else
                {
                    Error("Expected relational operator");
                }
            }

            private bool Match(TokenType type)
            {
                if (Check(type))
                {
                    Advance();
                    return true;
                }
                return false;
            }

            private bool Check(TokenType type)
            {
                if (IsAtEnd()) return false;
                return Peek().Type == type;
            }

            private Token Advance()
            {
                if (!IsAtEnd()) current++;
                return Previous();
            }

            private bool IsAtEnd() => Peek().Type == TokenType.EndOfFile;

            private Token Peek() => tokens[current];

            private Token Previous() => tokens[current - 1];

            private void Error(string message)
            {
                throw new Exception($"[Error] {message} at {Peek().Lexeme}");
            }
        }


    }
}
