using System;
using System.Collections.Generic;

namespace DUO2C.Parsers
{
    /// <summary>
    /// Exception thrown when a keyword is expected but not found.
    /// </summary>
    [ExceptionUtility(20)]
    public class KeywordExpectedException : ParserException
    {
        /// <summary>
        /// The keyword that was expected.
        /// </summary>
        public String Keyword { get; private set; }

        /// <summary>
        /// Constructor to create a new keyword expected exception, containing
        /// information about the location in the source string that the exception
        /// occurred, and the keyword that was expected.
        /// </summary>
        /// <param name="keyword">The keyword that was expected</param>
        /// <param name="str">The source string being parsed</param>
        /// <param name="index">Start index in the source string of the exception</param>
        public KeywordExpectedException(String keyword, String str, int index)
            : base(String.Format("Expected \"{0}\"", keyword), str, index)
        {
            Keyword = keyword;
        }
    }

    /// <summary>
    /// Atomic parser that parses a specified keyword.
    /// </summary>
    public class PKeyword : Parser
    {
        /// <summary>
        /// Keyword this parser matches.
        /// </summary>
        public String Keyword { get; private set; }

        /// <summary>
        /// Creates a new keyword parser matching a specified keyword.
        /// </summary>
        /// <param name="ruleset">The ruleset that will contain this parser</param>
        /// <param name="keyword">The keyword to match</param>
        public PKeyword(Ruleset ruleset, String keyword)
            : base(ruleset)
        {
            Keyword = keyword;
        }

        public override bool IsMatch(string str, ref int i)
        {
            int init = i;
            SkipWhitespace(str, ref i);
            for (int j = 0; j < Keyword.Length; ++i, ++j) {
                if (i >= str.Length || str[i] != Keyword[j]) {
                    i = init; return false;
                }
            }

            if (i < str.Length && char.IsLetterOrDigit(str[i - 1]) && char.IsLetterOrDigit(str[i])) {
                // If the word being parsed hasn't ended, reject
                i = init; return false;
            }
            return true;
        }

        public override ParseNode Parse(string str, ref int i)
        {
            SkipWhitespace(str, ref i);
                
            i += Keyword.Length;

            return new LeafNode(i - Keyword.Length, Keyword.Length, Keyword, "keyword");
        }

        public override IEnumerable<int> FindSyntaxError(string str, int i, out ParserException exception)
        {
            if (IsMatch(str, ref i)) {
                exception = null;
                return new int[] { i };
            } else {
                exception = new KeywordExpectedException(Keyword, str, i);
                return EmptyIndexArray;
            }
        }

        public override string ToString()
        {
            return "\"" + Keyword + "\"";
        }
    }
}
