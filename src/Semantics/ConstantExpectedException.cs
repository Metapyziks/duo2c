using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DUO2C.Nodes.Oberon2;

namespace DUO2C.Semantics
{
    public class ConstantExpectedException : ParserException
    {
        public ExpressionElement Element { get; private set; }

        public ConstantExpectedException(ExpressionElement element)
            : base(ParserError.Semantic, "Constant value expected", element.StartIndex, element.Length)
        {
            Element = element;
        }
    }
}
