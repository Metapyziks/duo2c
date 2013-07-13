using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUO2C.Nodes.Oberon2
{
    /// <summary>
    /// Substitution node for type guards.
    /// </summary>
    [SubstituteToken("TypeGuard")]
    class NTypeGuard : Selector
    {
        public NQualIdent Type { get; private set; }

        public override string String
        {
            get { return Type.String; }
        }

        public NTypeGuard(ParseNode original)
            : base(original, false)
        {
            Children = Children.Where(x => x is NQualIdent);
            Type = (NQualIdent) Children.First();
        }
    }
}
