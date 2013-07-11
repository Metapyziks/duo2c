using System;

namespace DUO2C.Nodes.Oberon2
{
    /// <summary>
    /// An enumeration of all real number types in Oberon-2
    /// </summary>
    public enum RealType : byte
    {
        REAL = 4,
        LONGREAL = 8
    }

    /// <summary>
    /// Substitution node for real numbers.
    /// </summary>
    [SubstituteToken("real")]
    public class NReal : SubstituteNode
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

        [Serialize("type")]
        public RealType Type { get; private set; }

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
                Type = RealType.REAL;
            } else {
                double mantissa = double.Parse(base.String.Substring(0, expStart));
                double exponent = double.Parse(base.String.Substring(expStart + 1));
                Value = mantissa * Math.Pow(10.0, exponent);
                Type = base.String[expStart] == 'E' ? RealType.REAL : RealType.LONGREAL;
            }
        }
    }
}
