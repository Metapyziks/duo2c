using System;

namespace DUO2C.Nodes
{
    /// <summary>
    /// Substitution node for identifiers.
    /// </summary>
    [SubstituteToken("ident")]
    public class NIdent : SubstituteNode
    {
        private String _string;

        public override string String
        {
            get
            {
                return _string;
            }
        }

        /// <summary>
        /// Constructor to create a new identifier substitution
        /// node.
        /// </summary>
        /// <param name="original">The original parse node to be substituted</param>
        public NIdent(ParseNode original)
            : base(original)
        {
            _string = base.String.Trim();
        }
    }
}
