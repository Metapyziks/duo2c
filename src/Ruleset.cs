using System;
using System.Collections.Generic;
using System.Linq;

using DUO2C.Parsers;

namespace DUO2C
{
    /// <summary>
    /// Contains a collection of production rules to be used when parsing a string.
    /// </summary>
    public class Ruleset : IEnumerable<KeyValuePair<PToken, Parser>>
    {
        /// <summary>
        /// Build a eBNF expression parser from a given collection of term lists.
        /// </summary>
        /// <param name="ruleset">Ruleset containing all known tokens</param>
        /// <param name="nodes">Collection of nodes to build an expression parser from</param>
        /// <returns>Constructed expression parser</returns>
        static Parser BuildExpr(Ruleset ruleset, IEnumerable<ParseNode> nodes)
        {
            Parser curr = null;
            foreach (var node in nodes) {
                // Ignore pipes, should be every second node
                if (node.Token == "keyword" && node.String == "|") continue;

                // Each expression is a collection of term lists
                var list = BuildList(ruleset, ((BranchNode) node).Children);

                if (curr == null) {
                    // First term list becomes the current
                    curr = list;
                } else {
                    // Chain together multiple term lists in a disjunction
                    curr = new EitherOrParser(curr, list);
                }
            }
            return curr;
        }

        /// <summary>
        /// Build an eBNF concatenated term list parser from a given list of terms.
        /// </summary>
        /// <param name="ruleset">Ruleset containing all known tokens</param>
        /// <param name="nodes">Collection of nodes to build a concatenated term list parser from</param>
        /// <returns>Constructed concatenated term list parser</returns>
        static Parser BuildList(Ruleset ruleset, IEnumerable<ParseNode> nodes)
        {
            Parser curr = null;
            foreach (var node in nodes) {
                // Build individual term
                var term = BuildTerm(ruleset, ((BranchNode) node).Children);

                if (curr == null) {
                    // First term list becomes the current
                    curr = term;
                } else if (term is OptionalParser && ((OptionalParser) term).Left == null) {
                    // If new term is optional, attach the previous terms to it
                    curr = new OptionalParser(curr, ((OptionalParser) term).Right);
                } else if (term is OptionalRepeatParser && ((OptionalRepeatParser) term).Left == null) {
                    // If new term is a repeated optional, attach the previous terms to it
                    curr = new OptionalRepeatParser(curr, ((OptionalRepeatParser) term).Right);
                } else {
                    // Concatenate new term with the previous ones
                    curr = new ConcatParser(curr, term);
                }
            }
            return curr;
        }

        /// <summary>
        /// Build an individual eBNF term parser from either a literal, a rule reference,
        /// or an expression surrounded by parentheses.
        /// </summary>
        /// <param name="ruleset">Ruleset containing all known tokens</param>
        /// <param name="nodes">Collection of nodes to build a term parser from</param>
        /// <returns>Constructed term parser</returns>
        static Parser BuildTerm(Ruleset ruleset, IEnumerable<ParseNode> nodes)
        {
            if (nodes.Count() == 1) {
                // If there is only one node, expect a literal or rule reference
                var node = (LeafNode) nodes.First();
                if (node.Token == "string") {
                    // String literals are keywords
                    return new PKeyword(node.String);
                } else if (node.Token == "ident") {
                    switch (node.String) {
                        // Check for various predefined parsers
                        case "string":
                            return new PString();
                        case "letter":
                            return new PLetter();
                        case "digit":
                            return new PDigit();
                        case "ident":
                            return new PIdent();

                        default:
                            // Otherwise, look for a matching rule
                            var rule = ruleset.GetTokenReference(node.String);
                            if (rule != null) return rule;

                            // If no matching rule is found, assume term is a keyword
                            return new PKeyword(node.String);
                    }
                } else {
                    // I can't think of anything that lead to here, but to be safe...
                    throw new NotImplementedException(node.Token + " : " + node.String);
                }
            } else {
                // If we are here, expect an expression inside parentheses
                var expr = BuildExpr(ruleset, ((BranchNode) nodes.ElementAt(1)).Children);
                if (nodes.First().String == "[") {
                    // Square braces surround an optional expression
                    return new OptionalParser(null, expr);
                } else if (nodes.First().String == "{") {
                    // Delorean braces surround a repeated optional expression
                    return new OptionalRepeatParser(null, expr);
                } else {
                    // Otherwise, assume the expression is surrounded by normal braces
                    return expr;
                }
            }
        }

        static Ruleset _sEBNFRuleset;

