using System.Linq;

namespace DUO2C.Parsers
{
    public class OptionalParser : BinaryParser
    {
        public OptionalParser(Parser left, Parser right)
            : base(left, right) { }

        public override bool IsMatch(string str, ref int i)
        {
            int init = i;
            if (Left == null || Left.IsMatch(str, ref i)) {
                init = i;
                if (Right.IsMatch(str, ref i)) return true;
                i = init; return true;
            }
            i = init; return false;
        }

        public override ParseNode Parse(string str, ref int i)
        {
            var left = (Left == null) ? null : Left.Parse(str, ref i);

            int j = i;
            if (!Right.IsMatch(str, ref j)) {
                return left;
            }

            var right = Right.Parse(str, ref i);
            if (left == null) return right;

            if (right is BranchNode && right.Token == null) {
                return new BranchNode(new ParseNode[] { left }.Concat(((BranchNode) right).Children));
            } else {
                return new BranchNode(new ParseNode[] { left, right });
            }
        }

        public override string ToString()
        {
            return (Left != null ? Left.ToString() : "") + " [" + Right.ToString() + "]";
        }
    }
}
