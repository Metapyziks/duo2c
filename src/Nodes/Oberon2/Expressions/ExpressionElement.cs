using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DUO2C.Semantics;

namespace DUO2C.Nodes.Oberon2
{
    public abstract class ExpressionElement : SubstituteNode
    {
        protected static readonly IEnumerable<ParserException> EmptyExceptionArray = new ParserException[0];

        public ExpressionElement(ParseNode original, bool leaf, bool hasPayload = true)
            : base(original, leaf, hasPayload) { }

        public abstract OberonType FinalType { get; }

        public abstract bool IsConstant { get; }

        public abstract IEnumerable<ParserException> FindTypeErrors();
    }
}
