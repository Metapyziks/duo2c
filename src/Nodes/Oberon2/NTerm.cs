using System;
using System.Collections.Generic;
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

        public NTerm Prev
        {
            get { return Children.Count() == 1 ? null : (NTerm) Children.First(); }
        }

        public NFactor Factor
        {
            get { return (NFactor) Children.Last(); }
        }

        public override OberonType FinalType
        {
            get
            {
                if (Prev == null) {
                    return Factor.FinalType;
                } else {
                    if (Operator == TermOperator.And) {
                        return BooleanType.Default;
                    } else if (Factor.FinalType is SetType) {
                        return SetType.Default;
                    } else if (Operator == TermOperator.Divide) {
                        return NumericType.Largest(RealType.Default,
                            NumericType.Largest((NumericType) Factor.FinalType, (NumericType) Prev.FinalType));
                    } else {
                        return NumericType.Largest((NumericType) Factor.FinalType, (NumericType) Prev.FinalType);
                    }
                }
            }
        }

        public override bool IsConstant
        {
            get { return Factor.IsConstant && (Prev == null || Prev.IsConstant); }
        }

        public NTerm(ParseNode original)
            : base(original, false)
        {
            if (Children.Count() == 1) {
                Operator = TermOperator.None;
            } else {
                var op = Children.ElementAt(Children.Count() - 2);
                switch (op.String) {
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

                Children = new ParseNode[] {
                    new NTerm(new BranchNode(Children.Take(Children.Count() - 2), Token)),
                    Factor
                };
            }
        }

        public override IEnumerable<ParserException> CheckTypes()
        {
            bool innerFound = false;

            foreach (var e in Factor.CheckTypes()) {
            innerFound = true;
                yield return e;
            }

            if (Prev != null) {
                foreach (var e in Prev.CheckTypes()) {
                    innerFound = true;
                    yield return e;
                }
            }

            if (!innerFound && Prev != null) {
                var left = Factor.FinalType;
                var right = Prev.FinalType;

                if (Operator == TermOperator.IntDivide || Operator == TermOperator.Modulo) {
                    if (!(left is IntegerType)) {
                        yield return new TypeMismatchException(IntegerType.Default, Factor);
                    } else if (!(right is IntegerType)) {
                        yield return new TypeMismatchException(IntegerType.Default, Prev);
                    }
                } else if (Operator == TermOperator.And) {
                    if (!(left is BooleanType)) {
                        yield return new TypeMismatchException(BooleanType.Default, Factor);
                    } else if (!(right is BooleanType)) {
                        yield return new TypeMismatchException(BooleanType.Default, Prev);
                    }
                } else if (!(left is SetType) || !(right is SetType)) {
                    if (!(left is NumericType)) {
                        yield return new TypeMismatchException(NumericType.Default, Factor);
                    } else if (!(right is NumericType)) {
                        yield return new TypeMismatchException(NumericType.Default, Prev);
                    }
                }
            }
        }
    }
}
