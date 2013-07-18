using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUO2C.Nodes.Oberon2
{
    /// <summary>
    /// Substitution node for invocations / type guards.
    /// </summary>
    [SubstituteToken("Invocation")]
    class NInvocation : Selector
    {
        public NExprList Args
        {
            get { return (NExprList) Children.First(); }
        }

        public NInvocation(ParseNode original)
            : base(original, false)
        {
            Children = Children.Where(x => x is NExprList);
        }
    }
}
