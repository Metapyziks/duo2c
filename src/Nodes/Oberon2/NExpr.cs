using System;
using System.Linq;

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
    public class NExpr : SubstituteNode
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
            }
        }
    }
}
