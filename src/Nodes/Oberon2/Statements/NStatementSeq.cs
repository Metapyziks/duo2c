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
        public NStatementSeq(ParseNode original)
            : base(original, false)
        {

        }

        public IEnumerable<ParserException> FindTypeErrors()
        {
            return Children.SelectMany(x => ((NStatement) x).FindTypeErrors());
        }
    }
}
