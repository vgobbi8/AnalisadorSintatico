using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalisadorSintatico.Arvore
{

    public abstract class ASTNode
    {
        public abstract void GenerateCode(CodeGenerator generator);
    }



}
