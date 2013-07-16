using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DUO2C.Nodes.Oberon2;

namespace DUO2C.Semantics
{
    public class UnresolvedTypeException : ParserException
    {
        public UnresolvedTypeException(ExpressionElement node)
            : base(ParserError.Type, "Could not resolve type", node.StartIndex, node.Length) { }
    }
}
