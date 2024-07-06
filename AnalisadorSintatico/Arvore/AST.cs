using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AnalisadorSintatico.Enums;
namespace AnalisadorSintatico.Arvore
{
    public abstract class ASTNode
    {
        public abstract string GeraC3E(GeradorDeCodigo generator);
    }

    public class ProgramNode : ASTNode
    {
        public List<ASTNode> DeclaracaoVariaiveis { get; } = new List<ASTNode>();
        public List<ASTNode> StatementsPrograma { get; } = new List<ASTNode>();

        public override string GeraC3E(GeradorDeCodigo generator)
        {
            foreach (var declaration in DeclaracaoVariaiveis)
            {
                declaration.GeraC3E(generator);
            }
            foreach (var statement in StatementsPrograma)
            {
                statement.GeraC3E(generator);
            }
            return "";
        }
    }

    public class VarDeclarationsNode : ASTNode
    {
        public List<VarDeclarationNode> DeclaracaoVariaveis { get; }

        public VarDeclarationsNode(List<VarDeclarationNode> declarations)
        {
            DeclaracaoVariaveis = declarations;
        }

        public override string GeraC3E(GeradorDeCodigo generator)
        {
            foreach (var declaration in DeclaracaoVariaveis)
            {
                declaration.GeraC3E(generator);
            }
            return "";
        }
    }

    public class VarDeclarationNode : ASTNode
    {
        public string Type { get; }
        public List<string> Identifiers { get; }

        public VarDeclarationNode(string type, List<string> identifiers)
        {
            Type = type;
            Identifiers = identifiers;
        }

        public override string GeraC3E(GeradorDeCodigo generator)
        {
            foreach (var identifier in Identifiers)
            {
                generator.EmitDeclaration(Type, identifier);
            }
            return "";
        }
    }

    public class AssignmentNode : ASTNode
    {
        public string Identifier { get; }
        public ExpressionNode Expression { get; }

        public AssignmentNode(string identifier, ExpressionNode expression)
        {
            Identifier = identifier;
            Expression = expression;
        }

        public override string GeraC3E(GeradorDeCodigo generator)
        {
            var value = Expression.GeraC3E(generator);
            generator.Assign(Identifier, value);
            return "";
        }
    }

    public abstract class ExpressionNode : ASTNode
    {
        public override abstract string GeraC3E(GeradorDeCodigo generator);
    }

    public class OperacaoBinariaNode : ExpressionNode
    {
        public ExpressionNode Left { get; }
        public TokenType Operator { get; }
        public ExpressionNode Right { get; }

        public OperacaoBinariaNode(ExpressionNode left, TokenType op, ExpressionNode right)
        {
            Left = left;
            Operator = op;
            Right = right;
        }

        public override string GeraC3E(GeradorDeCodigo generator)
        {
            var leftTemp = Left.GeraC3E(generator);
            var rightTemp = Right.GeraC3E(generator);
            var temp = generator.GenerateTemp();
            generator.Emit($"{temp} = {leftTemp} {OperatorToString(Operator)} {rightTemp}");
            return temp;
        }

        private string OperatorToString(TokenType op)
        {
            return op switch
            {
                TokenType.Plus => "+",
                TokenType.Minus => "-",
                TokenType.Multiply => "*",
                TokenType.Divide => "/",
                TokenType.Power => "^",
                TokenType.Less => "<",
                TokenType.Greater => ">",
                TokenType.LessEqual => "<=",
                TokenType.GreaterEqual => ">=",
                TokenType.Equal => "==",
                TokenType.NotEqual => "!=",
                _ => throw new Exception($"Unknown operator {op}")
            };
        }
    }

    public class VariableNode : ExpressionNode
    {
        public string Name { get; }

        public VariableNode(string name)
        {
            Name = name;
        }

        public override string GeraC3E(GeradorDeCodigo generator)
        {
            return Name;
        }

    }

    public class LiteralNode : ExpressionNode
    {
        public string Value { get; }

        public LiteralNode(string value)
        {
            Value = value;
        }

        public override string GeraC3E(GeradorDeCodigo generator)
        {
            return Value;
        }

    }

    public class WhileNode : ASTNode
    {
        public ExpressionNode Condition { get; }
        public List<ASTNode> Body { get; }

        public WhileNode(ExpressionNode condition, List<ASTNode> body)
        {
            Condition = condition;
            Body = body;
        }

        public override string GeraC3E(GeradorDeCodigo generator)
        {
            var startLabel = generator.GenerateLabel("WHILE");
            var endLabel = generator.GenerateLabel("END");
            generator.Emit($"{startLabel}:");
            var conditionCode = Condition.GeraC3E(generator);
            generator.Emit($"if {conditionCode} == 0 goto {endLabel}");
            foreach (var statement in Body)
            {
                statement.GeraC3E(generator);
            }
            generator.Emit($"goto {startLabel}");
            generator.Emit($"{endLabel}:");
            return "";
        }
    }

    public class IfNode : ASTNode
    {
        public ExpressionNode Condition { get; }
        public List<ASTNode> ThenBranch { get; }
        public List<ASTNode> ElseBranch { get; }

        public IfNode(ExpressionNode condition, List<ASTNode> thenBranch, List<ASTNode> elseBranch)
        {
            Condition = condition;
            ThenBranch = thenBranch;
            ElseBranch = elseBranch;
        }

        public override string GeraC3E(GeradorDeCodigo generator)
        {
            var elseLabel = generator.GenerateLabel();
            var endLabel = generator.GenerateLabel();
            var conditionCode = Condition.GeraC3E(generator);
            generator.Emit($"if {conditionCode} == 0 goto {elseLabel}");
            foreach (var statement in ThenBranch)
            {
                statement.GeraC3E(generator);
            }
            generator.Emit($"goto {endLabel}");
            generator.Emit($"{elseLabel}:");
            if (ElseBranch != null)
            {
                foreach (var statement in ElseBranch)
                {
                    statement.GeraC3E(generator);
                }
            }
            generator.Emit($"{endLabel}:");
            return "";
        }
    }

}
