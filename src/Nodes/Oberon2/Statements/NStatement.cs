using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DUO2C.Semantics;

namespace DUO2C.Nodes.Oberon2
{
    [SubstituteToken("Statement")]
    public class NStatement : SubstituteNode, ITypeErrorSource, IDeclarationSource
    {
        public Statement Inner {
            get { return (Statement) Children.First(); }
        }

        public NStatement(ParseNode original)
            : base(original, false)
        {
            Children = Children.Where(x => x is Statement);
        }

        public void FindDeclarations(Scope scope)
        {
            if (Inner is IDeclarationSource) {
                ((IDeclarationSource) Inner).FindDeclarations(scope);
            }
        }

        public IEnumerable<ParserException> FindTypeErrors(Scope scope)
        {
            return Inner.FindTypeErrors(scope);
        }
    }
}
