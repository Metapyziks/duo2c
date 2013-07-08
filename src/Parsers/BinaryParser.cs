using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUO2C.Parsers
{
    abstract class BinaryParser : Parser
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
