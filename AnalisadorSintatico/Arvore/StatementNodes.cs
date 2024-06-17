using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalisadorSintatico.Arvore
{
    public abstract class StatementNode : ASTNode
    {
    }

    public class AssignmentNode : StatementNode
    {
        public string Variable { get; }
        public ExpressionNode Expression { get; }

        public AssignmentNode(string variable, ExpressionNode expression)
        {
            Variable = variable;
            Expression = expression;
        }

        public override string GenerateCode(CodeGenerator generator)
        {
            string expressionTemp = Expression.GenerateCode(generator);
            generator.Assign(Variable, expressionTemp);
            return expressionTemp;
        }
    }

    public class WhileNode : StatementNode
    {
        public ExpressionNode Condition { get; }
        public List<StatementNode> Body { get; }

        public WhileNode(ExpressionNode condition, List<StatementNode> body)
        {
            Condition = condition;
            Body = body;
        }

        public override string GenerateCode(CodeGenerator generator)
        {
            string whileLabel = generator.GenerateLabel();
            string endLabel = generator.GenerateLabel();

            generator.EmitLabel(whileLabel);
            string conditionTemp = Condition.GenerateCode(generator);
            generator.EmitIfFalse(conditionTemp, endLabel);

            foreach (var statement in Body)
            {
                statement.GenerateCode(generator);
            }

            generator.EmitGoto(whileLabel);
            generator.EmitLabel(endLabel);
            return "";
        }
    }

    public class IfNode : StatementNode
    {
        public ExpressionNode Condition { get; }
        public List<StatementNode> TrueBranch { get; }
        public List<StatementNode> FalseBranch { get; }

        public IfNode(ExpressionNode condition, List<StatementNode> trueBranch, List<StatementNode> falseBranch)
        {
            Condition = condition;
            TrueBranch = trueBranch;
            FalseBranch = falseBranch;
        }

        public override string GenerateCode(CodeGenerator generator)
        {
            string falseLabel = generator.GenerateLabel();
            string endLabel = generator.GenerateLabel();

            string conditionTemp = Condition.GenerateCode(generator);
            generator.EmitIfFalse(conditionTemp, falseLabel);

            foreach (var statement in TrueBranch)
            {
                statement.GenerateCode(generator);
            }

            generator.EmitGoto(endLabel);
            generator.EmitLabel(falseLabel);

            foreach (var statement in FalseBranch)
            {
                statement.GenerateCode(generator);
            }

            generator.EmitLabel(endLabel);
            return "";

        }
    }

}
