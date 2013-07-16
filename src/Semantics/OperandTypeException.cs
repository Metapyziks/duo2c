using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DUO2C.Nodes;

namespace DUO2C.Semantics
{
    public class OperandTypeException : ParserException
    {
        public OberonType Left { get; private set; }
        public OberonType Right { get; private set; }
        public String Operator { get; private set; }

        public OperandTypeException(OberonType left, OberonType right, String op, ParseNode node)
            : base(ParserError.Type, String.Format("Operator '{0}' cannot be applied to "
                + "operands of type '{1}' and '{2}'", op.ToString(), left.ToString(), right.ToString()),
                node.StartIndex, node.Length)
        {
            Left = left;
            Right = right;
            Operator = op;
        }
    }
}
