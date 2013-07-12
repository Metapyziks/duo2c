using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DUO2C.Semantics;

namespace DUO2C.Nodes.Oberon2
{
    [SubstituteToken("Invocation")]
    public class NInvocation : ExpressionElement
    {
        public NDesignator Procedure
        {
            get { return (NDesignator) Children.First(); }
        }

        // TODO: Lookup actual type
        public override OberonType FinalType
        {
            get { return PointerType.NilPointer; }
        }

        public override bool IsConstant
        {
            get { return false; }
        }

        public NInvocation(ParseNode original)
            : base(original, false)
        {
            Children = Children.Where(x => x is NDesignator || x.Token == "ExprList");
        }

        // TODO: Actually check something
        public override IEnumerable<ParserException> CheckTypes()
        {
            return EmptyExceptionArray;
        }
    }
}
