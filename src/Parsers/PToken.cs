﻿using System;
using System.Linq;
using System.Collections.Generic;

namespace DUO2C.Parsers
{
    /// <summary>
    /// Exception thrown when a specific token is expected but not found.
    /// </summary>
    [ExceptionUtility(100)]
    public class TokenExpectedException : ParserException
    {
        /// <summary>
        /// The token that was expected.
        /// </summary>
        public String Token { get; private set; }

        /// <summary>
        /// Constructor to create a new digit expected exception, containing
        /// information about the location in the source string that the exception
        /// occurred.
        /// </summary>
        /// <param name="str">The source string being parsed</param>
        /// <param name="index">Start index in the source string of the exception</param>
        public TokenExpectedException(String token, String str, int index)
            : base(String.Format("{0} expected", token), str, index)
        {
            Token = token;
        }
    }

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

        /// <summary>
        /// If true, leading whitespace will be ignored when matching
        /// this token.
        /// </summary>
        public bool IgnoreWhitespace
        {
            get { return Token.Length > 0 && char.IsUpper(Token[0]); }
        }

        private Parser _parser;

        /// <summary>
        /// Property that hides retrieving the parser referenced by
        /// this token.
        /// </summary>
        private Parser Parser
        {
            get {
                if (_parser == null) {
                    _parser = Ruleset.GetReferencedParser(this);
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
            : base(ruleset)
        {
            _parser = null;

            Token = token;
            Flatten = flatten;
        }

        public override bool IsMatch(string str, ref int i, bool whitespace)
        {
            // Trigger matching event
            if (MatchTested != null) MatchTested(this, new EventArgs());

            int init = i;
            if (IgnoreWhitespace) SkipWhitespace(str, ref i);
            if (Parser.IsMatch(str, ref i, IgnoreWhitespace)) return true;
            i = init; return false;
        }

        public override ParseNode Parse(string str, ref int i, bool whitespace)
        {
            // Trigger parsing event
            if (Parsed != null) Parsed(this, new EventArgs());

            if (IgnoreWhitespace) SkipWhitespace(str, ref i);
            var tree = Parser.Parse(str, ref i, IgnoreWhitespace);
            if (Flatten) {
                return new LeafNode(tree.SourceIndex, tree.Length, tree.String, Token);
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

        public override IEnumerable<int> FindSyntaxError(string str, int i, bool whitespace, out ParserException exception)
        {
            if (IgnoreWhitespace) SkipWhitespace(str, ref i);
            var indices = Parser.FindSyntaxError(str, i, IgnoreWhitespace, out exception);
            if (indices.Count() == 0) {
                exception = ChooseParserException(exception, new TokenExpectedException(Token, str, i));
            }
            return indices;
        }

        public override string ToString()
        {
            return Token.ToString();
        }
    }
}
