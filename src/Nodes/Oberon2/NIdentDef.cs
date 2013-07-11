using System;
using System.Linq;

namespace DUO2C.Nodes.Oberon2
{
    public enum AccessModifier : byte
    {
        Private = 0,
        ReadOnly = 1,
        Public = 2
    }

    /// <summary>
    /// Substitution node for identifier definitions.
    /// </summary>
    [SubstituteToken("IdentDef")]
    public class NIdentDef : SubstituteNode
    {
        /// <summary>
        /// The identifier without an access modifier.
        /// </summary>
        [Serialize("access")]
        public AccessModifier AccessModifier { get; private set; }

        /// <summary>
        /// The identifier without an access modifier.
        /// </summary>
        public String Identifier { get; private set; }
        
        public override string String
        {
            get {
                return Identifier;
            }
        }

        /// <summary>
        /// Constructor to create a new identifier definition substitution node.
        /// </summary>
        /// <param name="original">The original parse node to be substituted</param>
        public NIdentDef(ParseNode original)
            : base(original, true)
        {
            Identifier = Children.First().String;
            if (Children.Count() > 1) {
                if (Children.Last().String == "*") {
                    AccessModifier = AccessModifier.Public;
                } else {
                    AccessModifier = AccessModifier.ReadOnly;
                }
            } else {
                AccessModifier = AccessModifier.Private;
            }
        }
    }
}
