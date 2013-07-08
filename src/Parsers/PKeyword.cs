using System;

namespace DUO2C.Parsers
{
    public class PKeyword : Parser
    {
        public String Keyword { get; private set; }

        public PKeyword(String keyword)
        {
            Keyword = keyword;
        }

        public override bool IsMatch(string str, ref int i)
        {
            int init = i;
            SkipWhitespace(str, ref i);
            for (int j = 0; j < Keyword.Length; ++i, ++j) {
                if (i >= str.Length || str[i] != Keyword[j]) {
                    i = init; return false;
                }
            }
            if (i < str.Length && char.IsLetterOrDigit(str[i - 1]) && char.IsLetterOrDigit(str[i])) {
                i = init; return false;
            }
            return true;
        }

        public override ParseNode Parse(string str, ref int i)
        {
            SkipWhitespace(str, ref i);
            i += Keyword.Length;
            return new LeafNode(Keyword, "keyword");
        }

        public override string ToString()
        {
            return "\"" + Keyword + "\"";
        }
    }
}
