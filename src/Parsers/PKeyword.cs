using System;

namespace DUO2C.Parsers
{
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
        /// <param name="keyword">The keyword to match</param>
        public PKeyword(String keyword)
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
            return new LeafNode(Keyword, "keyword");
        }

        public override string ToString()
        {
            return "\"" + Keyword + "\"";
        }
    }
}
