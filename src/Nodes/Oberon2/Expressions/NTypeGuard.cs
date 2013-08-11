using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUO2C.Nodes.Oberon2
{
    public class NTypeGuard : Selector
    {
        public override string String
        {
            get { return String.Format("({0})", TypeIdent.String); }
        }

        public NQualIdent TypeIdent
        {
            get { return (NQualIdent) Children.First(); }
        }

        public NTypeGuard(NInvocation original)
            : base(original, true)
        {
            Token = "TypeGuard";

            var expr = original.Args.Expressions.First();
            var simpleExpr = expr.SimpleExpr;
            var term = simpleExpr.Term;
            var factor = term.Factor;

            if (factor.Inner is NDesignator) {
                Children = new ParseNode[] {
                    ((NDesignator) factor.Inner).Element as NQualIdent
                };
            }
        }
    }
}
