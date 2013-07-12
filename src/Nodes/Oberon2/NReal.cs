using System;
using System.Collections.Generic;

using DUO2C.Semantics;

namespace DUO2C.Nodes.Oberon2
{
    /// <summary>
    /// Substitution node for real numbers.
    /// </summary>
    [SubstituteToken("real")]
    public class NReal : ExpressionElement
    {
        /// <summary>
        /// The parsed value of the real number.
        /// </summary>
        public double Value { get; private set; }
        
        public override string String
        {
            get {
                return Value.ToString();
            }
        }

        public override OberonType FinalType
        {
            get { return new RealType(Range); }
        }

        public override bool IsConstant
        {
            get { return true; }
        }

        public RealRange Range { get; private set; }

        /// <summary>
        /// Constructor to create a new real number substitution node.
        /// </summary>
        /// <param name="original">The original parse node to be substituted</param>
        public NReal(ParseNode original)
            : base(original, true)
        {
            int expStart = Math.Max(base.String.IndexOf('E'), base.String.IndexOf('D'));
            if (expStart == -1) {
                Value = double.Parse(base.String);
                Range = RealRange.Real;
            } else {
                double mantissa = double.Parse(base.String.Substring(0, expStart));
                double exponent = double.Parse(base.String.Substring(expStart + 1));
                Value = mantissa * Math.Pow(10.0, exponent);
                Range = base.String[expStart] == 'E' ? RealRange.Real : RealRange.LongReal;
            }
        }

        public override IEnumerable<ParserException> CheckTypes()
        {
            return EmptyExceptionArray;
        }
    }
}
