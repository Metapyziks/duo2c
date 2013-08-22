﻿using System;
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
        static readonly Dictionary<String, SimpleExprOperator> _sOpMap = new Dictionary<string,SimpleExprOperator>() {
            { "+", SimpleExprOperator.Add },
            { "-", SimpleExprOperator.Subtract },
            { "OR", SimpleExprOperator.Or }
        };

        private String _opString;

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

        public override string String
        {
            get {
                return Prev != null ? String.Format("{0} {1} {2}", Prev.String, _opString, Term.String) : Term.String;
            }
        }

        public override OberonType GetFinalType(Scope scope)
        {
            if (Prev == null) {
                return Term.GetFinalType(scope);
            } else {
                var left = Prev.GetFinalType(scope);
                var right = Term.GetFinalType(scope);

                bool isVector = left.IsVector || right.IsVector;
                int vecLength = isVector ? left.IsVector ? left.As<VectorType>().Length : right.As<VectorType>().Length : 0;

                if (left.IsVector) left = left.As<VectorType>().ElementType;
                if (right.IsVector) right = right.As<VectorType>().ElementType;

                OberonType type;

                if (Operator == SimpleExprOperator.Or && Term.GetFinalType(scope).IsBool) {
                    type = BooleanType.Default;
                } else if (Term.GetFinalType(scope).IsSet) {
                    type = SetType.Default;
                } else {
                    type = NumericType.Largest(left.As<NumericType>(), right.As<NumericType>());
                }
                
                return isVector ? (OberonType) new VectorType(type, vecLength) : type;
            }
        }

        public override bool IsConstant(Scope scope)
        {
            return Term.IsConstant(scope) && (Prev == null || Prev.IsConstant(scope));
        }

        public NSimpleExpr(ParseNode original)
            : base(original, false)
        {
            if (Children.Count() == 1) {
                Operator = SimpleExprOperator.None;
            } else {
                _opString = Children.ElementAt(Children.Count() - 2).String;
                Operator = _sOpMap[_opString];

                Children = new ParseNode[] {
                    new NSimpleExpr(new BranchNode(Children.Take(Children.Count() - 2), Token)),
                    Term
                };
            }
        }

        public override IEnumerable<CompilerException> FindTypeErrors(Scope scope)
        {
            bool innerFound = false;

            foreach (var e in Term.FindTypeErrors(scope)) {
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
                var right = Term.GetFinalType(scope);

                if (left == null) {
                    innerFound = true;
                    yield return new UndeclaredIdentifierException(Prev);
                } else if (left.IsVector) {
                    left = left.As<VectorType>().ElementType;
                }

                if (right == null) {
                    innerFound = true;
                    yield return new UndeclaredIdentifierException(Term);
                } else if (right.IsVector) {
                    right = right.As<VectorType>().ElementType;
                }

                if (left != null && right != null) {
                    if (Operator == SimpleExprOperator.Or) {
                        if ((!left.IsBool || !right.IsBool) && (!left.IsInteger && !right.IsInteger)) {
                            innerFound = true;
                            yield return new OperandTypeException(left, right, _opString, this);
                        }
                    } else if ((!left.IsSet || !right.IsSet) && (!left.IsNumeric || !right.IsNumeric)) {
                        innerFound = true;
                        yield return new OperandTypeException(left, right, _opString, this);
                    }
                }

                if (!innerFound && IsConstant(scope)) {
                    Term.Factor.OverwriteConst(EvaluateConst(scope));
                    Children = new[] { Term };
                    Operator = SimpleExprOperator.None;
                }
            }
        }

        public override LiteralElement EvaluateConst(Scope scope)
        {
            return Operator == SimpleExprOperator.None
                ? Term.EvaluateConst(scope)
                : Prev.EvaluateConst(scope).EvaluateConst(this, Term.EvaluateConst(scope), Operator, scope);
        }
    }
}
