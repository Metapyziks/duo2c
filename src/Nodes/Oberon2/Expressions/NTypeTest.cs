using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUO2C.Nodes.Oberon2
{
    [SubstituteToken("TypeTest")]
    public class NTypeTest : Selector
    {
        public NQualIdent TypeIdent { get { return (NQualIdent) Children.First(); } }

        public NTypeTest(ParseNode original)
            : base(original, true)
        {
            Children = Children.Where(x => x is NQualIdent);
        }
    }
}
