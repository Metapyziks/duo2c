using System;
using System.Collections.Generic;

using DUO2C.Nodes;

namespace DUO2C.Parsers
{
    /// <summary>
    /// Atomic parser that parses an identifier.
    /// </summary>
    public class PIdent : Parser
    {
        /// <summary>
        /// Constructor to create a new identifier parser.
        /// </summary>
        /// <param name="ruleset">The ruleset that will contain this parser</param>
        public PIdent(Ruleset ruleset)
            : base(ruleset) { }

        public override bool IsMatch(string str, ref int i, bool whitespace)
        {
            int init = i;
            if (whitespace) SkipWhitespace(str, ref i);
            int j = 0;
            while (i < str.Length && (char.IsLetter(str[i]) || (j > 0 && char.IsDigit(str[i])))) {
                ++i; ++j;
            }
            if (j > 0 && !Ruleset.IsKeyword(str.Substring(init, j))) return true;
            i = init; return false;
        }

        public override IEnumerable<ParseNode> Parse(string str, int i, bool whitespace, out CompilerException exception)
        {
            int j = i;
            if (IsMatch(str, ref i, whitespace)) {
                exception = null;
                try {
                    return new ParseNode[] { Ruleset.GetSubstitution(new LeafNode(j, i - j, str.Substring(j, i - j), "ident")) };
                } catch (CompilerException e) {
                    exception = e;
                    return EmptyNodeArray;
                }
            } else {
                exception = new SymbolExpectedException("Identifier", i, 1);
                return EmptyNodeArray;
            }
        }

        public override string ToString()
        {
            return "ident";
        }
    }
}
