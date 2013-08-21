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

        public abstract IEnumerable<CompilerException> FindTypeErrors(Scope scope);
    }

    public abstract class FactorElement : ExpressionElement
    {
        public FactorElement(ParseNode original, bool leaf, bool hasPayload = true)
            : base(original, leaf, hasPayload) { }

        public abstract FactorElement EvaluateConst(FactorElement other, ExprOperator op);
        public abstract FactorElement EvaluateConst(FactorElement other, SimpleExprOperator op);
        public abstract FactorElement EvaluateConst(FactorElement other, TermOperator op);
        public abstract FactorElement EvaluateConst(UnaryOperator op);
    }
}
