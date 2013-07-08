namespace DUO2C.Parsers
{
    public class PIdent : Parser
    {
        public override bool IsMatch(string str, ref int i)
        {
            int init = i;
            SkipWhitespace(str, ref i);
            int j = 0;
            while (i < str.Length && (char.IsLetter(str[i]) || (j > 0 && char.IsDigit(str[i])))) {
                ++i; ++j;
            }
            if (j > 0) return true;
            i = init; return false;
        }

        public override ParseNode Parse(string str, ref int i)
        {
            SkipWhitespace(str, ref i);
            int j = i;
            IsMatch(str, ref i);
            return new LeafNode(str.Substring(j, i - j), "ident");
        }

        public override string ToString()
        {
            return "ident";
        }
    }
}
