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
                    if (Operator == TermOperator.And) {
                        return BooleanType.Default;
                    } else if (Factor.FinalType is SetType) {
                        return Factor.FinalType;
                    } else if (Operator == TermOperator.Divide) {
                        return NumericType.Largest(RealType.Default,
                            NumericType.Largest((NumericType) Factor.FinalType, (NumericType) Next.FinalType));
                    } else {
                        return NumericType.Largest((NumericType) Factor.FinalType, (NumericType) Next.FinalType);
                    }
                }
            }
        }

        public override bool IsConstant
        {
            get { return Factor.IsConstant && (Next == null || Next.IsConstant); }
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

                var left = Factor.FinalType;
                var right = Next.FinalType;

                if (Operator == TermOperator.IntDivide || Operator == TermOperator.Modulo) {
                    if (!(left is IntegerType)) {
                        throw new TypeMismatchException(IntegerType.Default, Factor);
                    } else if (!(right is IntegerType)) {
                        throw new TypeMismatchException(IntegerType.Default, Next);
                    }
                } else if (Operator == TermOperator.And) {
                    if (!(left is BooleanType)) {
                        throw new TypeMismatchException(BooleanType.Default, Factor);
                    } else if (!(right is BooleanType)) {
                        throw new TypeMismatchException(BooleanType.Default, Next);
                    }
                } else if (!(left is SetType) || !(right is SetType)) {
                    if (!(left is NumericType)) {
                        throw new TypeMismatchException(NumericType.Default, Factor);
                    } else if (!(right is NumericType)) {
                        throw new TypeMismatchException(NumericType.Default, Next);
                    }
                }
            }
        }
    }
}
