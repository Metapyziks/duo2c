using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DUO2C.Parsers;
using DUO2C.Nodes;
using DUO2C.Nodes.Oberon2;

namespace DUO2C.Semantics
{
    public class TypeMismatchException : ParserException
    {
        public OberonType Expected { get; private set; }
        public OberonType Actual { get; private set; }

        public TypeMismatchException(OberonType expected, ExpressionElement node)
            : base(String.Format("Type mismatch: expected {0}, received {1}", expected, node.FinalType),
                node.StartIndex, node.Length)
        {
            Expected = expected;
            Actual = node.FinalType;
        }
    }
}
