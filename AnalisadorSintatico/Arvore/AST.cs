using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace AnalisadorSintatico.Arvore
{
    public abstract class ASTNode
    {
        public abstract string GenerateCode(CodeGenerator generator);
    }

    public class ProgramNode : ASTNode
    {
        public List<ASTNode> Declarations { get; } = new List<ASTNode>();
        public List<ASTNode> Statements { get; } = new List<ASTNode>();

        public override string GenerateCode(CodeGenerator generator)
        {
            foreach (var declaration in Declarations)
            {
                declaration.GenerateCode(generator);
            }
            foreach (var statement in Statements)
            {
                statement.GenerateCode(generator);
            }
            return "";
        }
    }

    public class VarDeclarationsNode : ASTNode
    {
        public List<VarDeclarationNode> Declarations { get; }

        public VarDeclarationsNode(List<VarDeclarationNode> declarations)
        {
            Declarations = declarations;
        }

        public override string GenerateCode(CodeGenerator generator)
        {
            foreach (var declaration in Declarations)
            {
                declaration.GenerateCode(generator);
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

        public override string GenerateCode(CodeGenerator generator)
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

        public override string GenerateCode(CodeGenerator generator)
        {
            var value = Expression.GenerateCode(generator);
            generator.Assign(Identifier, value);
            return "";
        }
    }

    public abstract class ExpressionNode : ASTNode
    {
        public override abstract string GenerateCode(CodeGenerator generator);
    }

    public class BinaryOperationNode : ExpressionNode
    {
        public ExpressionNode Left { get; }
        public TokenType Operator { get; }
        public ExpressionNode Right { get; }

        public BinaryOperationNode(ExpressionNode left, TokenType op, ExpressionNode right)
        {
            Left = left;
            Operator = op;
            Right = right;
        }

        public override string GenerateCode(CodeGenerator generator)
        {
            var leftTemp = Left.GenerateCode(generator);
            var rightTemp = Right.GenerateCode(generator);
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

        public override string GenerateCode(CodeGenerator generator)
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

        public override string GenerateCode(CodeGenerator generator)
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

        public override string GenerateCode(CodeGenerator generator)
        {
            var startLabel = generator.GenerateLabel();
            var endLabel = generator.GenerateLabel();
            generator.Emit($"{startLabel}:");
            var conditionCode = Condition.GenerateCode(generator);
            generator.Emit($"if {conditionCode} == 0 goto {endLabel}");
            foreach (var statement in Body)
            {
                statement.GenerateCode(generator);
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

        public override string GenerateCode(CodeGenerator generator)
        {
            var elseLabel = generator.GenerateLabel();
            var endLabel = generator.GenerateLabel();
            var conditionCode = Condition.GenerateCode(generator);
            generator.Emit($"if {conditionCode} == 0 goto {elseLabel}");
            foreach (var statement in ThenBranch)
            {
                statement.GenerateCode(generator);
            }
            generator.Emit($"goto {endLabel}");
            generator.Emit($"{elseLabel}:");
            if (ElseBranch != null)
            {
                foreach (var statement in ElseBranch)
                {
                    statement.GenerateCode(generator);
                }
            }
            generator.Emit($"{endLabel}:");
            return "";
        }
    }

}
//namespace AnalisadorSintatico.Arvore
//{
//    public abstract class ASTNode
//    {
//        public abstract string GenerateCode(CodeGenerator generator);
//    }

//    public class ProgramNode : ASTNode
//    {
//        public List<ASTNode> Declarations { get; } = new List<ASTNode>();
//        public List<ASTNode> Statements { get; } = new List<ASTNode>();

//        public override string GenerateCode(CodeGenerator generator)
//        {
//            foreach (var declaration in Declarations)
//            {
//                declaration.GenerateCode(generator);
//            }
//            foreach (var statement in Statements)
//            {
//                statement.GenerateCode(generator);
//            }
//            return "";
//        }
//    }

//    public class VarDeclarationNode : ASTNode
//    {
//        public string Type { get; }
//        public List<string> Identifiers { get; }

//        public VarDeclarationNode(string type, List<string> identifiers)
//        {
//            Type = type;
//            Identifiers = identifiers;
//        }

//        public override string GenerateCode(CodeGenerator generator)
//        {
//            return "";
//        }
//    }

//    public class AssignmentNode : ASTNode
//    {
//        public string Identifier { get; }
//        public ExpressionNode Expression { get; }

//        public AssignmentNode(string identifier, ExpressionNode expression)
//        {
//            Identifier = identifier;
//            Expression = expression;
//        }

//        public override string GenerateCode(CodeGenerator generator)
//        {
//            var temp = Expression.GenerateCode(generator);
//            generator.Assign(Identifier, temp);
//            return "";
//        }
//    }

//    public abstract class ExpressionNode : ASTNode
//    {
//        public override abstract string GenerateCode(CodeGenerator generator);
//    }

//    public class LiteralNode : ExpressionNode
//    {
//        public string Value { get; }

//        public LiteralNode(string value)
//        {
//            Value = value;
//        }

//        public override string GenerateCode(CodeGenerator generator)
//        {
//            return Value;
//        }

//    }

//    public class VariableNode : ExpressionNode
//    {
//        public string Name { get; }

//        public VariableNode(string name)
//        {
//            Name = name;
//        }

//        public override string GenerateCode(CodeGenerator generator)
//        {
//            return Name;
//        }
//    }

//    public class BinaryOperationNode : ExpressionNode
//    {
//        public ExpressionNode Left { get; }
//        public string Operator { get; }
//        public ExpressionNode Right { get; }

//        public BinaryOperationNode(ExpressionNode left, string @operator, ExpressionNode right)
//        {
//            Left = left;
//            Operator = @operator;
//            Right = right;
//        }

//        public override string GenerateCode(CodeGenerator generator)
//        {
//            var leftTemp = Left.GenerateCode(generator);
//            var rightTemp = Right.GenerateCode(generator);
//            var temp = generator.GenerateTemp();
//            generator.EmitLabel.Emit($"{temp} = {leftTemp} {OperatorToString(Operator)} {rightTemp}");
//            return temp;
//            switch (Operator)
//            {
//                case "+":
//                    generator.Addition(temp, leftTemp, rightTemp);
//                    break;
//                case "-":
//                    generator.Subtraction(temp, leftTemp, rightTemp);
//                    break;
//                case "*":
//                    generator.Multiplication(temp, leftTemp, rightTemp);
//                    break;
//                case "/":
//                    generator.Division(temp, leftTemp, rightTemp);
//                    break;
//                case "^":
//                    generator.Exponentiation(temp, leftTemp, rightTemp);
//                    break;
//                default:
//                    throw new Exception($"Unknown operator {Operator}");
//            }

//            return temp;
//        }
//        private string OperatorToString(TokenType op)
//        {
//            return op switch
//            {
//                TokenType.Plus => "+",
//                TokenType.Minus => "-",
//                TokenType.Multiply => "*",
//                TokenType.Divide => "/",
//                TokenType.Power => "^",
//                TokenType.Less => "<",
//                TokenType.Greater => ">",
//                TokenType.LessEqual => "<=",
//                TokenType.GreaterEqual => ">=",
//                TokenType.Equal => "==",
//                TokenType.NotEqual => "!=",
//                _ => throw new Exception($"Unknown operator {op}")
//            };
//        }

//    }
//    public class WhileNode : ASTNode
//    {
//        public ExpressionNode Condition { get; }
//        public List<ASTNode> Body { get; }

//        public WhileNode(ExpressionNode condition, List<ASTNode> body)
//        {
//            Condition = condition;
//            Body = body;
//        }

//        public override string GenerateCode(CodeGenerator generator)
//        {
//            string startLabel = generator.GenerateLabel("WHILE");
//            string endLabel = generator.GenerateLabel("END");

//            generator.EmitLabel(startLabel);
//            var conditionTemp = Condition.GenerateCode(generator);
//            generator.EmitConditionalJump(conditionTemp, endLabel, false);

//            foreach (var statement in Body)
//            {
//                statement.GenerateCode(generator);
//            }

//            generator.EmitJump(startLabel);
//            generator.EmitLabel(endLabel);
//            return "";
//        }
//    }
//    public class IfNode : ASTNode
//    {
//        public ExpressionNode Condition { get; }
//        public List<ASTNode> ThenBranch { get; }
//        public List<ASTNode> ElseBranch { get; }

//        public IfNode(ExpressionNode condition, List<ASTNode> thenBranch, List<ASTNode> elseBranch)
//        {
//            Condition = condition;
//            ThenBranch = thenBranch;
//            ElseBranch = elseBranch;
//        }

//        public override string GenerateCode(CodeGenerator generator)
//        {
//            var elseLabel = generator.GenerateLabel("ELSE");
//            var endLabel = generator.GenerateLabel("END");

//            var conditionTemp = Condition.GenerateCode(generator);
//            generator.EmitConditionalJump(conditionTemp, elseLabel, false);

//            foreach (var statement in ThenBranch)
//            {
//                statement.GenerateCode(generator);
//            }

//            generator.EmitJump(endLabel);
//            generator.EmitLabel(elseLabel);

//            if (ElseBranch != null)
//            {
//                foreach (var statement in ElseBranch)
//                {
//                    statement.GenerateCode(generator);
//                }
//            }

//            generator.EmitLabel(endLabel);
//            return "";
//        }
//    }

//}
