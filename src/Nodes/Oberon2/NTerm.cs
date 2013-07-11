using System;
using System.Linq;

using DUO2C.Semantics;

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
    public class NTerm : ExpressionElement
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

        public override OberonType FinalType
        {
            get
            {
                if (Next == null) {
                    return Factor.FinalType;
                } else {

                }
            }
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

                if (Operator == TermOperator.IntDivide || Operator == TermOperator.Modulo) {
                    if (!(Factor.FinalType is IntegerType)) {
                        throw new TypeMismatchException(IntegerType.Default, Factor);
                    } else if (!(Next.FinalType is IntegerType)) {
                        throw new TypeMismatchException(IntegerType.Default, Next);
                    }
                } else if (Operator == TermOperator.And) {
                    if (!(Factor.FinalType is BooleanType)) {
                        throw new TypeMismatchException(BooleanType.Default, Factor);
                    } else if (!(Next.FinalType is BooleanType)) {
                        throw new TypeMismatchException(BooleanType.Default, Next);
                    }
                } else {
                    if (!(Factor.FinalType is NumericType)) {
                        throw new TypeMismatchException(NumericType.Default, Factor);
                    } else if (!(Next.FinalType is NumericType)) {
                        throw new TypeMismatchException(NumericType.Default, Next);
                    }
                }
            }
        }
    }
}
