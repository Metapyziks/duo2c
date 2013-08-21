using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DUO2C.Semantics;

namespace DUO2C.Nodes.Oberon2
{
    [SubstituteToken("ConstExpr")]
    public class NConstExpr : ExpressionElement
    {
        public NExpr Inner { get { return (NExpr) Children.First(); } }

        public override OberonType GetFinalType(Scope scope)
        {
            return Inner.GetFinalType(scope);
        }

        public override bool IsConstant(Scope scope)
        {
            return true;
        }

        public NConstExpr(ParseNode original)
            : base(original, false)
        {

        }

        public override IEnumerable<CompilerException> FindTypeErrors(Scope scope)
        {
            foreach (var e in Inner.FindTypeErrors(scope)) {
                yield return e;
            }

            if (!Inner.IsConstant(scope)) {
                yield return new ConstantExpectedException(Inner);
            }
        }

        public override LiteralElement EvaluateConst(Scope scope)
        {
            return Inner.EvaluateConst(scope);
        }
    }
}
