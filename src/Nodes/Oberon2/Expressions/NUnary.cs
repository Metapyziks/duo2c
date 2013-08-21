using System;
using System.Collections.Generic;
using System.Linq;

using DUO2C.Semantics;

namespace DUO2C.Nodes.Oberon2
{
    public enum UnaryOperator : byte
    {
        Identity,
        Negation,
        Not
    }

    [SubstituteToken("Unary")]
    public class NUnary : ExpressionElement
    {
        [Serialize("operator")]
        public UnaryOperator Operator { get; private set; }

        public NFactor Factor
        {
            get { return (NFactor) Children.First(); }
        }

        public override OberonType GetFinalType(Scope scope)
        {
            return Factor.GetFinalType(scope);
        }

        public override bool IsConstant(Scope scope)
        {
            return Factor.IsConstant(scope);
        }

        public NUnary(ParseNode original)
            : base(original, false)
        {
            switch (Children.First().String) {
                case "+":
                    Operator = UnaryOperator.Identity; break;
                case "-":
                    Operator = UnaryOperator.Negation; break;
                case "~":
                    Operator = UnaryOperator.Not; break;
            }

            Children = new ParseNode[] { Children.Last() };
        }

        public override IEnumerable<CompilerException> FindTypeErrors(Scope scope)
        {
            bool foundInner = false;
            foreach (var e in Factor.FindTypeErrors(scope)) {
                foundInner = true;
                yield return e;
            }

            if (!foundInner) {
                var type = GetFinalType(scope);
                if (Operator == UnaryOperator.Not) {
                    if (!(type is BooleanType)) {
                        yield return new TypeMismatchException(BooleanType.Default, type, Factor);
                    }
                } else if (!(type is SetType) && !(type is NumericType)) {
                    yield return new TypeMismatchException(NumericType.Default, type, Factor);
                }
            }
        }

        public override LiteralElement EvaluateConst(Scope scope)
        {
            return Factor.EvaluateConst(scope).EvaluateConst(this, Operator, scope);
        }
    }
}
