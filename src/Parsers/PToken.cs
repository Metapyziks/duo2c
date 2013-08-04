using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using DUO2C.Nodes;

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

        /// <summary>
        /// Parsed structures won't be substituted if this value is true.
        /// </summary>
        public bool NoSubstitution { get; private set; }

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
        /// <param name="nosub">Parsed structures won't be substituted if
        /// this value is true</param>
        public PToken(Ruleset ruleset, String token, bool flatten, bool nosub)
            : base(ruleset)
        {
            _parser = null;

            Token = token;
            Flatten = flatten;
            NoSubstitution = nosub;
        }

        public override bool IsMatch(string str, ref int i, bool whitespace)
        {
            int init = i;
            if (IgnoreWhitespace) SkipWhitespace(str, ref i);
            if (Parser.IsMatch(str, ref i, IgnoreWhitespace)) return true;
            i = init; return false;
        }

        public override IEnumerable<ParseNode> Parse(string str, int i, bool whitespace, out CompilerException exception)
        {
            if (IgnoreWhitespace) SkipWhitespace(str, ref i);
            var nodes = Parser.Parse(str, i, IgnoreWhitespace, out exception);
            if (nodes.Count() == 0) {
                exception = ChooseParserException(exception, new SymbolExpectedException(Token, i, 0));
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
                } catch (CompilerException e) {
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
