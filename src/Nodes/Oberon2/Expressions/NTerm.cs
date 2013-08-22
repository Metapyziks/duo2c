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
        static readonly Dictionary<String, TermOperator> _sOpMap = new Dictionary<string,TermOperator>() {
            { "*", TermOperator.Multiply },
            { "/", TermOperator.Divide },
            { "DIV", TermOperator.IntDivide },
            { "MOD", TermOperator.Modulo },
            { "&", TermOperator.And }
        };

        String _opString;

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

        public override string String
        {
            get {
                return Prev != null ? String.Format("{0} {1} {2}", Prev.String, _opString, Factor.String) : Factor.String;
            }
        }

        public override OberonType GetFinalType(Scope scope)
        {
            if (Prev == null) {
                return Factor.GetFinalType(scope);
            } else {
                var left = Prev.GetFinalType(scope);
                var right = Factor.GetFinalType(scope);

                bool isVector = left.IsVector || right.IsVector;
                int vecLength = isVector ? left.IsVector ? left.As<VectorType>().Length : right.As<VectorType>().Length : 0;

                if (left.IsVector) left = left.As<VectorType>().ElementType;
                if (right.IsVector) right = right.As<VectorType>().ElementType;

                OberonType type;

                if (Operator == TermOperator.And) {
                    type = BooleanType.Default;
                } else if (right.IsSet) {
                    type = SetType.Default;
                } else if (Operator == TermOperator.Divide) {
                    type = NumericType.Largest(RealType.Real,
                        NumericType.Largest(left.As<NumericType>(), right.As<NumericType>()));
                } else {
                    type = NumericType.Largest(left.As<NumericType>(), right.As<NumericType>());
                }

                return isVector ? (OberonType) new VectorType(type, vecLength) : type;
            }
        }

        public override bool IsConstant(Scope scope)
        {
            return Factor.IsConstant(scope) && (Prev == null || Prev.IsConstant(scope));
        }

        public NTerm(ParseNode original)
            : base(original, false)
        {
            if (Children.Count() == 1) {
                Operator = TermOperator.None;
            } else {
                _opString = Children.ElementAt(Children.Count() - 2).String;
                Operator = _sOpMap[_opString];

                Children = new ParseNode[] {
                    new NTerm(new BranchNode(Children.Take(Children.Count() - 2), Token)),
                    Factor
                };
            }
        }

        public override IEnumerable<CompilerException> FindTypeErrors(Scope scope)
        {
            bool innerFound = false;

            foreach (var e in Factor.FindTypeErrors(scope)) {
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
                var right = Factor.GetFinalType(scope);

                if (left == null) {
                    innerFound = true;
                    yield return new UndeclaredIdentifierException(Prev);
                } else if (left.IsVector) {
                    left = left.As<VectorType>().ElementType;
                }

                if (right == null) {
                    innerFound = true;
                    yield return new UndeclaredIdentifierException(Factor);
                } else if (right.IsVector) {
                    right = right.As<VectorType>().ElementType;
                }

                if (left != null && right != null) {
                    if (Operator == TermOperator.IntDivide || Operator == TermOperator.Modulo) {
                        if (!left.IsInteger || !right.IsInteger) {
                            innerFound = true;
                            yield return new OperandTypeException(left, right, _opString, this);
                        }
                    } else if (Operator == TermOperator.And) {
                        if (!left.IsBool || !right.IsBool) {
                            innerFound = true;
                            yield return new OperandTypeException(left, right, _opString, this);
                        }
                    } else if ((!left.IsSet || !right.IsSet) && (!left.IsNumeric || !right.IsNumeric)) {
                        innerFound = true;
                        yield return new OperandTypeException(left, right, _opString, this);
                    }
                }

                if (!innerFound && IsConstant(scope)) {
                    Factor.OverwriteConst(EvaluateConst(scope));
                    Children = new[] { Factor };
                    Operator = TermOperator.None;
                }
            }
        }

        public override LiteralElement EvaluateConst(Scope scope)
        {
            return Operator == TermOperator.None
                ? Factor.EvaluateConst(scope)
                : Prev.EvaluateConst(scope).EvaluateConst(this, Factor.EvaluateConst(scope), Operator, scope);
        }
    }
}
