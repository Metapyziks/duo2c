using System;
using System.Linq;

using DUO2C.Semantics;

namespace DUO2C.Nodes.Oberon2
{
    public enum ExprOperator : byte
    {
        None,
        Equals,
        NotEquals,
        LessThan,
        LessThanOrEqual,
        GreaterThan,
        GreaterThanOrEqual,
        InSet,
        IsType
    }

    [SubstituteToken("Expr")]
    public class NExpr : ExpressionElement
    {
        [Serialize("operator", ExprOperator.None)]
        public ExprOperator Operator { get; private set; }

        public NSimpleExpr SimpleExpr
        {
            get { return (NSimpleExpr) Children.First(); }
        }

        public NExpr Next
        {
            get { return Children.Count() == 1 ? null : (NExpr) Children.Last(); }
        }

        public override OberonType FinalType
        {
            get { return Next == null ? SimpleExpr.FinalType : BooleanType.Default; }
        }

        public override bool IsConstant
        {
            get { return SimpleExpr.IsConstant && (Next == null || Next.IsConstant); }
        }

        public NExpr(ParseNode original)
            : base(original, false)
        {
            if (Children.Count() == 1) {
                Operator = ExprOperator.None;
            } else {
                switch (Children.ElementAt(1).String) {
                    case "=":
                        Operator = ExprOperator.Equals; break;
                    case "#":
                        Operator = ExprOperator.NotEquals; break;
                    case "<":
                        Operator = ExprOperator.LessThan; break;
                    case "<=":
                        Operator = ExprOperator.LessThanOrEqual; break;
                    case ">":
                        Operator = ExprOperator.GreaterThan; break;
                    case ">=":
                        Operator = ExprOperator.GreaterThanOrEqual; break;
                    case "IN":
                        Operator = ExprOperator.InSet; break;
                    case "IS":
                        Operator = ExprOperator.IsType; break;
                    default:
                        Operator = ExprOperator.None; break;
                }
                Children = new ParseNode[] { SimpleExpr, Next };

                var left = SimpleExpr.FinalType;
                var right = Next.FinalType;

                if (Operator == ExprOperator.InSet) {
                    if (!(left is IntegerType)) {
                        throw new TypeMismatchException(IntegerType.Default, SimpleExpr);
                    } else if (!(right is SetType)) {
                        throw new TypeMismatchException(SetType.Default, Next);
                    }
                } else if (Operator == ExprOperator.Equals || Operator == ExprOperator.NotEquals) {
                    if (!left.CanTestEquality(right)) {
                        throw new TypeMismatchException(left, Next);
                    }
                } else {
                    if (!left.CanCompare(right)) {
                        throw new TypeMismatchException(left, Next);
                    }
                }
            }
        }
    }
}
