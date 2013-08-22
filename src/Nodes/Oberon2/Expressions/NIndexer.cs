using System;
using System.Collections.Generic;

using System.Linq;

namespace DUO2C.Nodes.Oberon2
{
    /// <summary>
    /// Substitution node for indexers.
    /// </summary>
    [SubstituteToken("Indexer")]
    class NIndexer : Selector
    {
        public IEnumerable<NExpr> Expressions
        {
            get { return ((NExprList) Children.First()).Expressions; }
        }

        public override string String
        {
            get {
                return String.Format("[{0}]", String.Join(", ", Expressions.Select(x => x.String)));
            }
        }

        public NIndexer(ParseNode original)
            : base(original, false)
        {
            Children = Children.Where(x => x.Token == "ExprList");
        }
    }
}
