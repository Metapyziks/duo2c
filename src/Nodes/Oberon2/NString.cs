using System;

using DUO2C.Semantics;

namespace DUO2C.Nodes.Oberon2
{
    /// <summary>
    /// Substitution node for strings.
    /// </summary>
    [SubstituteToken("string")]
    public class NString : ExpressionElement
    {
        private String _string;

        public override string String
        {
            get {
                return _string;
            }
        }

        public override OberonType FinalType
        {
            get { return String.Length == 1 ? (OberonType) CharType.Default : new ArrayType(CharType.Default, String.Length); }
        }

        public override bool IsConstant
        {
            get { return true; }
        }

        /// <summary>
        /// Constructor to create a new string substitution
        /// node.
        /// </summary>
        /// <param name="original">The original parse node to be substituted</param>
        public NString(ParseNode original)
            : base(original, true)
        {
            _string = base.String.Substring(1, base.String.Length - 2);
        }
    }
}
