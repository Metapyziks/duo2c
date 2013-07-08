using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace DUO2C
{
    class Ruleset : IEnumerable<KeyValuePair<PRule, Parser>>
    {
        static bool IsPredefined(String token)
        {
            return token.Length > 1 && token.All(x => char.IsLetter(x) && char.IsLower(x));
        }

        static bool IsKeyword(String token)
        {
            return token.Length > 1 && token.All(x => char.IsLetter(x) && char.IsUpper(x));
        }

        static bool IsToken(String token)
        {
            return char.IsUpper(token[0]) && token.All(x => char.IsLetter(x));
        }

        static Parser BuildExpr(Ruleset ruleset, IEnumerable<ParseNode> nodes)
        {
            Parser curr = null;
            foreach (var node in nodes) {
                if (node.Token == "keyword" && node.String == "|") continue;

                var list = BuildList(ruleset, ((BranchNode) node).Children);
                if (curr == null) {
                    curr = list;
                } else {
                    curr = new EitherOrParser(curr, list);
                }
            }
            return curr;
        }

        static Parser BuildList(Ruleset ruleset, IEnumerable<ParseNode> nodes)
        {
            Parser curr = null;
            foreach (var node in nodes) {
                var term = BuildTerm(ruleset, ((BranchNode) node).Children);
                if (curr == null) {
                    curr = term;
                } else if (term is OptionalParser && ((OptionalParser) term).Left == null) {
                    curr = new OptionalParser(curr, ((OptionalParser) term).Right);
                } else if (term is OptionalRepeatParser && ((OptionalRepeatParser) term).Left == null) {
                    curr = new OptionalRepeatParser(curr, ((OptionalRepeatParser) term).Right);
                } else {
                    curr = new ConcatParser(curr, term);
                }
            }
            return curr;
        }

        static Parser BuildTerm(Ruleset ruleset, IEnumerable<ParseNode> nodes)
        {
            if (nodes.Count() == 1) {
                var node = (LeafNode) nodes.First();
                if (node.Token == "string" || IsKeyword(node.String)) {
                    return new PKeyword(node.String);
                } else if (node.Token == "ident") {
                    switch (node.String) {
                        case "string":
                            return new PString();
                        case "letter":
                            return new PLetter();
                        case "digit":
                            return new PDigit();
                        case "ident":
                            return new PIdent();
                        default:
                            var rule = ruleset.GetRule(node.String);
                            if (rule != null) return rule;
                            return new PKeyword(node.String);
                    }
                } else {
                    throw new NotImplementedException(node.Token + " : " + node.String);
                }
            } else {
                var expr = BuildExpr(ruleset, ((BranchNode) nodes.ElementAt(1)).Children);
                if (nodes.First().String == "[") {
                    return new OptionalParser(null, expr);
                } else if (nodes.First().String == "{") {
                    return new OptionalRepeatParser(null, expr);
                } else {
                    return expr;
                }
            }
        }

        public static Ruleset Parse(String bnf)
        {
            var ident = new PIdent();
            var str = new PString();

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

            var bnfRuleset = new Ruleset();
            var rSyntax = bnfRuleset.CreateRuleToken("Syntax", false, true);
            var rRule = bnfRuleset.CreateRuleToken("Rule");
            var rExpr = bnfRuleset.CreateRuleToken("Expr");
            var rList = bnfRuleset.CreateRuleToken("List");
            var rTerm = bnfRuleset.CreateRuleToken("Term");

            bnfRuleset.Add(rSyntax, null *(+rRule));
            bnfRuleset.Add(rRule, +ident [+flat] +eq +rExpr +fs);
            bnfRuleset.Add(rExpr, +rList *(+pipe +rList));
            bnfRuleset.Add(rList, +rTerm *(+rTerm));
            bnfRuleset.Add(rTerm, (+pbOpen +rExpr +pbClose) | (+sbOpen +rExpr +sbClose) | (+cbOpen +rExpr +cbClose) | +str | +ident);

            var tree = (BranchNode) Parser.Parse(bnf, bnfRuleset);
            var parsed = new Ruleset();

            var toBuild = new Dictionary<PRule, IEnumerable<ParseNode>>();

            bool first = true;
            foreach (var rule in tree) {
                var children = ((BranchNode) rule).Children;
                var name = children.First().String;
                bool flatten = children.ElementAt(1).String == "*";
                var token = parsed.CreateRuleToken(name, flatten, first);
                toBuild.Add(token, ((BranchNode) children.ElementAt(flatten ? 3 : 2)).Children);
                first = false;
            }

            foreach (var rule in toBuild) {
                var parser = BuildExpr(parsed, rule.Value);
                parsed.Add(rule.Key, parser);
            }

            return parsed;
        }

        Parser _root;
        Dictionary<PRule, Parser> _dict;

        public Parser RootParser
        {
            get { return _root; }
        }

        public Ruleset()
        {
            _dict = new Dictionary<PRule, Parser>();
        }

        public PRule CreateRuleToken(String token, bool flatten = false, bool root = false)
        {
            var rule = new PRule(this, token, flatten);
            _dict.Add(rule, null);
            if (root) _root = rule;
            return rule;
        }

        public void Add(PRule rule, Parser parser)
        {
            Debug.WriteLine("{0} ::= {1}.", rule.Token, parser.ToString());

            var prev = _dict[rule];
            if (prev == null) {
                _dict[rule] = parser;
            } else {
                _dict[rule] = new EitherOrParser(prev, parser);
            }
        }

        public void Add(KeyValuePair<PRule, Parser> item)
        {
            Add(item.Key, item.Value);
        }

        public PRule GetRule(String name)
        {
            return _dict.Keys.FirstOrDefault(x => x.Token == name);
        }

        public Parser GetParser(PRule rule)
        {
            return _dict[rule];
        }

        public IEnumerator<KeyValuePair<PRule, Parser>> GetEnumerator()
        {
            return _dict.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _dict.GetEnumerator();
        }
    }
}
