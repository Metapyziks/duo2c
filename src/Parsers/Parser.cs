using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace DUO2C.Parsers
{
    public abstract class Parser : IEnumerable<Parser>
    {
        protected static void SkipWhitespace(String str, ref int i)
        {
            while (i < str.Length) {
                if (char.IsWhiteSpace(str[i])) {
                    ++i;
                } else if (i < str.Length - 3 && str[i] == '(' && str[i + 1] == '*') {
                    int init = i;
                    SkipComment(str, ref i);
                    if (init == i) return;
                } else {
                    return;
                }
            }
        }

        static Regex _sCommentPattern = new Regex("\\(\\*.*\\*\\)");
        static void SkipComment(String str, ref int i)
        {
            var match = _sCommentPattern.Match(str, i);
            if (match.Success && match.Index == i) i += match.Length;
        }

        public abstract bool IsMatch(String str, ref int i);
        public abstract ParseNode Parse(String str, ref int i);

        public static Parser operator +(Parser parser)
        {
            return parser;
        }

        public static Parser operator +(Parser left, Parser right)
        {
            return new ConcatParser(left, right);
        }

        public static Parser operator |(Parser left, Parser right)
        {
            return new EitherOrParser(left, right);
        }

        public Parser this[Parser parser]
        {
            get
            {
                return new OptionalParser(this, parser);
            }
        }

        public static Parser operator *(Parser left, Parser right)
        {
            return new OptionalRepeatParser(left, right);
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
