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
    public class NInvocation : Selector
    {
        public override string String
        {
            get
            {
                return "(" + base.String + ")";
            }
        }

        public NExprList Args
        {
            get { return (NExprList) Children.FirstOrDefault(); }
        }

        public NInvocation(ParseNode original)
            : base(original, false)
        {
            Children = Children.Where(x => x is NExprList);
        }
    }
}
