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
        public ExpressionElement(ParseNode original, bool leaf = false, bool hasPayload = true)
            : base(original, leaf, hasPayload) { }

        public abstract OberonType FinalType();
    }
}
