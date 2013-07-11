using System;
using System.Collections.Generic;

using DUO2C.Nodes;

namespace DUO2C.Parsers
{
    /// <summary>
    /// Exception thrown when a letter is expected but not found.
    /// </summary>
    [ExceptionUtility(10)]
    public class LetterExpectedException : ParserException
    {
        /// <summary>
        /// Constructor to create a new letter expected exception, containing
        /// information about the location in the source string that the exception
        /// occurred.
        /// </summary>
        /// <param name="index">Start index in the source string of the exception</param>
        public LetterExpectedException(int index)
            : base("Letter expected", index) { }
    }

    /// <summary>
    /// Atomic parser that parses a single letter.
    /// </summary>
    public class PLetter : Parser
    {
        /// <summary>
        /// Constructor to create a new letter parser.
        /// </summary>
        /// <param name="ruleset">The ruleset that will contain this parser</param>
        public PLetter(Ruleset ruleset)
            : base(ruleset) { }

        public override bool IsMatch(string str, ref int i, bool whitespace)
        {
            int init = i;
            if (whitespace) SkipWhitespace(str, ref i);
            if (i < str.Length && char.IsLetter(str[i])) {
                ++i; return true;
            }
            i = init; return false;
        }

        public override IEnumerable<ParseNode> Parse(string str, int i, bool whitespace, out ParserException exception)
        {
            int j = i;
            if (IsMatch(str, ref j, whitespace)) {
                exception = null;
                return new ParseNode[] { new LeafNode(i, 1, str[i].ToString(), "letter") };
            } else {
                exception = new LetterExpectedException(i);
                return EmptyNodeArray;
            }
        }

        public override string ToString()
        {
            return "letter";
        }
    }
}
