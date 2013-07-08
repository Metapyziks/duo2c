using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUO2C.Parsers
{
    class ConcatParser : BinaryParser
    {
        public ConcatParser(Parser left, Parser right)
            : base(left, right) { }

        public override bool IsMatch(string str, ref int i)
        {
            int init = i;
            bool left = Left.IsMatch(str, ref i);
            if (left) {
                bool right = Right.IsMatch(str, ref i);
                if (right) return true;
            }
            i = init; return false;
        }

        public override ParseNode Parse(string str, ref int i)
        {
            var left = Left.Parse(str, ref i);
            var right = Right.Parse(str, ref i);
            if (left == null) return right;
            if (right == null) return left;

            if (left is BranchNode && left.Token == null) {
                return new BranchNode(((BranchNode) left).Children.Concat(new ParseNode[] { right }));
            } else {
                return new BranchNode(new ParseNode[] { left, right });
            }
        }

        public override string ToString()
        {
            return Left.ToString() + " " + Right.ToString();
        }
    }
}
