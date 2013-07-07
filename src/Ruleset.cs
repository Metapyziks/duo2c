using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace DUO2C
{
    class Ruleset : IEnumerable<KeyValuePair<PRule, Parser>>
    {
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
            var rTerm = bnfRuleset.CreateRuleToken("Term");

            bnfRuleset.Add(rSyntax, null *(+ident +eq +rRule +fs));
            bnfRuleset.Add(rRule, +rTerm *(+rTerm) *(+pipe +rTerm *(+rTerm)));
            bnfRuleset.Add(rTerm, (+sbOpen +rRule +sbClose) | (+cbOpen +rRule +cbClose) | +str | +ident);

            var tree = Parser.Parse(bnf, bnfRuleset);
            Console.WriteLine(tree.ToString());

            var parsed = new Ruleset();

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
