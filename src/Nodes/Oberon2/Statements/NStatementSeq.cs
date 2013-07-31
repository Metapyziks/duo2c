using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DUO2C.Semantics;

namespace DUO2C.Nodes.Oberon2
{
    [SubstituteToken("StatementSeq")]
    public class NStatementSeq : SubstituteNode, ITypeErrorSource, IDeclarationSource
    {
        public IEnumerable<NStatement> Statements
        {
            get { return Children.Select(x => (NStatement)x ); }
        }

        public NStatementSeq(ParseNode original)
            : base(original, false)
        {
            Children = Children.Where(x => x is NStatement);
        }

        public void FindDeclarations(Scope scope)
        {
            foreach (var stmnt in Statements) {
                stmnt.FindDeclarations(scope);
            }
        }

        public IEnumerable<CompilerException> FindTypeErrors(Scope scope)
        {
            return Statements.SelectMany(x => x.FindTypeErrors(scope));
        }
    }
}
