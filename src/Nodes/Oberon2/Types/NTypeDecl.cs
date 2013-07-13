using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUO2C.Nodes.Oberon2
{
    [SubstituteToken("TypeDecl")]
    public class NTypeDecl : SubstituteNode
    {
        public String Identifier
        {
            get {
                return Children.First().String;
            }
        }

        public AccessModifier Visibility
        {
            get {
                return ((NIdentDef) Children.First()).Visibility;
            }
        }

        public NTypeDecl(ParseNode original)
            : base(original, false)
        {
            Children = Children.Where(x => x is NIdentDef || x is NType);
        }
    }
}
