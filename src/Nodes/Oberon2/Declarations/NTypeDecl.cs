using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUO2C.Nodes.Oberon2
{
    [SubstituteToken("TypeDecl")]
    public class NTypeDecl : Declaration
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
    }
}
