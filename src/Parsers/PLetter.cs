using System;
using System.Collections.Generic;

namespace DUO2C.Parsers
{
    /// <summary>
    /// Exception thrown when a letter is expected but not found.
    /// </summary>
    public class LetterExpectedException : ParserException
    {
        /// <summary>
        /// Constructor to create a new letter expected exception, containing
        /// information about the location in the source string that the exception
        /// occurred.
        /// </summary>
        /// <param name="str">The source string being parsed</param>
        /// <param name="index">Start index in the source string of the exception</param>
        public LetterExpectedException(String str, int index)
            : base("Letter expected", str, index) { }
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

        public override bool IsMatch(string str, ref int i)
        {
            if (i < str.Length && char.IsLetter(str[i])) {
                ++i; return true;
            }
            return false;
        }

        public override ParseNode Parse(string str, ref int i)
        {
            return new LeafNode(i, 1, str[i++].ToString(), "letter");
        }

        protected override IEnumerable<int> FindSyntaxError(string str, int i)
        {
            if (IsMatch(str, ref i)) {
                yield return i;
            } else {
                throw new LetterExpectedException(str, i);
            }
        }

        public override string ToString()
        {
            return "letter";
        }
    }
}
