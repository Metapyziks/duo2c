using System;
using System.Collections.Generic;
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

        public override string String
        {
            get {
                if (Inner is NExpr) {
                    return String.Format("({0})", Inner.String);
                } else if (Inner is NString) {
                    return String.Format("\"{0}\"", Inner.String);
                } else {
                    return Inner.String;
                }
            }
        }

        public override OberonType GetFinalType(Scope scope)
        {
            return Inner.GetFinalType(scope);
        }

        public override bool IsConstant(Scope scope)
        {
            return Inner.IsConstant(scope);
        }

        public NFactor(ParseNode original)
            : base(original, false)
        {
            if (Children.Count() == 3) {
                Children = new ParseNode[] { Children.ElementAt(1) };
            }
        }

        public override IEnumerable<CompilerException> FindTypeErrors(Scope scope)
        {
            return Inner.FindTypeErrors(scope);
        }
    }
}
