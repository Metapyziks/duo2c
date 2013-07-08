namespace DUO2C.Parsers
{
    public class PString : Parser
    {
        public override bool IsMatch(string str, ref int i)
        {
            int init = i;
            SkipWhitespace(str, ref i);
            if (i < str.Length && (str[i] == '"' || str[i] == '\'')) {
                char startChar = str[i];
                while (++i < str.Length) {
                    if (str[i] == '\\') {
                        ++i;
                    }
                    if (str[i] == startChar) {
                        ++i;
                        return true;
                    }
                }
            }
            i = init; return false;
        }

        // TODO: Add proper support for escape characters
        public override ParseNode Parse(string str, ref int i)
        {
            SkipWhitespace(str, ref i);
            int j = i;
            IsMatch(str, ref i);
            return new LeafNode(str.Substring(j + 1, i - j - 2), "string");
        }

        public override string ToString()
        {
            return "string";
        }
    }
}
