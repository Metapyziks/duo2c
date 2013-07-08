using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUO2C.Parsers
{
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
