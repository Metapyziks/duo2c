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

        public override OberonType GetFinalType(Scope scope)
        {
            if (Prev == null) {
                return Factor.GetFinalType(scope);
            } else {
                if (Operator == TermOperator.And) {
                    return BooleanType.Default;
                } else if (Factor.GetFinalType(scope).IsSet) {
                    return SetType.Default;
                } else if (Operator == TermOperator.Divide) {
                    return NumericType.Largest(RealType.Real,
                        NumericType.Largest(Factor.GetFinalType(scope).As<NumericType>(), Prev.GetFinalType(scope).As<NumericType>()));
                } else {
                    return NumericType.Largest(Factor.GetFinalType(scope).As<NumericType>(), Prev.GetFinalType(scope).As<NumericType>());
                }
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
                switch (_opString) {
                    case "*":
                        Operator = TermOperator.Multiply; break;
                    case "/":
                        Operator = TermOperator.Divide; break;
                    case "DIV":
                        Operator = TermOperator.IntDivide; break;
                    case "MOD":
                        Operator = TermOperator.Modulo; break;
                    case "&":
                        Operator = TermOperator.And; break;
                    default:
                        Operator = TermOperator.None; break;
                }

                Children = new ParseNode[] {
                    new NTerm(new BranchNode(Children.Take(Children.Count() - 2), Token)),
                    Factor
                };
            }
        }

        public override IEnumerable<ParserException> FindTypeErrors(Scope scope)
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
                    yield return new UndeclaredIdentifierException(Prev);
                }

                if (right == null) {
                    yield return new UndeclaredIdentifierException(Factor);
                }

                if (left != null && right != null) {
                    if (Operator == TermOperator.IntDivide || Operator == TermOperator.Modulo) {
                        if (!left.IsInteger || !right.IsInteger) {
                            yield return new OperandTypeException(left, right, _opString, this);
                        }
                    } else if (Operator == TermOperator.And) {
                        if (!left.IsBool || !right.IsBool) {
                            yield return new OperandTypeException(left, right, _opString, this);
                        }
                    } else if ((!left.IsSet || !right.IsSet) && (!left.IsNumeric || !right.IsNumeric)) {
                        yield return new OperandTypeException(left, right, _opString, this);
                    }
                }
            }
        }
    }
}
