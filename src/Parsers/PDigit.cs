using System;
using System.Collections.Generic;

using DUO2C.Nodes;

namespace DUO2C.Parsers
{
    /// <summary>
    /// Exception thrown when a digit is expected but not found.
    /// </summary>
    [ExceptionUtility(10)]
    public class DigitExpectedException : ParserException
    {
        /// <summary>
        /// Constructor to create a new digit expected exception, containing
        /// information about the location in the source string that the exception
        /// occurred.
        /// </summary>
        /// <param name="index">Start index in the source string of the exception</param>
        public DigitExpectedException(int index)
            : base("Digit expected", index) { }
    }

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

        public override IEnumerable<ParseNode> Parse(string str, int i, bool whitespace, out ParserException exception)
        {
            int j = i;
            if (IsMatch(str, ref j, whitespace)) {
                exception = null;
                return new ParseNode[] { new LeafNode(i, 1, str[i].ToString(), "digit") };
            } else {
                exception = new DigitExpectedException(i);
                return EmptyNodeArray;
            }
        }

        public override string ToString()
        {
            return "digit";
        }
    }
}
