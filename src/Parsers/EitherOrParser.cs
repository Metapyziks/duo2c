using System.Collections.Generic;
using System.Linq;

using DUO2C.Nodes;

namespace DUO2C.Parsers
{
    /// <summary>
    /// Parser that attempts two parsers, selecting the first
    /// to match.
    /// </summary>
    public class EitherOrParser : BinaryParser
    {
        /// <summary>
        /// Constructor to create a new EitherOrParser.
        /// </summary>
        /// <param name="ruleset">The ruleset that will contain this parser</param>
        /// <param name="left">Parser to be attempted first</param>
        /// <param name="right">Parser to be attempted second</param>
        public EitherOrParser(Ruleset ruleset, Parser left, Parser right)
            : base(ruleset, left, right) { }

        public override bool IsMatch(string str, ref int i, bool whitespace)
        {
            int init = i;
            if (Left.IsMatch(str, ref i, whitespace)) return true;

            // Reset index before attempting second parser
            i = init;
            if (Right.IsMatch(str, ref i, whitespace)) return true;
            i = init; return false;
        }

        public override IEnumerable<ParseNode> Parse(string str, int i, bool whitespace, out ParserException exception)
        {
            ParserException right;
            var nodes = Left.Parse(str, i, whitespace, out exception)
                .Union(Right.Parse(str, i, whitespace, out right));
            exception = ChooseParserException(exception, right);
            return nodes;
        }

        public override string ToString()
        {
            return Left.ToString() + " | " + Right.ToString();
        }
    }
}
