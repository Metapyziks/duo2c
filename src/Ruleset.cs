using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;

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

        static Parser BuildParser(Ruleset ruleset, IEnumerable<ParseNode> nodes)
        {
            var arr = nodes.ToArray();
            var first = nodes.First();
            Parser firstParser = null;
            if (first is BranchNode) {
                var branch = (BranchNode) first;
                if (branch.Children.First().String == "[") {
                    firstParser = BuildParser(ruleset, branch.Children);
                } else if (branch.Children.First().String == "{") {
                    // YOU ARE HERE
                } else {
                    firstParser = BuildParser(ruleset, branch.Children);
                }
            } else if (first.Token == "string") {
                firstParser = new PKeyword("\"" + first.String + "\"");
            } else if (IsKeyword(first.String)) {
                firstParser = new PKeyword(first.String);
            } else if (IsPredefined(first.String)) {
                switch (first.String) {
                    case "string":
                        firstParser = new PString();
                        break;
                    case "ident":
                        firstParser = new PIdent();
                        break;
                    default:
                        // TODO: Throw something relevant
                        throw new NotImplementedException();
                }
            } else if (IsToken(first.String)) {
                firstParser = new PRule(ruleset, first.String);
            } else {
                // TODO: Throw something relevant
                throw new NotImplementedException();
            }

            if (nodes.Count() == 1) {
                return firstParser;
            } else {
                return new ConcatParser(firstParser, BuildParser(ruleset, nodes.Skip(1)));
            }
        }

        public static Ruleset Parse(String bnf)
        {
            var ident = new PIdent();
            var str = new PString();

            var eq = new PKeyword("=");
            var fs = new PKeyword(".");
            var pipe = new PKeyword("|");
            var sbOpen = new PKeyword("[");
            var sbClose = new PKeyword("]");
            var cbOpen = new PKeyword("{");
            var cbClose = new PKeyword("}");

            var bnfRuleset = new Ruleset();
            var rSyntax = bnfRuleset.CreateRuleToken("Syntax", true);
            var rRule = bnfRuleset.CreateRuleToken("Rule");
            var rExpr = bnfRuleset.CreateRuleToken("Expr");
            var rTerm = bnfRuleset.CreateRuleToken("Term");

            bnfRuleset.Add(rSyntax, null *(+rRule));
            bnfRuleset.Add(rRule, +ident +eq +rExpr +fs);
            bnfRuleset.Add(rExpr, +rTerm *(+rTerm) *(+pipe +rTerm *(+rTerm)));
            bnfRuleset.Add(rTerm, (+sbOpen +rExpr +sbClose) | (+cbOpen +rExpr +cbClose) | +str | +ident);

            var tree = (BranchNode) Parser.Parse(bnf, bnfRuleset);
            var parsed = new Ruleset();

            var toBuild = new Dictionary<PRule, IEnumerable<ParseNode>>();

            bool first = true;
            foreach (var rule in tree) {
                var branch = (BranchNode) rule;
                var name = branch.Children.First().String;
                var token = parsed.CreateRuleToken(name, first);
                toBuild.Add(token, ((BranchNode) branch.Children.ElementAt(2)).Children);
                first = false;
            }

            foreach (var rule in toBuild) {
                var parser = BuildParser(parsed, rule.Value);
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

        public PRule CreateRuleToken(String token, bool root = false)
        {
            var rule = new PRule(this, token);
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
            return _dict.Keys.First(x => x.Token == name);
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
