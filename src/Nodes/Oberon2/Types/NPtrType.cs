using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DUO2C.Semantics;

namespace DUO2C.Nodes.Oberon2
{
    [SubstituteToken("PtrType")]
    public class NPtrType : TypeDefinition
    {
        public override OberonType Type
        {
            get { return new PointerType(((NType) Children.First()).Type); }
        }

        public NPtrType(ParseNode original)
            : base(original, false)
        {
            Children = Children.Where(x => x is NType);
        }
    }
}
