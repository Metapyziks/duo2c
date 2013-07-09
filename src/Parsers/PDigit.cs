using System;
using System.Collections.Generic;

namespace DUO2C.Parsers
{
    /// <summary>
    /// Exception thrown when a digit is expected but not found.
    /// </summary>
    public class DigitExpectedException : ParserException
    {
        /// <summary>
        /// Constructor to create a new digit expected exception, containing
        /// information about the location in the source string that the exception
        /// occurred.
        /// </summary>
        /// <param name="str">The source string being parsed</param>
        /// <param name="index">Start index in the source string of the exception</param>
        public DigitExpectedException(String str, int index)
            : base("Digit expected", str, index) { }
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

        public override bool IsMatch(string str, ref int i)
        {
            if (i < str.Length && char.IsDigit(str[i])) {
                ++i; return true;
            }
            return false;
        }

        public override ParseNode Parse(string str, ref int i)
        {
            return new LeafNode(i, 1, str[i++].ToString(), "digit");
        }

        protected override IEnumerable<int> FindSyntaxError(string str, int i)
        {
            if (IsMatch(str, ref i)) {
                yield return i;
            } else {
                throw new DigitExpectedException(str, i);
            }
        }

        public override string ToString()
        {
            return "digit";
        }
    }
}
