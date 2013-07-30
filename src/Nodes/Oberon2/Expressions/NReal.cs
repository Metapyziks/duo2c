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
                return Value.ToString("e");
            }
        }

        private OberonType _finalType;
        public override OberonType GetFinalType(Scope scope)
        {
            return _finalType;
        }

        public override bool IsConstant(Scope scope)
        {
            return true;
        }

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
                _finalType = RealType.Real;
            } else {
                double mantissa = double.Parse(base.String.Substring(0, expStart));
                double exponent = double.Parse(base.String.Substring(expStart + 1));
                Value = mantissa * Math.Pow(10.0, exponent);
                _finalType = base.String[expStart] == 'E' ? RealType.Real : RealType.LongReal;
            }
        }

        public override IEnumerable<ParserException> FindTypeErrors(Scope scope)
        {
            return EmptyExceptionArray;
        }
    }
}
