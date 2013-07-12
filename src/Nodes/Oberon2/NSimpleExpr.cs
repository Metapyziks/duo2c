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

        public NSimpleExpr Prev
        {
            get { return Children.Count() == 1 ? null : (NSimpleExpr) Children.First(); }
        }

        public NTerm Term
        {
            get { return (NTerm) Children.Last(); }
        }

        public override OberonType FinalType
        {
            get
            {
                if (Prev == null) {
                    return Term.FinalType;
                } else {
                    if (Operator == SimpleExprOperator.Or) {
                        return BooleanType.Default;
                    } else if (Term.FinalType is SetType) {
                        return SetType.Default;
                    } else {
                        return NumericType.Largest((NumericType) Term.FinalType, (NumericType) Prev.FinalType);
                    }
                }
            }
        }

        public override bool IsConstant
        {
            get { return Term.IsConstant && (Prev == null || Prev.IsConstant); }
        }

        public NSimpleExpr(ParseNode original)
            : base(original, false)
        {
            if (Children.Count() == 1) {
                Operator = SimpleExprOperator.None;
            } else {
                var op = Children.ElementAt(Children.Count() - 2);

                switch (op.String) {
                    case "+":
                        Operator = SimpleExprOperator.Add; break;
                    case "-":
                        Operator = SimpleExprOperator.Subtract; break;
                    case "OR":
                        Operator = SimpleExprOperator.Or; break;
                    default:
                        Operator = SimpleExprOperator.None; break;
                }

                Children = new ParseNode[] {
                    new NSimpleExpr(new BranchNode(Children.Take(Children.Count() - 2), Token)),
                    Term
                };
            }
        }

        public override IEnumerable<ParserException> CheckTypes()
        {
            bool innerFound = false;

            foreach (var e in Term.CheckTypes()) {
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
                var left = Term.FinalType;
                var right = Prev.FinalType;

                if (Operator == SimpleExprOperator.Or) {
                    if (!(left is BooleanType)) {
                        yield return new TypeMismatchException(BooleanType.Default, Term);
                    } else if (!(right is BooleanType)) {
                        yield return new TypeMismatchException(BooleanType.Default, Prev);
                    }
                } else if (!(left is SetType) || !(right is SetType)) {
                    if (!(left is NumericType)) {
                        yield return new TypeMismatchException(NumericType.Default, Term);
                    } else if (!(right is NumericType)) {
                        yield return new TypeMismatchException(NumericType.Default, Prev);
                    }
                }
            }
        }
    }
}
