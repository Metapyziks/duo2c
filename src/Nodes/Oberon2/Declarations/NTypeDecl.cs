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
    public class NTypeDecl : DeclarationStatement
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
            scope.DeclareType(Identifier, Type.Type, Visibility);
        }

        public override IEnumerable<CompilerException> FindTypeErrors(Scope scope)
        {
            bool found = false;
            foreach (var e in base.FindTypeErrors(scope)) {
                found = true;
                yield return e;
            }

            if (!found && Visibility != AccessModifier.Private) {
                foreach (var e in Type.FindAccessibilityErrors(scope)) {
                    yield return e;
                }
            }
        }
    }
}
