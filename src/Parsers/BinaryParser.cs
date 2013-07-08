namespace DUO2C.Parsers
{
    public abstract class BinaryParser : Parser
    {
        public Parser Left { get; private set; }
        public Parser Right { get; private set; }

        public BinaryParser(Parser left, Parser right)
        {
            Left = left;
            Right = right;
        }
    }
}
