using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DUO2C.Semantics;

namespace DUO2C.Nodes.Oberon2
{
    [SubstituteToken("Nil")]
    public class NNil : ExpressionElement
    {
        public NNil(ParseNode original)
            : base(original, true, false)
        {
            Children = new ParseNode[0];
        }

        public override OberonType FinalType
        {
            get { return PointerType.NilPointer; }
        }

        public override bool IsConstant
        {
            get { return true; }
        }

        public override IEnumerable<ParserException> FindTypeErrors()
        {
            return EmptyExceptionArray;
        }
    }
}
