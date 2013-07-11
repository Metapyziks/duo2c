using System;
using System.Linq;

namespace DUO2C.Nodes.Oberon2
{
    /// <summary>
    /// Substitution node for qualified identifiers.
    /// </summary>
    [SubstituteToken("QualIdent")]
    public class NQualIdent : SubstituteNode
    {
        /// <summary>
        /// The module that contains this identifier.
        /// </summary>
        [Serialize("module")]
        public String Module { get; private set; }

        /// <summary>
        /// The identifier without a module prefix.
        /// </summary>
        public String Identifier { get; private set; }

        public override string String
        {
            get {
                return Identifier;
            }
        }

        /// <summary>
        /// Constructor to create a new qualified identifier substitution node.
        /// </summary>
        /// <param name="original">The original parse node to be substituted</param>
        public NQualIdent(ParseNode original)
            : base(original, true)
        {
            var children = Children.Where(x => x is NIdent);
            Children = new ParseNode[0];

            Identifier = children.Last().String;

            if (children.Count() == 2) {
                Module = children.First().String;
            } else {
                Module = null;
            }
        }
    }
}
