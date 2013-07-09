using System;
using System.Collections.Generic;

namespace DUO2C.Parsers
{
    /// <summary>
    /// Exception thrown when an identifier is expected but not found.
    /// </summary>
    [ExceptionUtility(30)]
    public class IdentifierExpectedException : ParserException
    {
        /// <summary>
        /// Constructor to create a new digit expected exception, containing
        /// information about the location in the source string that the exception
        /// occurred.
        /// </summary>
        /// <param name="str">The source string being parsed</param>
        /// <param name="index">Start index in the source string of the exception</param>
        public IdentifierExpectedException(String str, int index)
            : base("Identifier expected", str, index) { }
    }

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

        public override bool IsMatch(string str, ref int i)
        {
            int init = i;
            SkipWhitespace(str, ref i);
            int j = 0;
            while (i < str.Length && (char.IsLetter(str[i]) || (j > 0 && char.IsDigit(str[i])))) {
                ++i; ++j;
            }
            if (j > 0 && !Ruleset.IsKeyword(str.Substring(init, j))) return true;
            i = init; return false;
        }

        public override ParseNode Parse(string str, ref int i)
        {
            SkipWhitespace(str, ref i);
            int j = i;
            IsMatch(str, ref i);
            return new LeafNode(j, i - j, str.Substring(j, i - j), "ident");
        }

        public override IEnumerable<int> FindSyntaxError(string str, int i, out ParserException exception)
        {
            if (IsMatch(str, ref i)) {
                exception = null;
                return new int[] { i };
            } else {
                exception = new IdentifierExpectedException(str, i);
                return EmptyIndexArray;
            }
        }

        public override string ToString()
        {
            return "ident";
        }
    }
}
