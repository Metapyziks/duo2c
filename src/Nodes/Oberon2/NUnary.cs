using System;
using System.Linq;

namespace DUO2C.Nodes.Oberon2
{
    public enum UnaryOperator : byte
    {
        Identity,
        Negation,
        Not
    }

    [SubstituteToken("Unary")]
    public class NUnary : SubstituteNode
    {
        [Serialize("operator")]
        public UnaryOperator Operator { get; private set; }

        public NFactor Factor
        {
            get { return (NFactor) Children.First(); }
        }

        public NUnary(ParseNode original)
            : base(original, false)
        {
            switch (Children.First().String) {
                case "+":
                    Operator = UnaryOperator.Identity; break;
                case "-":
                    Operator = UnaryOperator.Negation; break;
                case "~":
                    Operator = UnaryOperator.Not; break;
            }
            Children = new ParseNode[] { Children.Last() };
        }
    }
}
