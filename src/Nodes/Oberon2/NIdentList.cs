using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUO2C.Nodes.Oberon2
{
    [SubstituteToken("IdentList")]
    public class NIdentList : SubstituteNode
    {
        public IEnumerable<NIdentDef> IdentDefs
        {
            get { return Children.Select(x => (NIdentDef) x); }
        }

        public NIdentList(ParseNode original)
            : base(original, false)
        {
            Children = Children.Where(x => x is NIdentDef);
        }
    }
}
