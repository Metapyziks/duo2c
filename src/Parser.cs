using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace DUO2C
{
    abstract class Parser : IEnumerable<Parser>
    {
        public static ParseNode Parse(String str, Ruleset ruleset)
        {
            int i = 0, j = 0;
            var parser = ruleset.RootParser;
            if (parser.IsMatch(str, ref j)) {
                var tree = parser.Parse(str, ref i);
                return tree;
            } else {
                return null;
            }
        }

        protected static void SkipWhitespace(String str, ref int i)
        {
            while (i < str.Length && char.IsWhiteSpace(str[i])) ++i;
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

        public Parser this[Parser parser] {
            get {
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

    abstract class BinaryParser : Parser
    {
        public Parser Left { get; private set; }
        public Parser Right { get; private set; }

        public BinaryParser(Parser left, Parser right)
        {
            Left = left;
            Right = right;
        }
    }

    class ConcatParser : BinaryParser
    {
        public ConcatParser(Parser left, Parser right)
            : base(left, right) { }

        public override bool IsMatch(string str, ref int i)
        {
            int init = i;
            bool left = Left.IsMatch(str, ref i);
            if (left) {
                bool right = Right.IsMatch(str, ref i);
                if (right) return true;
            }
            i = init; return false;
        }

        public override ParseNode Parse(string str, ref int i)
        {
            var left = Left.Parse(str, ref i);
            var right = Right.Parse(str, ref i);
            if (left == null) return right;
            if (right == null) return left;

            if (left is BranchNode && left.Token == null) {
                return new BranchNode(((BranchNode) left).Children.Concat(new ParseNode[] { right }));
            } else {
                return new BranchNode(new ParseNode[] { left, right });
            }
        }

        public override string ToString()
        {
            return Left.ToString() + " " + Right.ToString();
        }
    }

    class OptionalParser : BinaryParser
    {
        public OptionalParser(Parser left, Parser right)
            : base(left, right) { }

        public override bool IsMatch(string str, ref int i)
        {
            int init = i;
            if (Left == null || Left.IsMatch(str, ref i)) {
                init = i;
                if (Right.IsMatch(str, ref i)) return true;
                i = init; return true;
            }
            i = init; return false;
        }

        public override ParseNode Parse(string str, ref int i)
        {
            var left = (Left == null) ? null : Left.Parse(str, ref i);

            int j = i;
            if (!Right.IsMatch(str, ref j)) {
                return left;
            }

            var right = Right.Parse(str, ref i);
            if (left == null) return right;

            if (right is BranchNode && right.Token == null) {
                return new BranchNode(new ParseNode[] { left }.Concat(((BranchNode) right).Children));
            } else {
                return new BranchNode(new ParseNode[] { left, right });
            }
        }

        public override string ToString()
        {
            return (Left != null ? Left.ToString() : "") + " [" + Right.ToString() + "]";
        }
    }

    class OptionalRepeatParser : BinaryParser
    {
        public OptionalRepeatParser(Parser left, Parser right)
            : base(left, right) { }

        public override bool IsMatch(string str, ref int i)
        {
            int init = i;
            if (Left == null || Left.IsMatch(str, ref i)) {
                init = i;
                while (Right.IsMatch(str, ref i)) init = i;
                i = init; return true;
            }
            i = init; return false;
        }

        public override ParseNode Parse(string str, ref int i)
        {
            var left = Left == null ? null : Left.Parse(str, ref i);

            var right = new List<ParseNode>();
            
            int init = i;
            while (Right.IsMatch(str, ref i))
            {
                i = init;
                right.Add(Right.Parse(str, ref i));
                init = i;
            }

            if (left == null) return new BranchNode(right);

            if (left is BranchNode && left.Token == null) {
                return new BranchNode(((BranchNode) left).Children.Concat(right));
            } else {
                return new BranchNode(new ParseNode[] { left }.Concat(right));
            }
        }

        public override string ToString()
        {
            return (Left != null ? Left.ToString() : "") + " {" + Right.ToString() + "}";
        }
    }

    class EitherOrParser : BinaryParser
    {
        public EitherOrParser(Parser left, Parser right)
            : base(left, right) { }

        public override bool IsMatch(string str, ref int i)
        {
            int init = i;
            if (Left.IsMatch(str, ref i)) return true;
            i = init;
            if (Right.IsMatch(str, ref i)) return true;
            i = init; return false;
        }

        public override ParseNode Parse(string str, ref int i)
        {
            int j = i;
            if (Left.IsMatch(str, ref j)) {
                return Left.Parse(str, ref i);
            } else {
                return Right.Parse(str, ref i);
            }
        }

        public override string ToString()
        {
            return Left.ToString() + " | " + Right.ToString();
        }
    }

    class PKeyword : Parser
    {
        public String Keyword { get; private set; }

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

    class PString : Parser
    {
        public override bool IsMatch(string str, ref int i)
        {
            int init = i;
            SkipWhitespace(str, ref i);
            if (i < str.Length && (str[i] == '"' || str[i] == '\'')) {
                char startChar = str[i];
                while (++i < str.Length) {
                    if (str[i] == '\\') {
                        ++ i;
                    }
                    if (str[i] == startChar) {
                        ++ i;
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
            return new LeafNode(str.Substring(j + 1, i - j - 2), "string");
        }

        public override string ToString()
        {
            return "string";
        }
    }

    class PLetter : Parser
    {
        public override bool IsMatch(string str, ref int i)
        {
            if (i < str.Length && char.IsLetter(str[i])) {
                ++i; return true; 
            }
            return false;
        }

        // TODO: Add proper support for escape characters
        public override ParseNode Parse(string str, ref int i)
        {
            return new LeafNode(str[i++].ToString(), "letter");
        }

        public override string ToString()
        {
            return "letter";
        }
    }

    class PDigit : Parser
    {
        public override bool IsMatch(string str, ref int i)
        {
            if (i < str.Length && char.IsDigit(str[i])) {
                ++i; return true;
            }
            return false;
        }

        // TODO: Add proper support for escape characters
        public override ParseNode Parse(string str, ref int i)
        {
            return new LeafNode(str[i++].ToString(), "digit");
        }

        public override string ToString()
        {
            return "digit";
        }
    }

    class PIdent : Parser
    {
        public override bool IsMatch(string str, ref int i)
        {
            int init = i;
            SkipWhitespace(str, ref i);
            int j = 0;
            while (i < str.Length && (char.IsLetter(str[i]) || (j > 0 && char.IsDigit(str[i])))) {
                ++i; ++j;
            }
            if (j > 0) return true;
            i = init; return false;
        }

        public override ParseNode Parse(string str, ref int i)
        {
            SkipWhitespace(str, ref i);
            int j = i;
            IsMatch(str, ref i);
            return new LeafNode(str.Substring(j, i - j), "ident");
        }

        public override string ToString()
        {
            return "ident";
        }
    }

    class PRule : Parser
    {
        public String Token { get; private set; }
        public bool Flatten { get; private set; }

        private Ruleset _ruleset;
        private Parser _parser;

        private Parser Parser
        {
            get
            {
                if (_parser == null) {
                    _parser = _ruleset.GetParser(this);
                }

                return _parser;
            }
        }

        public event EventHandler MatchTested;
        public event EventHandler Parsed;

        public PRule(Ruleset ruleset, String token, bool flatten)
        {
            _ruleset = ruleset;
            _parser = null;
            Token = token;
            Flatten = flatten;
        }

        public override bool IsMatch(string str, ref int i)
        {
            if (MatchTested != null) MatchTested(this, new EventArgs());

            int init = i;
            SkipWhitespace(str, ref i);
            if (Parser.IsMatch(str, ref i)) return true;
            i = init; return false;
        }

        public override ParseNode Parse(string str, ref int i)
        {
            if (Parsed != null) Parsed(this, new EventArgs());

            SkipWhitespace(str, ref i);
            var tree = Parser.Parse(str, ref i);
            if (Flatten) {
                return new LeafNode(tree.String, Token);
            } else if (tree.Token == null) {
                tree.Token = Token;
                return tree;
            } else {
                return new BranchNode(new ParseNode[] { tree }, Token);
            }
        }

        public override string ToString()
        {
            return Token.ToString();
        }
    }
}
