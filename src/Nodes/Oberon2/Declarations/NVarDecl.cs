using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUO2C.Nodes.Oberon2
{
    [SubstituteToken("VarDecl")]
    public class NVarDecl : Declaration
    {
        public override NIdentDef IdentDef
        {
            get {
                return IdentList.IdentDefs.First();
            }
        }

        public NIdentList IdentList
        {
            get { return (NIdentList) Children.First();}
        }

        public bool IsSingle
        {
            get { return IdentList.Count() == 0; }
        }

        public NVarDecl(ParseNode original)
            : base(original)
        {
            Children = Children.Where(x => x is NIdentList || x is NType);
        }
    }
}
