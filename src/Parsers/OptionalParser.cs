using System.Collections.Generic;
using System.Linq;

using DUO2C.Nodes;

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

        public override bool IsMatch(string str, ref int i, bool whitespace)
        {
            int init = i;
            if (Left == null || Left.IsMatch(str, ref i, whitespace)) {
                init = i;
                if (Right.IsMatch(str, ref i, whitespace)) return true;

                // Reset index to before the last match was attempted
                i = init; return true;
            }
            i = init; return false;
        }

        public override ParseNode Parse(string str, ref int i, bool whitespace)
        {
            var left = (Left == null) ? null : Left.Parse(str, ref i, whitespace);

            int j = i;
            if (!Right.IsMatch(str, ref j, whitespace)) {
                // If the optional parser doesn't match, just
                // return the left result (may be null)
                return left;
            }

            var right = Right.Parse(str, ref i, whitespace);
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

        public override IEnumerable<int> FindSyntaxError(string str, int i, bool whitespace, out ParserException exception)
        {
            exception = null;
            SortedSet<int> indices = new SortedSet<int>();
            List<int> left;
            if (Left != null) {
                left = Left.FindSyntaxError(str, i, whitespace, out exception).ToList();
            } else {
                left = new List<int> { i };
            }
            foreach (int j in left) {
                indices.Add(j);
                ParserException innerError;
                foreach (int k in Right.FindSyntaxError(str, j, whitespace, out innerError)) {
                    indices.Add(k);
                }
                exception = ChooseParserException(exception, innerError);
            }
            return indices;
        }

        public override string ToString()
        {
            return (Left != null ? Left.ToString() + " " : "") + "[" + Right.ToString() + "]";
        }
    }
}
