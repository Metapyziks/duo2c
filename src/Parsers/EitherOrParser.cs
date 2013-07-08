
namespace DUO2C.Parsers
{
    public class EitherOrParser : BinaryParser
    {
        public EitherOrParser(Parser left, Parser right)
            : base(left, right) { }

        public override bool IsMatch(string str, ref int i)
        {
            int init = i;
            if (Left.IsMatch(str, ref i)) return true;
            i = init;
            if (Right.IsMatch(str, ref i)) return true;
            i = init; return false;
        }

        public override ParseNode Parse(string str, ref int i)
        {
            int j = i;
            if (Left.IsMatch(str, ref j)) {
                return Left.Parse(str, ref i);
            } else {
                return Right.Parse(str, ref i);
            }
        }

        public override string ToString()
        {
            return Left.ToString() + " | " + Right.ToString();
        }
    }
}
