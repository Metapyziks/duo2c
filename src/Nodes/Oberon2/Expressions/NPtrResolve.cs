using System;
using System.Linq;

namespace DUO2C.Nodes.Oberon2
{
    /// <summary>
    /// Substitution node for pointer resolving.
    /// </summary>
    [SubstituteToken("PtrResolve")]
    class NPtrResolve : Selector
    {
        public override string String
        {
            get { return "^"; }
        }

        public NPtrResolve(ParseNode original)
            : base(original, true, false)
        {
            Children = new ParseNode[0];
        }
    }
}
