using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DUO2C.Semantics;

namespace DUO2C.Nodes.Oberon2
{
    public class Declaration : SubstituteNode, ITypeErrorSource
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

        public IEnumerable<ParserException> FindTypeErrors()
        {
            return Children.SelectMany(x => (x is ITypeErrorSource)
                ? ((ITypeErrorSource) x).FindTypeErrors()
                : new ParserException[0]);
        }
    }
}
