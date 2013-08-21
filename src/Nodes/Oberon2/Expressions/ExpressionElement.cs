using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DUO2C.Semantics;

namespace DUO2C.Nodes.Oberon2
{
    public abstract class ExpressionElement : SubstituteNode, ITypeErrorSource
    {
        protected static readonly IEnumerable<CompilerException> EmptyExceptionArray = new CompilerException[0];

        public ExpressionElement(ParseNode original, bool leaf, bool hasPayload = true)
            : base(original, leaf, hasPayload) { }

        public abstract OberonType GetFinalType(Scope scope);

        public abstract bool IsConstant(Scope scope);
        public abstract LiteralElement EvaluateConst(Scope scope);

        public abstract IEnumerable<CompilerException> FindTypeErrors(Scope scope);
    }

    public abstract class LiteralElement : ExpressionElement
    {
        public LiteralElement(ParseNode original, bool leaf, bool hasPayload = true)
            : base(original, leaf, hasPayload) { }

        public abstract LiteralElement EvaluateConst(ParseNode orig, LiteralElement other, ExprOperator op, Scope scope);
        public abstract LiteralElement EvaluateConst(ParseNode orig, LiteralElement other, SimpleExprOperator op, Scope scope);
        public abstract LiteralElement EvaluateConst(ParseNode orig, LiteralElement other, TermOperator op, Scope scope);
        public abstract LiteralElement EvaluateConst(ParseNode orig, UnaryOperator op, Scope scope);

        public override LiteralElement EvaluateConst(Scope scope)
        {
            return this;
        }
    }
}
