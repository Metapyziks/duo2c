﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

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
                    curr = new EitherOrParser(ruleset, curr, list);
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
            foreach (var node in nodes.Reverse()) {
                // Build individual term
                var term = BuildTerm(ruleset, ((BranchNode) node).Children);

                if (curr == null) {
                    // First term list becomes the current
                    curr = term;
                    continue;
                }
                
                if (curr is ConcatParser) {
                    var left = ((BinaryParser) curr).Left as BinaryParser;
                    var right = ((BinaryParser) curr).Right;
                    Parser combined = null;

                    if (left is OptionalParser && left.Left == null) {
                        combined = new OptionalParser(ruleset, term, left.Right);
                    } else if (left is OptionalRepeatParser && left.Left == null) {
                        combined = new OptionalRepeatParser(ruleset, term, left.Right);
                    }

                    if (combined != null) {
                        curr = new ConcatParser(ruleset, combined, right);
                        continue;
                    }
                }

                // Concatenate new term with the previous ones
                curr = new ConcatParser(ruleset, term, curr);
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
                    return ruleset.CreateKeywordParser(node.String);
                } else if (node.Token == "ident") {
                    switch (node.String) {
                        // Check for various predefined parsers
                        case "string":
                            return ruleset.CreateStringParser();
                        case "letter":
                            return new PLetter(ruleset);
                        case "digit":
                            return new PDigit(ruleset);
                        case "ident":
                            return ruleset.CreateIdentifierParser();

                        default:
                            // Otherwise, look for a matching rule
                            var rule = ruleset.GetTokenReference(node.String);
                            if (rule != null) return rule;

                            // If no matching rule is found, assume term is a keyword
                            return ruleset.CreateKeywordParser(node.String);
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
                    return new OptionalParser(ruleset, null, expr);
                } else if (nodes.First().String == "{") {
                    // Delorean braces surround a repeated optional expression
                    return new OptionalRepeatParser(ruleset, null, expr);
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
                var ruleset = _sEBNFRuleset = new Ruleset();

                // Define keywords
                var eq = ruleset.CreateKeywordParser("=");
                var fs = ruleset.CreateKeywordParser(".");
                var pipe = ruleset.CreateKeywordParser("|");
                var flat = ruleset.CreateKeywordParser("*");
                var pbOpen = ruleset.CreateKeywordParser("(");
                var pbClose = ruleset.CreateKeywordParser(")");
                var sbOpen = ruleset.CreateKeywordParser("[");
                var sbClose = ruleset.CreateKeywordParser("]");
                var cbOpen = ruleset.CreateKeywordParser("{");
                var cbClose = ruleset.CreateKeywordParser("}");

                // Define some simple parsers to use
                var ident = ruleset.CreateIdentifierParser();
                var str = ruleset.CreateStringParser();
                
                // Define tokens
                var rSyntax = ruleset.CreateTokenParser("Syntax", false, true);
                var rRule = ruleset.CreateTokenParser("Rule");
                var rExpr = ruleset.CreateTokenParser("Expr");
                var rList = ruleset.CreateTokenParser("List");
                var rTerm = ruleset.CreateTokenParser("Term");

                // Define production rules
                ruleset.Add(rSyntax, null * (+rRule));
                ruleset.Add(rRule, +ident[+flat] + eq + rExpr + fs);
                ruleset.Add(rExpr, +rList * (+pipe + rList));
                ruleset.Add(rList, +rTerm * (+rTerm));
                ruleset.Add(rTerm, (+pbOpen + rExpr + pbClose)
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
            var tree = (BranchNode) GetEBNFRuleset().ParseString(bnf);
            var parsed = new Ruleset();

            // Prepare a list of rules to build after all tokens are declared
            var toBuild = new Dictionary<PToken, IEnumerable<ParseNode>>();

            bool first = true;
            foreach (var rule in tree) {
                var children = ((BranchNode) rule).Children;
                var name = children.First().String;
                
                // Tokens marked with a * are flattened into one node
                bool flatten = children.ElementAt(1).String == "*";
                var token = parsed.CreateTokenParser(name, flatten, first);

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
        Dictionary<PToken, Parser> _rules;
        SortedSet<String> _keywords;

        /// <summary>
        /// Constructor to create an empty Ruleset.
        /// </summary>
        public Ruleset()
        {
            _rules = new Dictionary<PToken, Parser>();
            _keywords = new SortedSet<String>();
        }

        /// <summary>
        /// Establishes whether a given string matches the grammar defined
        /// by this ruleset.
        /// </summary>
        /// <param name="str">String to examine the validity of</param>
        /// <returns>True if the given string is valid</returns>
        public bool IsMatch(String str)
        {
            int i = 0;
            return _root.IsMatch(str, ref i, true);
        }

        /// <summary>
        /// Attempt to parse the structure of a string using this Ruleset.
        /// </summary>
        /// <param name="str">String to parse the structure of</param>
        /// <returns>Node tree representing the string's structure</returns>
        public ParseNode ParseString(String str)
        {
            if (IsMatch(str)) {
                int i = 0;
                var tree = _root.Parse(str, ref i, true);
                return tree;
            } else {
                ParserException error;
                _root.FindSyntaxError(str, 0, true, out error);
                throw error;
            }
        }

        /// <summary>
        /// Attempt to parse the structure of the contents of a file using this Ruleset.
        /// </summary>
        /// <param name="filepath">Path of the file to be parsed</param>
        /// <returns>Node tree representing the file's structure</returns>
        public ParseNode ParseFile(String filepath)
        {
            try {
                return ParseString(File.ReadAllText(filepath));
            } catch (ParserException e) {
                throw new ParserException(e, filepath);
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
        public PToken CreateTokenParser(String token, bool flatten = false, bool root = false)
        {
            var rule = new PToken(this, token, flatten);
            _rules.Add(rule, null);
            if (root) _root = rule;
            return rule;
        }

        /// <summary>
        /// Declares a keyword to be used in this ruleset's grammar, and returns a parser
        /// for that keyword.
        /// </summary>
        /// <param name="keyword">Keyword to declare</param>
        /// <returns>Parser for the given keyword</returns>
        public PKeyword CreateKeywordParser(String keyword)
        {
            _keywords.Add(keyword);
            return new PKeyword(this, keyword);
        }

        /// <summary>
        /// Identifies is a given string has been registered as a keyword.
        /// </summary>
        /// <param name="str">String that may potentially be a keyword</param>
        /// <returns>True if the given string has been registered as a keyword</returns>
        public bool IsKeyword(String str)
        {
            return _keywords.Contains(str);
        }

        /// <summary>
        /// Creates a new parser that will match valid identifiers.
        /// </summary>
        /// <returns>Parser that matches identifiers</returns>
        public PIdent CreateIdentifierParser()
        {
            return new PIdent(this);
        }

        /// <summary>
        /// Creates a new parser that will match string literals.
        /// </summary>
        /// <returns>Parser that matches string literals</returns>
        public PString CreateStringParser()
        {
            return new PString(this);
        }

        /// <summary>
        /// Adds a production rule assigned to a given token.
        /// </summary>
        /// <param name="token">Token to assign the new production rule to</param>
        /// <param name="parser">Parser describing the production rule</param>
        public void Add(PToken token, Parser parser)
        {
            var prev = _rules[token];
            if (prev == null) {
                _rules[token] = parser;
            } else {
                // If a production already exists, combine them in a disjunction
                _rules[token] = new EitherOrParser(this, prev, parser);
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
            return _rules.Keys.FirstOrDefault(x => x.Token == name);
        }

        /// <summary>
        /// Gets the parser that corresponds with the given token.
        /// </summary>
        /// <param name="token">Token to find a matching parser from</param>
        /// <returns>Parser that corresponds with the given token</returns>
        public Parser GetReferencedParser(PToken token)
        {
            return _rules[token];
        }

        public IEnumerator<KeyValuePair<PToken, Parser>> GetEnumerator()
        {
            return _rules.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _rules.GetEnumerator();
        }
    }
}
