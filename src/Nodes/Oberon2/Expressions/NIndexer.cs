using System;
using System.Linq;

namespace DUO2C.Nodes.Oberon2
{
    /// <summary>
    /// Substitution node for indexers.
    /// </summary>
    [SubstituteToken("Indexer")]
    class NIndexer : Selector
    {
        public NIndexer(ParseNode original)
            : base(original, false)
        {
            Children = Children.Where(x => x.Token == "ExprList");
        }
    }
}
