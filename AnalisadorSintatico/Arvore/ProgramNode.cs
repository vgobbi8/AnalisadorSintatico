using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalisadorSintatico.Arvore
{
    public class ProgramNode : ASTNode
    {
        public List<ASTNode> Declarations { get; } = new List<ASTNode>();
        public List<ASTNode> Statements { get; } = new List<ASTNode>();

        public override void GenerateCode(CodeGenerator generator)
        {
            foreach (var decl in Declarations)
            {
                decl.GenerateCode(generator);
            }
            foreach (var stmt in Statements)
            {
                stmt.GenerateCode(generator);
            }
        }
    }
}
