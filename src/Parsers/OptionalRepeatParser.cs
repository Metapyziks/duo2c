using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUO2C.Parsers
{
    class OptionalRepeatParser : BinaryParser
    {
        public OptionalRepeatParser(Parser left, Parser right)
            : base(left, right) { }

        public override bool IsMatch(string str, ref int i)
        {
            int init = i;
            if (Left == null || Left.IsMatch(str, ref i)) {
                init = i;
                while (Right.IsMatch(str, ref i)) init = i;
                i = init; return true;
            }
            i = init; return false;
        }

        public override ParseNode Parse(string str, ref int i)
        {
            var left = Left == null ? null : Left.Parse(str, ref i);

            var right = new List<ParseNode>();

            int init = i;
            while (Right.IsMatch(str, ref i)) {
                i = init;
                right.Add(Right.Parse(str, ref i));
                init = i;
            }

            if (left == null) return new BranchNode(right);

            if (left is BranchNode && left.Token == null) {
                return new BranchNode(((BranchNode) left).Children.Concat(right));
            } else {
                return new BranchNode(new ParseNode[] { left }.Concat(right));
            }
        }

        public override string ToString()
        {
            return (Left != null ? Left.ToString() : "") + " {" + Right.ToString() + "}";
        }
    }
}
