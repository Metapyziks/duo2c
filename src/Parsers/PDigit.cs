namespace DUO2C.Parsers
{
    /// <summary>
    /// Atomic parser that parses a single digit.
    /// </summary>
    public class PDigit : Parser
    {
        /// <summary>
        /// Constructor to create a new digit parser.
        /// </summary>
        /// <param name="ruleset">The ruleset that will contain this parser</param>
        public PDigit(Ruleset ruleset)
            : base(ruleset) { }

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
