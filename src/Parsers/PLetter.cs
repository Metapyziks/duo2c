namespace DUO2C.Parsers
{
    /// <summary>
    /// Atomic parser that parses a single letter.
    /// </summary>
    public class PLetter : Parser
    {
        public override bool IsMatch(string str, ref int i)
        {
            if (i < str.Length && char.IsLetter(str[i])) {
                ++i; return true;
            }
            return false;
        }

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
