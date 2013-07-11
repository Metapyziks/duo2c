using System;
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
            get { throw new NotImplementedException(); }
        }

        public override bool IsConstant
        {
            get { throw new NotImplementedException(); }
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
    }
}
