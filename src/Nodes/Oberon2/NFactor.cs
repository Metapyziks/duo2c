using System;
using System.Linq;

using DUO2C.Semantics;

namespace DUO2C.Nodes.Oberon2
{
    [SubstituteToken("Factor")]
    public class NFactor : ExpressionElement
    {
        public ExpressionElement Inner
        {
            get { return (ExpressionElement) Children.First(); }
        }

        public override OberonType FinalType
        {
            get { return Inner.FinalType; }
        }

        public override bool IsConstant
        {
            get { return Inner.IsConstant; }
        }

        public NFactor(ParseNode original)
            : base(original, false)
        {
            if (Children.Count() == 3) {
                Children = new ParseNode[] { Children.ElementAt(1) };
            }
        }
    }
}
