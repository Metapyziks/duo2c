using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace DUO2C.Parsers
{
    /// <summary>
    /// Abstract class for a parser that decides if the next section
    /// of a given string matches a certain format, and produces a
    /// ParseNode for such matching strings.
    /// </summary>
    public abstract class Parser : IEnumerable<Parser>
    {
        protected class ParserExceptionWrapper
        {
            public ParserException Payload = null;
        }

        private static IEnumerable<int> _sEmptyIndexArray = new int[0];

        /// <summary>
        /// Utility function to ignore whitespace and comments.
        /// </summary>
        /// <param name="str">String being parsed</param>
        /// <param name="i">Current index</param>
        protected static void SkipWhitespace(String str, ref int i)
        {
            while (i < str.Length) {
                if (char.IsWhiteSpace(str[i])) {
                    ++i;
                } else if (i < str.Length - 3 && str[i] == '(' && str[i + 1] == '*') {
                    // Comments are prefixed with "(*" and end with "*)"
                    int init = i;
                    SkipComment(str, ref i);

                    // Abort if no comments were skipped
                    if (init == i) return;
                } else {
                    // If we haven't moved anywhere, abort
                    return;
                }
            }
        }

        static Regex _sCommentPattern = new Regex("\\(\\*.*\\*\\)");

        /// <summary>
        /// Utility function to ignore the next immediate comment.
        /// </summary>
        /// <param name="str">String being parsed</param>
        /// <param name="i">Current index</param>
        static void SkipComment(String str, ref int i)
        {
            var match = _sCommentPattern.Match(str, i);
            if (match.Success && match.Index == i) i += match.Length;
        }

        /// <summary>
        /// The ruleset that will contain this parser.
        /// </summary>
        protected Ruleset Ruleset { get; private set; }

        /// <summary>
        /// Abstract constructor to create a new Parser that will be contained
        /// within a given ruleset.
        /// </summary>
        /// <param name="ruleset">The ruleset that will contain this parser</param>
        public Parser(Ruleset ruleset)
        {
            Ruleset = ruleset;
        }

        /// <summary>
        /// Decides if the next immediate symbol in the given string matches the
        /// format specified by this parser. If a match is found, <paramref name="i"/>
        /// will be increased to point to the end of the match.
        /// </summary>
        /// <param name="str">String being parsed</param>
        /// <param name="i">Current index</param>
        /// <returns>True if the next symbol matches this parser's format</returns>
        public abstract bool IsMatch(String str, ref int i);

        /// <summary>
        /// Parses the next immediate symbol from the given string, assuming it
        /// matches the parser's format, and increases <paramref name="i"/> to point
        /// to the end of the match.
        /// </summary>
        /// <param name="str">String being parsed</param>
        /// <param name="i">Current index</param>
        /// <returns>Node representing the symbol parsed from the string</returns>
        public abstract ParseNode Parse(String str, ref int i);

        /// <summary>
        /// Attempts to find the first syntax error encountered using this parser
        /// from the given index.
        /// </summary>
        /// <param name="str">String being parsed</param>
        /// <param name="i">Current index</param>
        /// <param name="wrapper">Container for an exception if it is encountered</param>
        /// <returns>First syntax error found, if any. Otherwise, null.</returns>
        protected abstract IEnumerable<int> FindSyntaxError(String str, int i, ParserExceptionWrapper wrapper);

        /// <summary>
        /// Attempts to find the first syntax error encountered using this parser
        /// from the given index. Also returns an enumerable containing all possible
        /// valid positions the parser may be at after parsing.
        /// </summary>
        /// <param name="str">String being parsed</param>
        /// <param name="i">Current index</param>
        /// <param name="exception">Outputted exception</param>
        /// <returns>Enumerable over all valid indices.</returns>
        public IEnumerable<int> FindSyntaxError(String str, int i, out ParserException exception)
        {
            ParserExceptionWrapper wrapper = new ParserExceptionWrapper();
            var indices = FindSyntaxError(str, i, wrapper).ToArray() ?? _sEmptyIndexArray;
            exception = wrapper.Payload;
            if (exception == null && indices.Count() == 0) {
                System.Diagnostics.Debugger.Break();
            }
            return indices;
        }

        /// <summary>
        /// Literally does nothing, used for aesthetics.
        /// </summary>
        /// <param name="parser">Parser to do nothing with</param>
        /// <returns>The inputted parser</returns>
        public static Parser operator +(Parser parser)
        {
            return parser;
        }

        /// <summary>
        /// Concatenates two parsers to produce a single parser.
        /// </summary>
        /// <param name="left">Left parser to be parsed first</param>
        /// <param name="right">Right parser to be parsed second</param>
        /// <returns>A combined concatenation parser</returns>
        public static Parser operator +(Parser left, Parser right)
        {
            return new ConcatParser(left.Ruleset, left, right);
        }

        /// <summary>
        /// Produces an either-or parser using two given parsers. The left
        /// parser is tested for a match first, and if one isn't found the
        /// second parser is tested.
        /// </summary>
        /// <param name="left">Left parser to be attempted first</param>
        /// <param name="right">Right parser to be attempted second</param>
        /// <returns>A combined either-or parser</returns>
        public static Parser operator |(Parser left, Parser right)
        {
            return new EitherOrParser(left.Ruleset, left, right);
        }

        /// <summary>
        /// Produces a parser which requires the parser outside of the braces
        /// to match, but leaves the parser inside the braces as optional.
        /// </summary>
        /// <param name="parser">Parser to be added as optional</param>
        /// <returns>A combined right-optional parser</returns>
        public Parser this[Parser parser]
        {
            get { return new OptionalParser(Ruleset, this, parser); }
        }

        /// <summary>
        /// Produces a parser which requires the parser on the left to match,
        /// but leaves the parser on the right as optional. Additionally, multiple
        /// matches of the right parser are allowed.
        /// </summary>
        /// <param name="left">Parser that must match</param>
        /// <param name="right">Parser that may match zero or more times</param>
        /// <returns>A combined right-optional-repeat parser</returns>
        public static Parser operator *(Parser left, Parser right)
        {
            return new OptionalRepeatParser((left ?? right).Ruleset, left, right);
        }

        public IEnumerator<Parser> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
