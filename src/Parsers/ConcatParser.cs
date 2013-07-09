using System.Collections.Generic;
using System.Linq;

namespace DUO2C.Parsers
{
    /// <summary>
    /// Parser that concatenates two other parsers.
    /// </summary>
    public class ConcatParser : BinaryParser
    {
        /// <summary>
        /// Constructor to create a new ConcatParser.
        /// </summary>
        /// <param name="ruleset">The ruleset that will contain this parser</param>
        /// <param name="left">Parser to be matched first</param>
        /// <param name="right">Parser to be matched second</param>
        public ConcatParser(Ruleset ruleset, Parser left, Parser right)
            : base(ruleset, left, right) { }

        public override bool IsMatch(string str, ref int i)
        {
            int init = i;
            if (Left.IsMatch(str, ref i) && Right.IsMatch(str, ref i)) return true;
            i = init; return false;
        }

        public override ParseNode Parse(string str, ref int i)
        {
            var left = Left.Parse(str, ref i);
            var right = Right.Parse(str, ref i);

            // If either side is null, don't bother concatenating
            if (left == null) return right;
            if (right == null) return left;

            if (left is BranchNode && left.Token == null) {
                // If the parsed left hand side is a branch with no assigned token,
                // append the parsed right hand side
                return new BranchNode(((BranchNode) left).Children.Concat(new ParseNode[] { right }));
            } else {
                // Otherwise, create a new branch with only the two parsed results
                return new BranchNode(new ParseNode[] { left, right });
            }
        }

        public override IEnumerable<int> FindSyntaxError(string str, int i, out ParserException exception)
        {
            SortedSet<int> indices = new SortedSet<int>();
            foreach (int j in Left.FindSyntaxError(str, i, out exception)) {
                ParserException innerError;
                foreach (int k in Right.FindSyntaxError(str, j, out innerError)) {
                    indices.Add(k);
                }
                exception = ChooseParserException(exception, innerError);
            }
            return indices;
        }

        public override string ToString()
        {
            return Left.ToString() + " " + Right.ToString();
        }
    }
}
