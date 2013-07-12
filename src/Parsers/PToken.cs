﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using DUO2C.Nodes;

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
        /// <param name="index">Start index in the source string of the exception</param>
        public TokenExpectedException(String token, int index)
            : base(ParserError.Syntax, String.Format("{0} expected", token), index)
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
            int init = i;
            if (IgnoreWhitespace) SkipWhitespace(str, ref i);
            if (Parser.IsMatch(str, ref i, IgnoreWhitespace)) return true;
            i = init; return false;
        }

        public override IEnumerable<ParseNode> Parse(string str, int i, bool whitespace, out ParserException exception)
        {
            if (IgnoreWhitespace) SkipWhitespace(str, ref i);
            var nodes = Parser.Parse(str, i, IgnoreWhitespace, out exception);
            if (nodes.Count() == 0) {
                exception = ChooseParserException(exception, new TokenExpectedException(Token, i));
            }
            var except = exception;
            nodes = nodes.Select(node => {
                if (Flatten) {
                    node = new LeafNode(node.StartIndex, node.Length, node.String, Token);
                } else if (node.Token == null) {
                    // If the produced node has no token, use the
                    // this token name
                    node.Token = Token;
                } else {
                    // Otherwise, encapsulate with a new node with this
                    // token name
                    node = new BranchNode(new ParseNode[] { node }, Token);
                }

                try {
                    return Ruleset.GetSubstitution(node);
                } catch (ParserException e) {
                    except = ChooseParserException(except, e);
                    return null;
                }
            }).Where(x => x != null).ToArray();
            exception = except;
            return nodes;
        }

        public override string ToString()
        {
            return Token.ToString();
        }
    }
}
