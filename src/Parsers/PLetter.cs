
namespace DUO2C.Parsers
{
    public class PLetter : Parser
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
