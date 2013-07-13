﻿using System;
using System.Linq;

namespace DUO2C.Nodes.Oberon2
{
    /// <summary>
    /// Substitution node for member accesses.
    /// </summary>
    [SubstituteToken("MemberAccess")]
    class NMemberAccess : Selector
    {
        public String Identifier { get; private set; }

        public override string String
        {
            get { return Identifier; }
        }
        
        public NMemberAccess(ParseNode original)
            : base(original, true)
        {
            Children = Children.Where(x => x is NIdent);
            Identifier = Children.First().String;
        }
    }
}
