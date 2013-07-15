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
        public NNamedType Type { get; private set; }

        public override string String
        {
            get { return Type.String; }
        }

        public NTypeGuard(ParseNode original)
            : base(original, false)
        {
            Children = Children.Where(x => x is NNamedType);
            Type = (NNamedType) Children.First();
        }
    }
}
