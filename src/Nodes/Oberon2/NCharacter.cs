using System.Globalization;

namespace DUO2C.Nodes.Oberon2
{
    /// <summary>
    /// Substitution node for characters.
    /// </summary>
    [SubstituteToken("character")]
    public class NCharacter : SubstituteNode
    {
        /// <summary>
        /// The parsed value of the character.
        /// </summary>
        public char Value { get; private set; }

        public override string String
        {
            get {
                return ((ushort) Value).ToString("X4");
            }
        }

        /// <summary>
        /// Constructor to create a new character substitution node.
        /// </summary>
        /// <param name="original">The original parse node to be substituted</param>
        public NCharacter(ParseNode original)
            : base(original, false)
        {
            Value = (char) ushort.Parse(base.String.Substring(0, base.String.Length - 1), NumberStyles.HexNumber);
        }
    }
}
