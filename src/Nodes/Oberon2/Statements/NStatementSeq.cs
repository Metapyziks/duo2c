using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUO2C.Nodes.Oberon2
{
    [SubstituteToken("StatementSeq")]
    public class NStatementSeq : SubstituteNode, ITypeErrorSource
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

        public IEnumerable<ParserException> FindTypeErrors()
        {
            return Statements.SelectMany(x => x.FindTypeErrors());
        }
    }
}
