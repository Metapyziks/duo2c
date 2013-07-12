using System;
using System.Collections.Generic;
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

        public override OberonType FinalType
        {
            get { return new IntegerType(Range); }
        }

        public override bool IsConstant
        {
            get { return true; }
        }

        /// <summary>
        /// Finds the type of integer this literal represents
        /// based on the number of bytes required to store it.
        /// </summary>
        public IntegerRange Range
        {
            get {
                if (sbyte.MinValue <= Value && Value <= sbyte.MaxValue) {
                    return IntegerRange.Byte;
                } else if (short.MinValue <= Value && Value <= short.MaxValue) {
                    return IntegerRange.ShortInt;
                } else if (int.MinValue <= Value && Value <= int.MaxValue) {
                    return IntegerRange.Integer;
                } else {
                    return IntegerRange.LongInt;
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

        public override IEnumerable<ParserException> CheckTypes()
        {
            return EmptyExceptionArray;
        }
    }
}
