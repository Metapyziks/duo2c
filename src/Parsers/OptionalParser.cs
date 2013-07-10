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

        public override IEnumerable<ParseNode> Parse(string str, int i, bool whitespace, out ParserException exception)
        {
            exception = null;
            SortedSet<ParseNode> nodes = new SortedSet<ParseNode>();
            List<ParseNode> leftNodes;
            if (Left != null) {
                leftNodes = Left.Parse(str, i, whitespace, out exception).ToList();
            } else {
                leftNodes = new List<ParseNode> { new BranchNode(i) };
            }
            foreach (var left in leftNodes) {
                nodes.Add(left);
                ParserException innerError;
                foreach (var right in Right.Parse(str, left.EndIndex, whitespace, out innerError)) {
                    // If left side is null, don't bother concatenating
                    if (left.IsNull) {
                        nodes.Add(right);
                    } else if (left is BranchNode && left.Token == null) {
                        // If the parsed left hand side is a branch with no assigned token,
                        // append the parsed right hand side
                        nodes.Add(new BranchNode(((BranchNode) left).Children.Concat(new ParseNode[] { right })));
                    } else if (right is BranchNode && right.Token == null) {
                        // If the parsed right hand side is a branch with no assigned token,
                        // prepend the parsed left hand side
                        nodes.Add(new BranchNode(new ParseNode[] { left }.Concat(((BranchNode) right).Children)));
                    } else {
                        // Otherwise, create a new branch with only the two parsed results
                        nodes.Add(new BranchNode(new ParseNode[] { left, right }));
                    }
                }
                exception = ChooseParserException(exception, innerError);
            }
            return nodes;
        }

        public override string ToString()
        {
            return (Left != null ? Left.ToString() + " " : "") + "[" + Right.ToString() + "]";
        }
    }
}
