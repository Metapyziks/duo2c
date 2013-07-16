using System;
using System.Collections.Generic;
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
        String _opString;

        [Serialize("operator", ExprOperator.None)]
        public ExprOperator Operator { get; private set; }

        public NExpr Prev
        {
            get { return Children.Count() == 1 ? null : (NExpr) Children.First(); }
        }

        public NSimpleExpr SimpleExpr
        {
            get { return (NSimpleExpr) Children.Last(); }
        }

        public override OberonType FinalType
        {
            get { return Prev == null ? SimpleExpr.FinalType : BooleanType.Default; }
        }

        public override bool IsConstant
        {
            get { return SimpleExpr.IsConstant && (Prev == null || Prev.IsConstant); }
        }

        public NExpr(ParseNode original)
            : base(original, false)
        {
            if (Children.Count() == 1) {
                Operator = ExprOperator.None;
            } else {
                _opString = Children.ElementAt(1).String;
                switch (_opString) {
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

                Children = new ParseNode[] {
                    new NExpr(new BranchNode(Children.Take(Children.Count() - 2), Token)),
                    SimpleExpr
                };
            }
        }

        public override IEnumerable<ParserException> FindTypeErrors()
        {
            bool innerFound = false;

            foreach (var e in SimpleExpr.FindTypeErrors()) {
                innerFound = true;
                yield return e;
            }

            if (Prev != null) {
                foreach (var e in Prev.FindTypeErrors()) {
                    innerFound = true;
                    yield return e;
                }
            }

            if (!innerFound && Prev != null) {
                var left = SimpleExpr.FinalType;
                var right = Prev.FinalType;

                if (Operator == ExprOperator.InSet) {
                    if (!(left is IntegerType)) {
                        yield return new OperandTypeException(left, right, _opString, this);
                    } else if (!(right is SetType)) {
                        yield return new OperandTypeException(left, right, _opString, this);
                    }
                } else if (Operator == ExprOperator.Equals || Operator == ExprOperator.NotEquals) {
                    if (!left.CanTestEquality(right)) {
                        yield return new OperandTypeException(left, right, _opString, this);
                    }
                } else {
                    if (!left.CanCompare(right)) {
                        yield return new OperandTypeException(left, right, _opString, this);
                    }
                }
            }
        }

        public override string SerializeXML()
        {
            // TEMPORARY HACK
            var exceptions = FindTypeErrors();
            if (exceptions.Count() > 0) {
                throw exceptions.First();
            }

            return base.SerializeXML();
        }
    }
}
