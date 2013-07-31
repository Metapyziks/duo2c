using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DUO2C.Semantics;

namespace DUO2C.Nodes.Oberon2
{
    [SubstituteToken("Type")]
    public class NType : TypeDefinition, ITypeErrorSource, IAccessibilityErrorSource
    {
        public TypeDefinition Inner
        {
            get { return (TypeDefinition) Children.First(); }
        }

        public override OberonType Type
        {
            get { return Inner.Type; }
        }

        public NType(ParseNode original)
            : base(original, false) { }


        public override IEnumerable<CompilerException> FindTypeErrors(Scope scope)
        {
            if (Inner is ITypeErrorSource) {
                return ((ITypeErrorSource) Inner).FindTypeErrors(scope);
            } else {
                return new CompilerException[0];
            }
        }

        public override IEnumerable<CompilerException> FindAccessibilityErrors(Scope scope)
        {
            return Inner.FindAccessibilityErrors(scope);
        }
    }
}
