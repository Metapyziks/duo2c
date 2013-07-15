﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUO2C.Nodes.Oberon2
{
    [SubstituteToken("ExprList")]
    public class NExprList : SubstituteNode
    {
        public IEnumerable<NExpr> Expressions
        {
            get { return Children.Select(x => (NExpr) x); }
        }

        public NExprList(ParseNode original)
            : base(original, false)
        {
            Children = Children.Where(x => x is NExpr);
        }
    }
}
