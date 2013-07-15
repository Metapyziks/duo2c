using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUO2C.Nodes.Oberon2
{
    [SubstituteToken("Statement")]
    public class NStatement : SubstituteNode
    {
        public NStatement(ParseNode original)
            : base(original, false)
        {

        }
    }
}
