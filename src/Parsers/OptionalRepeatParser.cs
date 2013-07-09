using System;
using System.Collections.Generic;
using System.Linq;

namespace DUO2C.Parsers
{
    /// <summary>
    /// Binary parser that matches the left parser, and optionally matches
    /// the right parser as many times as possible.
    /// </summary>
    public class OptionalRepeatParser : BinaryParser
    {
        /// <summary>
        /// Constructor to create a new OptionalRepeatParser.
        /// </summary>
        /// <param name="ruleset">The ruleset that will contain this parser</param>
        /// <param name="left">Parser to be matched first</param>
        /// <param name="right">Parser to optionally attempt second</param>
        public OptionalRepeatParser(Ruleset ruleset, Parser left, Parser right)
            : base(ruleset, left, right) { }

        public override bool IsMatch(string str, ref int i)
        {
            int init = i;
            if (Left == null || Left.IsMatch(str, ref i)) {
                init = i;
                while (Right.IsMatch(str, ref i)) init = i;

                // Reset index to before the last match was attempted
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
                // Match the right parser as many times as possible
                right.Add(Right.Parse(str, ref i));
                init = i;
            }

            if (left == null) {
                if (right.Count() > 0) return new BranchNode(right);
                return new BranchNode(i);
            }

            if (right.Count() == 0) return left;

            if (left is BranchNode && left.Token == null) {
                // If the parsed left hand side is a branch with no assigned token,
                // append the parsed right hand side
                return new BranchNode(((BranchNode) left).Children.Concat(right));
            } else {
                // Otherwise, create a new branch with only the matches parsed here
                return new BranchNode(new ParseNode[] { left }.Concat(right));
            }
        }

        protected override IEnumerable<int> FindSyntaxError(string str, int i, ParserExceptionWrapper wrapper)
        {
            SortedSet<int> indices = new SortedSet<int>();
            List<int> fresh;
            if (Left != null) {
                fresh = Left.FindSyntaxError(str, i, out wrapper.Payload).ToList();
            } else {
                fresh = new List<int> { i };
            }
            List<int> stale = new List<int>();

            foreach (int j in fresh) indices.Add(j);

            while (fresh.Count > 0) {
                stale = fresh;
                fresh.Clear();

                foreach (int j in stale) {
                    ParserException innerError;
                    foreach (int k in Right.FindSyntaxError(str, j, out innerError)) {
                        if (indices.Add(k)) fresh.Add(k);
                    }
                    if (innerError != null && (wrapper.Payload == null || innerError.SourceIndex > wrapper.Payload.SourceIndex)) {
                        wrapper.Payload = innerError;
                    }
                }
            }

            return indices;
        }

        public override string ToString()
        {
            return (Left != null ? Left.ToString() + " " : "") + "{" + Right.ToString() + "}";
        }
    }
}
