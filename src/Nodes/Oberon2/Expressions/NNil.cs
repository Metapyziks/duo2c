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

        public override OberonType GetFinalType(Scope scope)
        {
            return PointerType.Null;
        }

        public override bool IsConstant(Scope scope)
        {
            return true;
        }

        public override IEnumerable<CompilerException> FindTypeErrors(Scope scope)
        {
            return EmptyExceptionArray;
        }
    }
}
