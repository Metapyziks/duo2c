using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DUO2C.Nodes;
using DUO2C.Nodes.Oberon2;

namespace DUO2C.Semantics
{
    public class MemberNotFoundException : ParserException
    {
        public OberonType ElementType { get; private set; }
        public NDesignator MemberAccess { get; private set; }

        public MemberNotFoundException(OberonType elementType, NDesignator memAccess)
            : base(ParserError.Semantic, String.Format("Member '{0}' not found for type '{1}'",
                ((NMemberAccess) memAccess.Operation).Identifier, elementType),
                memAccess.StartIndex, memAccess.Length)
        {
            ElementType = elementType;
            MemberAccess = memAccess;
        }
    }
}
