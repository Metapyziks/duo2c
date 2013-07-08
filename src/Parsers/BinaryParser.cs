namespace DUO2C.Parsers
{
    /// <summary>
    /// Abstract class for parsers that combine two other parsers
    /// in some way.
    /// </summary>
    public abstract class BinaryParser : Parser
    {
        /// <summary>
        /// Parser to be utilised or tested first.
        /// </summary>
        public Parser Left { get; private set; }

        /// <summary>
        /// Parser to be utilised or tested second.
        /// </summary>
        public Parser Right { get; private set; }

        /// <summary>
        /// Abstract constructor to create a new BinaryParser.
        /// </summary>
        /// <param name="left">Parser to be utilised or tested first</param>
        /// <param name="right">Parser to be utilised or tested second</param>
        public BinaryParser(Parser left, Parser right)
        {
            Left = left;
            Right = right;
        }
    }
}
