using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DUO2C.CodeGen;
using DUO2C.Semantics;

namespace DUO2C.Nodes.Oberon2
{
    [SubstituteToken("TypeDecl")]
    public class NTypeDecl : Declaration
    {
        public NType Type
        {
            get { return (NType) Children.Last(); }
        }

        public NTypeDecl(ParseNode original)
            : base(original)
        {
            Children = Children.Where(x => x is NIdentDef || x is NType);
        }

        public override void FindDeclarations(Scope scope)
        {
            scope.Declare(Identifier, Type.Type);
        }

        public override void GenerateCode(GenerationContext ctx)
        {
            ctx.NewLine();
            ctx = ctx + "; " + Identifier + " = " + Type.String;
            ctx.NewLine();
            ctx = ctx + "%" + Identifier + " = type " + Type;
            ctx.NewLine();
        }
    }
}
