using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DUO2C.Semantics;

namespace DUO2C.Nodes.Oberon2
{
    [SubstituteToken("Nil")]
    public class NNil : LiteralElement
    {
        public override string String
        {
            get { return "NIL"; }
        }

        public NNil(ParseNode original)
            : base(original, true, false)
        {
            Children = new ParseNode[0];
        }

        public override OberonType GetFinalType(Scope scope)
        {
            return PointerType.Null;
        }

        public override bool IsConstant(Scope scope)
        {
            return true;
        }

        public override IEnumerable<CompilerException> FindTypeErrors(Scope scope)
        {
            return EmptyExceptionArray;
        }

        public override LiteralElement EvaluateConst(NExpr orig, LiteralElement other, ExprOperator op, Scope scope)
        {
            throw new NotImplementedException();
        }

        public override LiteralElement EvaluateConst(NSimpleExpr orig, LiteralElement other, SimpleExprOperator op, Scope scope)
        {
            throw new NotImplementedException();
        }

        public override LiteralElement EvaluateConst(NTerm orig, LiteralElement other, TermOperator op, Scope scope)
        {
            throw new NotImplementedException();
        }

        public override LiteralElement EvaluateConst(NUnary orig, UnaryOperator op, Scope scope)
        {
            throw new NotImplementedException();
        }
    }
}
