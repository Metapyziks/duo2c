using System;

namespace DUO2C.Parsers
{
    /// <summary>
    /// Parser that references a token that has one or more
    /// production rules assigned to it.
    /// </summary>
    public class PToken : Parser
    {
        /// <summary>
        /// Token name associated with a set of production rules.
        /// </summary>
        public String Token { get; private set; }

        /// <summary>
        /// Parsed structures will be flattened into a single leaf
        /// node if this value is true.
        /// </summary>
        public bool Flatten { get; private set; }

        private Ruleset _ruleset;
        private Parser _parser;

        /// <summary>
        /// Property that hides retrieving the parser referenced by
        /// this token.
        /// </summary>
        private Parser Parser
        {
            get {
                if (_parser == null) {
                    _parser = _ruleset.GetReferencedParser(this);
                }

                return _parser;
            }
        }

        /// <summary>
        /// Event used for debugging purposes when this token is about
        /// to be tested for a match.
        /// </summary>
        public event EventHandler MatchTested;

        /// <summary>
        /// Event used for debugging purposes when this token is about
        /// to be parsed.
        /// </summary>
        public event EventHandler Parsed;

        /// <summary>
        /// Constructor to create a new token reference parser.
        /// </summary>
        /// <param name="ruleset">Ruleset that represents the grammar that
        /// contains this token</param>
        /// <param name="token">Name of the token to be referenced</param>
        /// <param name="flatten">Parsed structures will be flattened into
        /// a single leaf node if this value is true</param>
        public PToken(Ruleset ruleset, String token, bool flatten)
        {
            _parser = null;

            _ruleset = ruleset;
            Token = token;
            Flatten = flatten;
        }

        public override bool IsMatch(string str, ref int i)
        {
            // Trigger matching event
            if (MatchTested != null) MatchTested(this, new EventArgs());

            int init = i;
            SkipWhitespace(str, ref i);
            if (Parser.IsMatch(str, ref i)) return true;
            i = init; return false;
        }

        public override ParseNode Parse(string str, ref int i)
        {
            // Trigger parsing event
            if (Parsed != null) Parsed(this, new EventArgs());

            SkipWhitespace(str, ref i);
            var tree = Parser.Parse(str, ref i);
            if (Flatten) {
                return new LeafNode(tree.String, Token);
            } else if (tree.Token == null) {
                // If the produced node has no token, use the
                // this token name
                tree.Token = Token;
                return tree;
            } else {
                // Otherwise, encapsulate with a new node with this
                // token name
                return new BranchNode(new ParseNode[] { tree }, Token);
            }
        }

        public override string ToString()
        {
            return Token.ToString();
        }
    }
}
