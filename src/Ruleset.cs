using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace DUO2C
{
    class Ruleset : IEnumerable<KeyValuePair<PRule, Parser>>
    {
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
