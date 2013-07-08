using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUO2C.Parsers
{
    class PLetter : Parser
    {
        public override bool IsMatch(string str, ref int i)
        {
            if (i < str.Length && char.IsLetter(str[i])) {
                ++i; return true;
            }
            return false;
        }

        // TODO: Add proper support for escape characters
        public override ParseNode Parse(string str, ref int i)
        {
            return new LeafNode(str[i++].ToString(), "letter");
        }

        public override string ToString()
        {
            return "letter";
        }
    }
}
