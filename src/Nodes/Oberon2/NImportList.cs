using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUO2C.Nodes.Oberon2
{
    [SubstituteToken("ImportList")]
    public class NImportList : SubstituteNode
    {
        public IEnumerable<String> Modules
        {
            get { return Children.Select(x => x.String); }
        }

        public NImportList(ParseNode original)
            : base(original, false)
        {
            Children = Children.Where(x => x is NIdent);
        }
    }
}
