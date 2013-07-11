using System;
using System.Linq;

namespace DUO2C.Nodes.Oberon2
{
    public enum TermOperator : byte
    {
        None,
        Multiply,
        Divide,
        IntDivide,
        Modulo,
        And
    }

    [SubstituteToken("Term")]
    public class NTerm : SubstituteNode
    {
        [Serialize("operator", TermOperator.None)]
        public TermOperator Operator { get; private set; }

        public NFactor Factor
        {
            get { return (NFactor) Children.First(); }
        }

        public NTerm Next
        {
            get { return Children.Count() == 1 ? null : (NTerm) Children.Last(); }
        }

        public NTerm(ParseNode original)
            : base(original, false)
        {
            if (Children.Count() == 1) {
                Operator = TermOperator.None;
            } else {
                switch (Children.ElementAt(1).String) {
                    case "*":
                        Operator = TermOperator.Multiply; break;
                    case "/":
                        Operator = TermOperator.Divide; break;
                    case "DIV":
                        Operator = TermOperator.IntDivide; break;
                    case "MOD":
                        Operator = TermOperator.Modulo; break;
                    case "&":
                        Operator = TermOperator.And; break;
                    default:
                        Operator = TermOperator.None; break;
                }
                Children = new ParseNode[] { Factor, Next };
            }
        }
    }
}
