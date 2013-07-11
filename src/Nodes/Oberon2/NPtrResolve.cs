using System;
using System.Linq;

namespace DUO2C.Nodes.Oberon2
{
    /// <summary>
    /// Substitution node for pointer resolving.
    /// </summary>
    [SubstituteToken("PtrResolve")]
    class NPtrResolve : DesignatorOperation
    {
        public NPtrResolve(ParseNode original)
            : base(original, true, false)
        {
            Children = new ParseNode[0];
        }
    }
}
