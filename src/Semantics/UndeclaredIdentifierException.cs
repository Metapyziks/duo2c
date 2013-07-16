using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DUO2C.Nodes.Oberon2;

namespace DUO2C.Semantics
{
    public class UndeclaredIdentifierException : ParserException
    {
        public UndeclaredIdentifierException(ExpressionElement node)
            : base(ParserError.Semantic, "Undeclared identifier", node.StartIndex, node.Length) { }
    }
}
