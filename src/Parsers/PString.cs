using System;
using System.Collections.Generic;

namespace DUO2C.Parsers
{
    /// <summary>
    /// Exception thrown when a string literal is expected but not found.
    /// </summary>
    [ExceptionUtility(50)]
    public class StringExpectedException : ParserException
    {
        /// <summary>
        /// Constructor to create a new digit expected exception, containing
        /// information about the location in the source string that the exception
        /// occurred.
        /// </summary>
        /// <param name="str">The source string being parsed</param>
        /// <param name="index">Start index in the source string of the exception</param>
        public StringExpectedException(String str, int index)
            : base("String literal expected", str, index) { }
    }

    /// <summary>
    /// Atomic parser that parses a string literal surrounded by either
    /// single or double quotes.
    /// </summary>
    public class PString : Parser
    {
        /// <summary>
        /// Constructor to create a new string literal parser.
        /// </summary>
        /// <param name="ruleset">The ruleset that will contain this parser</param>
        public PString(Ruleset ruleset)
            : base(ruleset) { }

        public override bool IsMatch(string str, ref int i)
        {
            int init = i;
            SkipWhitespace(str, ref i);
            if (i < str.Length && (str[i] == '"' || str[i] == '\'')) {
                char startChar = str[i];
                while (++i < str.Length) {
                    if (str[i] == '\\') {
                        ++i;
                    }
                    if (str[i] == startChar) {
                        ++i;
                        return true;
                    }
                }
            }
            i = init; return false;
        }

        // TODO: Add proper support for escape characters
        public override ParseNode Parse(string str, ref int i)
        {
            SkipWhitespace(str, ref i);
            int j = i;
            IsMatch(str, ref i);
            return new LeafNode(j, i - j, str.Substring(j + 1, i - j - 2), "string");
        }

        public override IEnumerable<int> FindSyntaxError(string str, int i, out ParserException exception)
        {
            if (IsMatch(str, ref i)) {
                exception = null;
                return new int[] { i };
            } else {
                exception = new StringExpectedException(str, i);
                return EmptyIndexArray;
            }
        }

        public override string ToString()
        {
            return "string";
        }
    }
}
