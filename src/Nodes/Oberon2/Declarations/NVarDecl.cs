using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DUO2C.Semantics;

namespace DUO2C.Nodes.Oberon2
{
    [SubstituteToken("VarDecl")]
    public class NVarDecl : DeclarationStatement
    {
        public override NIdentDef IdentDef
        {
            get {
                return IdentList.IdentDefs.First();
            }
        }

        public NIdentList IdentList
        {
            get { return (NIdentList) Children.First(); }
        }

        public NType Type
        {
            get { return (NType) Children.Last(); }
        }

        public bool IsSingle
        {
            get { return IdentList.Count() == 0; }
        }

        public NVarDecl(ParseNode original)
            : base(original)
        {
            Children = Children.Where(x => x is NIdentList || x is NType);
        }

        public override void FindDeclarations(Scope scope)
        {
            foreach (var ident in IdentList.IdentDefs) {
                scope.DeclareSymbol(ident.Identifier, Type.Type, ident.Visibility, false);
            }
        }
    }
}
