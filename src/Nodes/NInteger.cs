using System;
using System.Globalization;

namespace DUO2C.Nodes
{
    /// <summary>
    /// An enumeration of all integer types in Oberon-2
    /// </summary>
    enum IntegerType : byte
    {
        BYTE = 1,
        SHORTINT = 2,
        INTEGER = 4,
        LONGINT = 8
    }

    /// <summary>
    /// Substitution node for integers.
    /// </summary>
    [SubstituteToken("integer")]
    class NInteger : SubstituteNode
    {
        /// <summary>
        /// The parsed value of the integer.
        /// </summary>
        public long Value { get; private set; }

        public override string String
        {
            get {
                return Value.ToString();
            }
        }

        /// <summary>
        /// Finds the type of integer this literal represents
        /// based on the number of bytes required to store it.
        /// </summary>
        [Serialize("type")]
        public IntegerType Type
        {
            get {
                if (sbyte.MinValue <= Value && Value <= sbyte.MaxValue) {
                    return IntegerType.BYTE;
                } else if (short.MinValue <= Value && Value <= short.MaxValue) {
                    return IntegerType.SHORTINT;
                } else if (int.MinValue <= Value && Value <= int.MaxValue) {
                    return IntegerType.INTEGER;
                } else {
                    return IntegerType.LONGINT;
                }
            }
        }

        /// <summary>
        /// Constructor to create a new integer substitution
        /// node.
        /// </summary>
        /// <param name="original">The original parse node to be substituted</param>
        public NInteger(ParseNode original)
            : base(original)
        {
            if (base.String.EndsWith("H")) {
                // Hex integer literals end with a "H"
                Value = int.Parse(base.String.Substring(0, base.String.Length - 1), NumberStyles.HexNumber);
            } else {
                Value = int.Parse(base.String);
            }
        }
    }
}
