using System;
using System.Collections.Generic;

using DUO2C.Nodes;

namespace DUO2C.Parsers
{
    /// <summary>
    /// Atomic parser that parses a single digit.
    /// </summary>
    public class PDigit : Parser
    {
        /// <summary>
        /// Constructor to create a new digit parser.
        /// </summary>
        /// <param name="ruleset">The ruleset that will contain this parser</param>
        public PDigit(Ruleset ruleset)
            : base(ruleset) { }

        public override bool IsMatch(string str, ref int i, bool whitespace)
        {
            int init = i;
            if (whitespace) SkipWhitespace(str, ref i);
            if (i < str.Length && char.IsDigit(str[i])) {
                ++i; return true;
            }
            i = init; return false;
        }

        public override IEnumerable<ParseNode> Parse(string str, int i, bool whitespace, out CompilerException exception)
        {
            int j = i;
            if (IsMatch(str, ref j, whitespace)) {
                exception = null;
                return new ParseNode[] { new LeafNode(i, 1, str[i].ToString(), "digit") };
            } else {
                exception = new SymbolExpectedException("Digit", i, 1);
                return EmptyNodeArray;
            }
        }

        public override string ToString()
        {
            return "digit";
        }
    }
}
