using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUO2C.Nodes.Oberon2
{
    public class Declaration : SubstituteNode
    {
        public virtual NIdentDef IdentDef
        {
            get { return (NIdentDef) Children.First(); }
        }

        public String Identifier
        {
            get { return IdentDef.String; }
        }

        public AccessModifier Visibility
        {
            get { return IdentDef.Visibility; }
        }

        public Declaration(ParseNode original)
            : base(original, false) { }
    }
}
