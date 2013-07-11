using System;
using System.Globalization;

using System.Linq;

namespace DUO2C.Nodes.Oberon2
{
    /// <summary>
    /// Substitution node for numbers.
    /// </summary>
    [SubstituteToken("number")]
    public class NNumber : SubstituteNode
    {
        public SubstituteNode Inner { get; private set; }
        
        public override string String
        {
            get {
                return Inner.String;
            }
        }

        public NNumber(ParseNode original)
            : base(original, false)
        {
            Inner = (SubstituteNode) Children.First();
        }
    }
}
