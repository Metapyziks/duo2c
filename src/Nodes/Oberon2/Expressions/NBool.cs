using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DUO2C.Semantics;

namespace DUO2C.Nodes.Oberon2
{
    [SubstituteToken("Bool")]
    public class NBool : LiteralElement
    {
        public bool Value { get; private set; }

        public override string String
        {
            get { return Value.ToString().ToUpper(); }
        }

        public override OberonType GetFinalType(Scope scope)
        {
            return BooleanType.Default;
        }

        public override bool IsConstant(Scope scope)
        {
            return true;
        }

        public NBool(ParseNode original)
            : base(original, true)
        {
            Value = original.String.Trim().Equals("TRUE");
        }

        public NBool(ParseNode orig, bool value)
            : base(orig, true)
        {
            Value = value;
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
