using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnalisadorSintatico.Arvore;

namespace AnalisadorSintatico
{
    public class GeradorDeCodigo
    {
        private int tempCount = 0;
        private int labelCount = 0;

        private Dictionary<string, int> contagemLabels = new Dictionary<string, int>();

        private System.IO.StreamWriter outputStream;

        public GeradorDeCodigo(StreamWriter outputStream)
        {
            this.outputStream = outputStream;
        }
        public void Emit(string code)
        {
            outputStream.WriteLine(code);
        }

        public string GenerateTemp()
        {
            return $"t{tempCount++}";
        }

        public string GenerateLabel(string nomeLabel = null)
        {
            if (string.IsNullOrWhiteSpace(nomeLabel))
            {
                return $"L{labelCount++}";
            }
            else
            {
                if (contagemLabels.ContainsKey(nomeLabel))
                {
                    contagemLabels[nomeLabel]++;
                }
                else
                {
                    contagemLabels[nomeLabel] = 1;
                }
                return nomeLabel + contagemLabels[nomeLabel];
            }
        }

        public void EmitDeclaration(string type, string identifier)
        {
            Emit($"{type} {identifier};");
        }

        public void Assign(string identifier, string value)
        {
            Emit($"{identifier} = {value};");
        }
    }

    //public class CodeGenerator
    //{
    //    private int tempCount = 0;
    //    private int labelCount = 0;
    //    private List<string> code = new List<string>();

    //    public string GenerateTemp()
    //    {
    //        return $"T{tempCount++}";
    //    }

    //    public string GenerateLabel(string prefix)
    //    {
    //        return $"{prefix}{labelCount++}";
    //    }

    //    public void EmitLabel(string label)
    //    {
    //        code.Add($"{label}:");
    //    }

    //    public void EmitJump(string label)
    //    {
    //        code.Add($"GOTO {label}");
    //    }

    //    public void EmitConditionalJump(string condition, string label, bool jumpIfTrue)
    //    {
    //        if (jumpIfTrue)
    //        {
    //            code.Add($"IF {condition} GOTO {label}");
    //        }
    //        else
    //        {
    //            code.Add($"IF NOT {condition} GOTO {label}");
    //        }
    //    }

    //    public void Assign(string target, string value)
    //    {
    //        code.Add($"{target} = {value}");
    //    }

    //    public void Addition(string target, string left, string right)
    //    {
    //        code.Add($"{target} = {left} + {right}");
    //    }

    //    public void Subtraction(string target, string left, string right)
    //    {
    //        code.Add($"{target} = {left} - {right}");
    //    }

    //    public void Multiplication(string target, string left, string right)
    //    {
    //        code.Add($"{target} = {left} * {right}");
    //    }

    //    public void Division(string target, string left, string right)
    //    {
    //        code.Add($"{target} = {left} / {right}");
    //    }

    //    public void Exponentiation(string target, string left, string right)
    //    {
    //        code.Add($"{target} = {left} ^ {right}");
    //    }

    //    public void PrintCode()
    //    {
    //        foreach (var line in code)
    //        {
    //            Console.WriteLine(line);
    //        }
    //    }
    //}

    //public class CodeGenerator
    //{
    //    private int tempCount = 1;
    //    private int labelCount = 1;

    //    private StringBuilder _code = new StringBuilder("");

    //    public void GenerateCode(ASTNode node)
    //    {
    //        node.GenerateCode(this);
    //    }

    //    public string GenerateTemp()
    //    {
    //        _code.AppendLine($"T{tempCount++}");
    //        return $"T{tempCount++}";
    //    }

    //    public string GenerateLabel()
    //    {
    //        _code.AppendLine($"LABEL{labelCount++}");
    //        return $"LABEL{labelCount++}";
    //    }

    //    public void Assign(string variable, string value)
    //    {
    //        _code.AppendLine($"{variable} = {value};");
    //        Console.WriteLine($"{variable} = {value};");
    //    }

    //    public void Addition(string result, string left, string right)
    //    {
    //        _code.AppendLine($"{result} = {left} + {right};");
    //        Console.WriteLine($"{result} = {left} + {right};");
    //    }

    //    public void Subtraction(string result, string left, string right)
    //    {
    //        _code.AppendLine($"{result} = {left} - {right};");
    //        Console.WriteLine($"{result} = {left} - {right};");
    //    }

    //    public void Multiplication(string result, string left, string right)
    //    {
    //        _code.AppendLine($"{result} = {left} * {right};");
    //        Console.WriteLine($"{result} = {left} * {right};");
    //    }

    //    public void Division(string result, string left, string right)
    //    {
    //        _code.AppendLine($"{result} = {left} / {right};");
    //        Console.WriteLine($"{result} = {left} / {right};");
    //    }

    //    public void Exponentiation(string result, string left, string right)
    //    {
    //        _code.AppendLine($"{result} = {left} ^ {right};");
    //        Console.WriteLine($"{result} = {left} ^ {right};");
    //    }

    //    public void Equal(string result, string left, string right)
    //    {
    //        _code.AppendLine($"{result} = {left} == {right};");
    //        Console.WriteLine($"{result} = {left} == {right};");
    //    }

    //    public void Less(string result, string left, string right)
    //    {
    //        _code.AppendLine($"{result} = {left} < {right};");
    //        Console.WriteLine($"{result} = {left} < {right};");
    //    }

    //    public void Greater(string result, string left, string right)
    //    {
    //        _code.AppendLine($"{result} = {left} > {right};");
    //        Console.WriteLine($"{result} = {left} > {right};");
    //    }

    //    public void EmitIfFalse(string conditionTemp, string falseLabel)
    //    {
    //        _code.AppendLine($"if {conditionTemp} == false goto {falseLabel};");
    //        Console.WriteLine($"if {conditionTemp} == false goto {falseLabel};");
    //    }

    //    public void EmitGoto(string label)
    //    {
    //        _code.AppendLine($"goto {label};");
    //        Console.WriteLine($"goto {label};");
    //    }

    //    public void EmitLabel(string label)
    //    {
    //        _code.AppendLine($"{label}:");
    //        Console.WriteLine($"{label}:");
    //    }
    //}


}
