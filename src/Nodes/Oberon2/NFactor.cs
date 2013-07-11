using System;
using System.Linq;

namespace DUO2C.Nodes.Oberon2
{
    [SubstituteToken("Factor")]
    public class NFactor : SubstituteNode
    {
        public NFactor(ParseNode original)
            : base(original, false)
        {
            if (Children.Count() == 3) {
                Children = new ParseNode[] { Children.ElementAt(1) };
            }
        }
    }
}
