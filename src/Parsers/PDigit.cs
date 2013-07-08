
namespace DUO2C.Parsers
{
    public class PDigit : Parser
    {
        public override bool IsMatch(string str, ref int i)
        {
            if (i < str.Length && char.IsDigit(str[i])) {
                ++i; return true;
            }
            return false;
        }

        // TODO: Add proper support for escape characters
        public override ParseNode Parse(string str, ref int i)
        {
            return new LeafNode(str[i++].ToString(), "digit");
        }

        public override string ToString()
        {
            return "digit";
        }
    }
}
