using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DUO2C.Semantics;

namespace DUO2C.Nodes.Oberon2
{
    public class NBool : ExpressionElement
    {
        public override OberonType FinalType
        {
            get { return BooleanType.Default; }
        }

        public override bool IsConstant
        {
            get { return true; }
        }

        public NBool(ParseNode original)
            : base(original, true) { }

        public override IEnumerable<ParserException> FindTypeErrors()
        {
            return EmptyExceptionArray;
        }
    }
}
