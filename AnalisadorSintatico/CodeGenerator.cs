using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnalisadorSintatico.Arvore;

namespace AnalisadorSintatico
{
    public class CodeGenerator
    {
        private int tempCount = 1;
        private int labelCount = 1;

        private StringBuilder _code = new StringBuilder("");

        public void GenerateCode(ASTNode node)
        {
            node.GenerateCode(this);
        }

        public string GenerateTemp()
        {
            _code.AppendLine($"T{tempCount++}");
            return $"T{tempCount++}";
        }

        public string GenerateLabel()
        {
            _code.AppendLine($"LABEL{labelCount++}");
            return $"LABEL{labelCount++}";
        }

        public void Assign(string variable, string value)
        {
            _code.AppendLine($"{variable} = {value};");
            Console.WriteLine($"{variable} = {value};");
        }

        public void Addition(string result, string left, string right)
        {
            _code.AppendLine($"{result} = {left} + {right};");
            Console.WriteLine($"{result} = {left} + {right};");
        }

        public void Subtraction(string result, string left, string right)
        {
            _code.AppendLine($"{result} = {left} - {right};");
            Console.WriteLine($"{result} = {left} - {right};");
        }

        public void Multiplication(string result, string left, string right)
        {
            _code.AppendLine($"{result} = {left} * {right};");
            Console.WriteLine($"{result} = {left} * {right};");
        }

        public void Division(string result, string left, string right)
        {
            _code.AppendLine($"{result} = {left} / {right};");
            Console.WriteLine($"{result} = {left} / {right};");
        }

        public void Exponentiation(string result, string left, string right)
        {
            _code.AppendLine($"{result} = {left} ^ {right};");
            Console.WriteLine($"{result} = {left} ^ {right};");
        }

        public void Equal(string result, string left, string right)
        {
            _code.AppendLine($"{result} = {left} == {right};");
            Console.WriteLine($"{result} = {left} == {right};");
        }

        public void Less(string result, string left, string right)
        {
            _code.AppendLine($"{result} = {left} < {right};");
            Console.WriteLine($"{result} = {left} < {right};");
        }

        public void Greater(string result, string left, string right)
        {
            _code.AppendLine($"{result} = {left} > {right};");
            Console.WriteLine($"{result} = {left} > {right};");
        }

        public void EmitIfFalse(string conditionTemp, string falseLabel)
        {
            _code.AppendLine($"if {conditionTemp} == false goto {falseLabel};");
            Console.WriteLine($"if {conditionTemp} == false goto {falseLabel};");
        }

        public void EmitGoto(string label)
        {
            _code.AppendLine($"goto {label};");
            Console.WriteLine($"goto {label};");
        }

        public void EmitLabel(string label)
        {
            _code.AppendLine($"{label}:");
            Console.WriteLine($"{label}:");
        }
    }


}
