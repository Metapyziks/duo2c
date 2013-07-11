using System;
using System.Globalization;

using DUO2C.Semantics;

namespace DUO2C.Nodes.Oberon2
{
    /// <summary>
    /// Substitution node for integers.
    /// </summary>
    [SubstituteToken("integer")]
    public class NInteger : ExpressionElement
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
        public IntegerRange Type
        {
            get {
                if (sbyte.MinValue <= Value && Value <= sbyte.MaxValue) {
                    return IntegerRange.BYTE;
                } else if (short.MinValue <= Value && Value <= short.MaxValue) {
                    return IntegerRange.SHORTINT;
                } else if (int.MinValue <= Value && Value <= int.MaxValue) {
                    return IntegerRange.INTEGER;
                } else {
                    return IntegerRange.LONGINT;
                }
            }
        }

        /// <summary>
        /// Constructor to create a new integer substitution
        /// node.
        /// </summary>
        /// <param name="original">The original parse node to be substituted</param>
        public NInteger(ParseNode original)
            : base(original, true)
        {
            if (base.String.EndsWith("H")) {
                // Hex integer literals end with a "H"
                Value = int.Parse(base.String.Substring(0, base.String.Length - 1), NumberStyles.HexNumber);
            } else {
                Value = int.Parse(base.String);
            }
        }

        public override OberonType FinalType()
        {
            return new IntegerType(Type);
        }
    }
}
