using System;
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

            if (Operator == UnaryOperator.Not) {
                if (!(FinalType is BooleanType)) {
                    throw new TypeMismatchException(BooleanType.Default, Factor);
                }
            } else if (!(FinalType is SetType) && !(FinalType is NumericType)) {
                throw new TypeMismatchException(NumericType.Default, Factor);
            }
        }
    }
}
