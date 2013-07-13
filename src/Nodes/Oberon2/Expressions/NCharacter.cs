using System.Collections.Generic;
using System.Globalization;

using DUO2C.Semantics;

namespace DUO2C.Nodes.Oberon2
{
    /// <summary>
    /// Substitution node for characters.
    /// </summary>
    [SubstituteToken("character")]
    public class NCharacter : ExpressionElement
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

        public override OberonType FinalType
        {
            get { return CharType.Default; }
        }

        public override bool IsConstant
        {
            get { return true; }
        }

        /// <summary>
        /// Constructor to create a new character substitution node.
        /// </summary>
        /// <param name="original">The original parse node to be substituted</param>
        public NCharacter(ParseNode original)
            : base(original, true)
        {
            Value = (char) ushort.Parse(base.String.Substring(0, base.String.Length - 1), NumberStyles.HexNumber);
        }

        public override IEnumerable<ParserException> FindTypeErrors()
        {
            return EmptyExceptionArray;
        }
    }
}
