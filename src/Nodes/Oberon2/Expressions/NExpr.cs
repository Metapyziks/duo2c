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
        InSet
    }

    [SubstituteToken("Expr")]
    public class NExpr : ExpressionElement
    {
        static readonly Dictionary<String, ExprOperator> _sOpMap = new Dictionary<string,ExprOperator>() {
            { "=", ExprOperator.Equals },
            { "#", ExprOperator.NotEquals },
            { "<", ExprOperator.LessThan },
            { "<=", ExprOperator.LessThanOrEqual },
            { ">", ExprOperator.GreaterThan },
            { ">=", ExprOperator.GreaterThanOrEqual },
            { "IN", ExprOperator.InSet }
        };

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

        public override string String
        {
            get {
                return Prev != null ? String.Format("{0} {1} {2}", Prev.String, _opString, SimpleExpr.String) : SimpleExpr.String;
            }
        }

        public override OberonType GetFinalType(Scope scope)
        {
            if (Prev == null) return SimpleExpr.GetFinalType(scope);

            var left = Prev.GetFinalType(scope);
            var right = SimpleExpr.GetFinalType(scope);

            bool isVector = left.IsVector || right.IsVector;
            int vecLength = isVector ? left.IsVector ? left.As<VectorType>().Length : right.As<VectorType>().Length : 0;

            if (isVector) {
                return new VectorType(BooleanType.Default, vecLength);
            } else {
                return BooleanType.Default;
            }
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
                Operator = _sOpMap[_opString];

                Children = new ParseNode[] {
                    new NExpr(new BranchNode(Children.Take(Children.Count() - 2), Token)),
                    SimpleExpr
                };
            }
        }

        public override IEnumerable<CompilerException> FindTypeErrors(Scope scope)
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
                    innerFound = true;
                    yield return new UndeclaredIdentifierException(Prev);
                } else if (left.IsVector) {
                    left = left.As<VectorType>().ElementType;
                }

                if (right == null) {
                    innerFound = true;
                    yield return new UndeclaredIdentifierException(SimpleExpr);
                } else if (right.IsVector) {
                    right = right.As<VectorType>().ElementType;
                }

                if (left != null && right != null) {
                    if (Operator == ExprOperator.InSet) {
                        if (!left.IsInteger) {
                            innerFound = true;
                            yield return new OperandTypeException(left, right, _opString, this);
                        }
                        if (!right.IsSet) {
                            innerFound = true;
                            yield return new OperandTypeException(left, right, _opString, this);
                        }
                    } else if (Operator == ExprOperator.Equals || Operator == ExprOperator.NotEquals) {
                        if (!OberonType.CanTestEquality(left, right)) {
                            innerFound = true;
                            yield return new OperandTypeException(left, right, _opString, this);
                        }
                    } else {
                        if (!OberonType.CanCompare(left, right)) {
                            innerFound = true;
                            yield return new OperandTypeException(left, right, _opString, this);
                        }
                    }
                }

                if (!innerFound && IsConstant(scope)) {
                    SimpleExpr.Term.Factor.OverwriteConst(EvaluateConst(scope));
                    Children = new[] { SimpleExpr };
                    Operator = ExprOperator.None;
                }
            }
        }

        public override LiteralElement EvaluateConst(Scope scope)
        {
            return Operator == ExprOperator.None
                ? SimpleExpr.EvaluateConst(scope)
                : Prev.EvaluateConst(scope).EvaluateConst(this, SimpleExpr.EvaluateConst(scope), Operator, scope);
        }
    }
}
