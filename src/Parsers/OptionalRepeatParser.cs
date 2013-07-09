﻿using System.Collections.Generic;
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

        public override ParserException FindSyntaxErrors(string str, ref int i)
        {
            int j = i;
            var error = Left.FindSyntaxErrors(str, ref i);
            while (error == null && j < i) {
                error = Right.FindSyntaxErrors(str, ref i);
                j = i;
            }
            return error;
        }

        public override string ToString()
        {
            return (Left != null ? Left.ToString() + " " : "") + "{" + Right.ToString() + "}";
        }
    }
}
