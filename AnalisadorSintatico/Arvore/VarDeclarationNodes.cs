using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalisadorSintatico.Arvore
{
    public class VarDeclarationNode : ASTNode
    {
        public string Type { get; }
        public List<string> Variables { get; }

        public VarDeclarationNode(string type, List<string> variables)
        {
            Type = type;
            Variables = variables;
        }

        public override string GenerateCode(CodeGenerator generator)
        {
            // Declarações de variáveis não geram código de três endereços
            // Podemos registrar variáveis se necessário
            return "";

        }
    }

}
