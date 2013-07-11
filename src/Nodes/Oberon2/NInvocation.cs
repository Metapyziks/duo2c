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
            get { return IntegerType.Default; }
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
    }
}
