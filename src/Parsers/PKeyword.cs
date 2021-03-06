﻿using System;
using System.Collections.Generic;

using DUO2C.Nodes;

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
        /// <param name="ruleset">The ruleset that will contain this parser</param>
        /// <param name="keyword">The keyword to match</param>
        public PKeyword(Ruleset ruleset, String keyword)
            : base(ruleset)
        {
            Keyword = keyword;
        }

        public override bool IsMatch(string str, ref int i, bool whitespace)
        {
            int init = i;
            if (whitespace) SkipWhitespace(str, ref i);
            for (int j = 0; j < Keyword.Length; ++i, ++j) {
                if (i >= str.Length || str[i] != Keyword[j]) {
                    i = init; return false;
                }
            }

            if (whitespace && i < str.Length && char.IsLetterOrDigit(str[i - 1]) && char.IsLetterOrDigit(str[i])) {
                // If the word being parsed hasn't ended, reject
                i = init; return false;
            }
            return true;
        }

        public override IEnumerable<ParseNode> Parse(string str, int i, bool whitespace, out CompilerException exception)
        {
            if (IsMatch(str, ref i, whitespace)) {
                exception = null;
                return new ParseNode[] { new LeafNode(i - Keyword.Length, Keyword.Length, Keyword, "keyword") };
            } else {
                exception = new SymbolExpectedException(String.Format("'{0}'", Keyword), i, 1);
                return EmptyNodeArray;
            }
        }

        public override string ToString()
        {
            return "\"" + Keyword + "\"";
        }
    }
}
