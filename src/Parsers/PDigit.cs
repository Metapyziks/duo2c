namespace DUO2C.Parsers
{
    /// <summary>
    /// Atomic parser that parses a single digit.
    /// </summary>
    public class PDigit : Parser
    {
        public override bool IsMatch(string str, ref int i)
        {
            if (i < str.Length && char.IsDigit(str[i])) {
                ++i; return true;
            }
            return false;
        }

        public override ParseNode Parse(string str, ref int i)
        {
            return new LeafNode(i, 1, str[i++].ToString(), "digit");
        }

        public override string ToString()
        {
            return "digit";
        }
    }
}
