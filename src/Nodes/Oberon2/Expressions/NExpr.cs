﻿using System;
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

        public override OberonType GetFinalType(Scope scope)
        {
            return Prev == null ? SimpleExpr.GetFinalType(scope) : BooleanType.Default;
        }

        public override bool IsConstant(Scope scope)
        {
            return SimpleExpr.IsConstant(scope) && (Prev == null || Prev.IsConstant(scope));
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

        public override IEnumerable<ParserException> FindTypeErrors(Scope scope)
        {
            bool innerFound = false;

            foreach (var e in SimpleExpr.FindTypeErrors(scope)) {
                innerFound = true;
                yield return e;
            }

            if (Prev != null) {
                foreach (var e in Prev.FindTypeErrors(scope)) {
                    innerFound = true;
                    yield return e;
                }
            }

            if (!innerFound && Prev != null) {
                var left = Prev.GetFinalType(scope);
                var right = SimpleExpr.GetFinalType(scope);
                
                if (left == null) {
                    yield return new UndeclaredIdentifierException(Prev);
                }

                if (right == null) {
                    yield return new UndeclaredIdentifierException(SimpleExpr);
                }

                if (left != null && right != null) {
                    if (Operator == ExprOperator.InSet) {
                        if (!left.IsInteger) {
                            yield return new OperandTypeException(left, right, _opString, this);
                        }
                        if (!right.IsSet) {
                            yield return new OperandTypeException(left, right, _opString, this);
                        }
                    } else if (Operator == ExprOperator.Equals || Operator == ExprOperator.NotEquals) {
                        if (!OberonType.CanTestEquality(left, right)) {
                            yield return new OperandTypeException(left, right, _opString, this);
                        }
                    } else {
                        if (!OberonType.CanCompare(left, right)) {
                            yield return new OperandTypeException(left, right, _opString, this);
                        }
                    }
                }
            }
        }
    }
}
