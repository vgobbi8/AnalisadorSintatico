using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalisadorSintatico.Arvore
{
    public abstract class ExpressionNode : ASTNode
    {
        public string Result { get; protected set; } // Armazena o resultado temporário da expressão

        public abstract override string GenerateCode(CodeGenerator generator);
    }

    public class BinaryExpressionNode : ExpressionNode
    {
        public ExpressionNode Left { get; }
        public TokenType Operator { get; }
        public ExpressionNode Right { get; }

        public BinaryExpressionNode(ExpressionNode left, TokenType @operator, ExpressionNode right)
        {
            Left = left;
            Operator = @operator;
            Right = right;
        }

        public override string GenerateCode(CodeGenerator generator)
        {
            string leftTemp = Left.GenerateCode(generator);
            string rightTemp = Right.GenerateCode(generator);
            Result = generator.GenerateTemp();

            switch (Operator)
            {
                case TokenType.Plus:
                    generator.Addition(Result, leftTemp, rightTemp);
                    break;
                case TokenType.Minus:
                    generator.Subtraction(Result, leftTemp, rightTemp);
                    break;
                case TokenType.Multiply:
                    generator.Multiplication(Result, leftTemp, rightTemp);
                    break;
                case TokenType.Divide:
                    generator.Division(Result, leftTemp, rightTemp);
                    break;
                case TokenType.Power:
                    generator.Exponentiation(Result, leftTemp, rightTemp);
                    break;
                case TokenType.Equal:
                    generator.Equal(Result, leftTemp, rightTemp);
                    break;
                case TokenType.Less:
                    generator.Less(Result, leftTemp, rightTemp);
                    break;
                case TokenType.Greater:
                    generator.Greater(Result, leftTemp, rightTemp);
                    break;
                // Adicionar outros operadores conforme necessário
                default:
                    throw new InvalidOperationException($"Unexpected operator: {Operator}");
            }

            return Result;
        }
    }

    public class LiteralNode : ExpressionNode
    {
        private readonly string value;

        public LiteralNode(string value)
        {
            this.value = value;
        }

        public override string GenerateCode(CodeGenerator generator)
        {
            Result = generator.GenerateTemp();
            generator.Assign(Result, value);
            return Result;
        }

        public override string ToString()
        {
            return value;
        }
    }

    public class VariableNode : ExpressionNode
    {
        private readonly string name;

        public VariableNode(string name)
        {
            this.name = name;
        }

        public override string GenerateCode(CodeGenerator generator)
        {
            Result = name; // Para variáveis, o resultado é o próprio nome da variável
            return Result;
        }

        public override string ToString()
        {
            return name;
        }
    }

}
