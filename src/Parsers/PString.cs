﻿using System;
using System.Collections.Generic;

using DUO2C.Nodes;

namespace DUO2C.Parsers
{
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

        public override bool IsMatch(string str, ref int i, bool whitespace)
        {
            int init = i;
            if (whitespace) SkipWhitespace(str, ref i);
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
        public override IEnumerable<ParseNode> Parse(string str, int i, bool whitespace, out CompilerException exception)
        {
            int j = i;
            if (IsMatch(str, ref i, whitespace)) {
                exception = null;
                try {
                    return new ParseNode[] { Ruleset.GetSubstitution(new LeafNode(j, i - j, str.Substring(j, i - j), "string")) };
                } catch (CompilerException e) {
                    exception = e;
                    return EmptyNodeArray;
                }
            } else {
                exception = new SymbolExpectedException("StringLiteral", i, 1);
                return EmptyNodeArray;
            }
        }

        public override string ToString()
        {
            return "string";
        }
    }
}
