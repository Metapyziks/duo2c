using System;
using System.Linq;
using System.Collections.Generic;

namespace DUO2C.Parsers
{
    /// <summary>
    /// Parser that attempts two parsers, selecting the first
    /// to match.
    /// </summary>
    public class EitherOrParser : BinaryParser
    {
        /// <summary>
        /// Constructor to create a new EitherOrParser.
        /// </summary>
        /// <param name="ruleset">The ruleset that will contain this parser</param>
        /// <param name="left">Parser to be attempted first</param>
        /// <param name="right">Parser to be attempted second</param>
        public EitherOrParser(Ruleset ruleset, Parser left, Parser right)
            : base(ruleset, left, right) { }

        public override bool IsMatch(string str, ref int i)
        {
            int init = i;
            if (Left.IsMatch(str, ref i)) return true;

            // Reset index before attempting second parser
            i = init;
            if (Right.IsMatch(str, ref i)) return true;
            i = init; return false;
        }

        public override ParseNode Parse(string str, ref int i)
        {
            int j = i;
            if (Left.IsMatch(str, ref j)) {
                return Left.Parse(str, ref i);
            } else {
                return Right.Parse(str, ref i);
            }
        }

        protected override IEnumerable<int> FindSyntaxError(string str, int i)
        {
            ParserException left, right;
            var indices = Left.FindSyntaxError(str, i, out left).Union(Right.FindSyntaxError(str, i, out right));
            foreach (int j in indices) {
                yield return j;
            }
            if (left != null && right != null) {
                throw left.SourceIndex >= right.SourceIndex ? left : right;
            } else if (left != null) {
                throw left;
            } else if (right != null) {
                throw right;
            }
        }

        public override string ToString()
        {
            return Left.ToString() + " | " + Right.ToString();
        }
    }
}
