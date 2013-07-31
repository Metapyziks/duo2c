using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DUO2C.Semantics;

namespace DUO2C.Nodes.Oberon2
{
    [SubstituteToken("Bool")]
    public class NBool : ExpressionElement
    {
        public override OberonType GetFinalType(Scope scope)
        {
            return BooleanType.Default;
        }

        public override bool IsConstant(Scope scope)
        {
            return true;
        }

        public NBool(ParseNode original)
            : base(original, true) { }

        public override IEnumerable<CompilerException> FindTypeErrors(Scope scope)
        {
            return EmptyExceptionArray;
        }
    }
}