        /// <summary>
        /// Gets a ruleset describing the syntax of extended Backus-Naur Form.
        /// </summary>
        /// <returns>Ruleset describing the syntax of extended Backus-Naur Form</returns>
        private static Ruleset GetEBNFRuleset()
        {
            if (_sEBNFRuleset == null) {
                // If the ruleset hasn't been constructed yet, do so now
                _sEBNFRuleset = new Ruleset();

                // Define some simple parsers to use
                var ident = new PIdent();
                var str = new PString();

                // Define keywords
                var eq = new PKeyword("=");
                var fs = new PKeyword(".");
                var pipe = new PKeyword("|");
                var flat = new PKeyword("*");
                var pbOpen = new PKeyword("(");
                var pbClose = new PKeyword(")");
                var sbOpen = new PKeyword("[");
                var sbClose = new PKeyword("]");
                var cbOpen = new PKeyword("{");
                var cbClose = new PKeyword("}");
                
                // Define tokens
                var rSyntax = _sEBNFRuleset.CreateRuleToken("Syntax", false, true);
                var rRule = _sEBNFRuleset.CreateRuleToken("Rule");
                var rExpr = _sEBNFRuleset.CreateRuleToken("Expr");
                var rList = _sEBNFRuleset.CreateRuleToken("List");
                var rTerm = _sEBNFRuleset.CreateRuleToken("Term");

                // Define production rules
                _sEBNFRuleset.Add(rSyntax,  null *(+rRule));
                _sEBNFRuleset.Add(rRule,    +ident [+flat] +eq +rExpr +fs);
                _sEBNFRuleset.Add(rExpr,    +rList *(+pipe +rList));
                _sEBNFRuleset.Add(rList,    +rTerm *(+rTerm));
                _sEBNFRuleset.Add(rTerm,    (+pbOpen +rExpr +pbClose)
                                          | (+sbOpen +rExpr +sbClose)
                                          | (+cbOpen +rExpr +cbClose)
                                          | +str
                                          | +ident);
            }

            return _sEBNFRuleset;
        }

        /// <summary>
        /// Parses a Ruleset from a string containing a syntax definition
        /// in extended Backus-Naur Form.
        /// </summary>
        /// <param name="bnf">A string in extended Backus-Naur Form</param>
        /// <returns>A Ruleset representing the given syntax definition</returns>
        public static Ruleset FromString(String bnf)
        {
            // Parse the syntax structure
            var tree = (BranchNode) GetEBNFRuleset().Parse(bnf);
            var parsed = new Ruleset();

            // Prepare a list of rules to build after all tokens are declared
            var toBuild = new Dictionary<PToken, IEnumerable<ParseNode>>();

            bool first = true;
            foreach (var rule in tree) {
                var children = ((BranchNode) rule).Children;
                var name = children.First().String;

                // Tokens marked with a * are flattened into one node
                bool flatten = children.ElementAt(1).String == "*";
                var token = parsed.CreateRuleToken(name, flatten, first);

                // If the token is marked to be flattened, the expression will
                // be one more node along in the array
                int exprIndex = flatten ? 3 : 2;
                toBuild.Add(token, ((BranchNode) children.ElementAt(exprIndex)).Children);
                first = false;
            }

            // Build rules
            foreach (var rule in toBuild) {
                parsed.Add(rule.Key, BuildExpr(parsed, rule.Value));
            }

            return parsed;
        }

        Parser _root;
        Dictionary<PToken, Parser> _dict;

        /// <summary>
        /// Constructor to create an empty Ruleset.
        /// </summary>
        public Ruleset()
        {
            _dict = new Dictionary<PToken, Parser>();
        }

        /// <summary>
        /// Attempt to parse the structure of a string using this Ruleset.
        /// </summary>
        /// <param name="str">String to parse the structure of</param>
        /// <returns>Node tree representing the string's structure</returns>
        public ParseNode Parse(String str)
        {
            int i = 0, j = 0;
            if (_root.IsMatch(str, ref j)) {
                // If the string is valid, parse it
                var tree = _root.Parse(str, ref i);
                return tree;
            } else {
                // TODO: Add some kind of error reporting
                return null;
            }
        }

        /// <summary>
        /// Declares a token that will have one or more production rules defined for it.
        /// </summary>
        /// <param name="token">String used to refer to the associated token</param>
        /// <param name="flatten">If true, the parsed structures produced for this token
        /// are flattened to a single leaf node</param>
        /// <param name="root">If true, this token is used as the initial production rule</param>
        /// <returns>Parser that refers to this token</returns>
        public PToken CreateRuleToken(String token, bool flatten = false, bool root = false)
        {
            var rule = new PToken(this, token, flatten);
            _dict.Add(rule, null);
            if (root) _root = rule;
            return rule;
        }

        /// <summary>
        /// Adds a production rule assigned to a given token.
        /// </summary>
        /// <param name="token">Token to assign the new production rule to</param>
        /// <param name="parser">Parser describing the production rule</param>
        public void Add(PToken token, Parser parser)
        {
            var prev = _dict[token];
            if (prev == null) {
                _dict[token] = parser;
            } else {
                // If a production already exists, combine them in a disjunction
                _dict[token] = new EitherOrParser(prev, parser);
            }
        }

        /// <summary>
        /// Gets the token parser corresponding to the given token name.
        /// </summary>
        /// <param name="name">Token name to find a matching token parser for</param>
        /// <returns>A token parser corresponding to the given token name, or
        /// null if one isn't found</returns>
        public PToken GetTokenReference(String name)
        {
            return _dict.Keys.FirstOrDefault(x => x.Token == name);
        }

        /// <summary>
        /// Gets the parser that corresponds with the given token.
        /// </summary>
        /// <param name="token">Token to find a matching parser from</param>
        /// <returns>Parser that corresponds with the given token</returns>
        public Parser GetReferencedParser(PToken token)
        {
            return _dict[token];
        }

        public IEnumerator<KeyValuePair<PToken, Parser>> GetEnumerator()
        {
            return _dict.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _dict.GetEnumerator();
        }
    }
}
