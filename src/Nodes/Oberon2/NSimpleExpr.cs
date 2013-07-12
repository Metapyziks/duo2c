using System;
using System.Collections.Generic;
using System.Linq;

using DUO2C.Semantics;

namespace DUO2C.Nodes.Oberon2
{
    public enum SimpleExprOperator : byte
    {
        None,
        Add,
        Subtract,
        Or
    }

    [SubstituteToken("SimpleExpr")]
    public class NSimpleExpr : ExpressionElement
    {
        [Serialize("operator", SimpleExprOperator.None)]
        public SimpleExprOperator Operator { get; private set; }

        public NTerm Term
        {
            get { return (NTerm) Children.First(); }
        }

        public NSimpleExpr Next
        {
            get { return Children.Count() == 1 ? null : (NSimpleExpr) Children.Last(); }
        }

        public override OberonType FinalType
        {
            get
            {
                if (Next == null) {
                    return Term.FinalType;
                } else {
                    if (Operator == SimpleExprOperator.Or) {
                        return BooleanType.Default;
                    } else if (Term.FinalType is SetType) {
                        return SetType.Default;
                    } else {
                        return NumericType.Largest((NumericType) Term.FinalType, (NumericType) Next.FinalType);
                    }
                }
            }
        }

        public override bool IsConstant
        {
            get { return Term.IsConstant && (Next == null || Next.IsConstant); }
        }

        public NSimpleExpr(ParseNode original)
            : base(original, false)
        {
            if (Children.Count() == 1) {
                Operator = SimpleExprOperator.None;
            } else {
                switch (Children.ElementAt(1).String) {
                    case "+":
                        Operator = SimpleExprOperator.Add; break;
                    case "-":
                        Operator = SimpleExprOperator.Subtract; break;
                    case "OR":
                        Operator = SimpleExprOperator.Or; break;
                    default:
                        Operator = SimpleExprOperator.None; break;
                }

                Children = new ParseNode[] { Term, Next };
            }
        }

        public override IEnumerable<ParserException> CheckTypes()
        {
            bool innerFound = false;

            foreach (var e in Term.CheckTypes()) {
                innerFound = true;
                yield return e;
            }

            if (Next != null) {
                foreach (var e in Next.CheckTypes()) {
                    innerFound = true;
                    yield return e;
                }
            }

            if (!innerFound && Next != null) {
                var left = Term.FinalType;
                var right = Next.FinalType;

                if (Operator == SimpleExprOperator.Or) {
                    if (!(left is BooleanType)) {
                        yield return new TypeMismatchException(BooleanType.Default, Term);
                    } else if (!(right is BooleanType)) {
                        yield return new TypeMismatchException(BooleanType.Default, Next);
                    }
                } else if (!(left is SetType) || !(right is SetType)) {
                    if (!(left is NumericType)) {
                        yield return new TypeMismatchException(NumericType.Default, Term);
                    } else if (!(right is NumericType)) {
                        yield return new TypeMismatchException(NumericType.Default, Next);
                    }
                }
            }
        }
    }
}
