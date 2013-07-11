using System;

namespace DUO2C.Nodes
{
    /// <summary>
    /// Substitution node for strings.
    /// </summary>
    [SubstituteToken("string")]
    public class NString : SubstituteNode
    {
        private String _string;

        public override string String
        {
            get {
                return _string;
            }
        }

        /// <summary>
        /// Constructor to create a new string substitution
        /// node.
        /// </summary>
        /// <param name="original">The original parse node to be substituted</param>
        public NString(ParseNode original)
            : base(original)
        {
            _string = base.String.Substring(1, base.String.Length - 2);
        }
    }
}
