using System.Linq;

namespace DUO2C.Parsers
{
    /// <summary>
    /// Binary parser that matches the left parser, and optionally matches
    /// the right parser if possible.
    /// </summary>
    public class OptionalParser : BinaryParser
    {
        /// <summary>
        /// Constructor to create a new OptionalParser.
        /// </summary>
        /// <param name="ruleset">The ruleset that will contain this parser</param>
        /// <param name="left">Parser to be matched first</param>
        /// <param name="right">Parser to optionally attempt second</param>
        public OptionalParser(Ruleset ruleset, Parser left, Parser right)
            : base(ruleset, left, right) { }

        public override bool IsMatch(string str, ref int i)
        {
            int init = i;
            if (Left == null || Left.IsMatch(str, ref i)) {
                init = i;
                if (Right.IsMatch(str, ref i)) return true;

                // Reset index to before the last match was attempted
                i = init; return true;
            }
            i = init; return false;
        }

        public override ParseNode Parse(string str, ref int i)
        {
            var left = (Left == null) ? null : Left.Parse(str, ref i);

            int j = i;
            if (!Right.IsMatch(str, ref j)) {
                // If the optional parser doesn't match, just
                // return the left result (may be null)
                return left;
            }

            var right = Right.Parse(str, ref i);
            if (left == null) return right;

            if (left is BranchNode && left.Token == null) {
                // If the parsed left hand side is a branch with no assigned token,
                // append the parsed right hand side
                return new BranchNode(((BranchNode) left).Children.Concat(new ParseNode[] { right }));
            } else {
                // Otherwise, create a new branch with only the two parsed results
                return new BranchNode(new ParseNode[] { left, right });
            }
        }

        public override ParserException FindSyntaxErrors(string str, ref int i)
        {
            return Left.FindSyntaxErrors(str, ref i) ?? Right.FindSyntaxErrors(str, ref i);
        }

        public override string ToString()
        {
            return (Left != null ? Left.ToString() + " " : "") + "[" + Right.ToString() + "]";
        }
    }
}
