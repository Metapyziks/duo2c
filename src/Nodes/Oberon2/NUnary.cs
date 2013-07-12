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

        public override OberonType FinalType
        {
            get { return Factor.FinalType; }
        }

        public override bool IsConstant
        {
            get { return Factor.IsConstant; }
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

        public override IEnumerable<ParserException> CheckTypes()
        {
            bool foundInner = false;
            foreach (var e in Factor.CheckTypes()) {
                foundInner = true;
                yield return e;
            }

            if (!foundInner) {
                if (Operator == UnaryOperator.Not) {
                    if (!(FinalType is BooleanType)) {
                        yield return new TypeMismatchException(BooleanType.Default, Factor);
                    }
                } else if (!(FinalType is SetType) && !(FinalType is NumericType)) {
                    yield return new TypeMismatchException(NumericType.Default, Factor);
                }
            }
        }
    }
}
