﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUO2C.Nodes.Oberon2
{
    [SubstituteToken("Statement")]
    public class NStatement : SubstituteNode, ITypeErrorSource
    {
        public Statement Inner {
            get { return (Statement) Children.First(); }
        }

        public NStatement(ParseNode original)
            : base(original, false)
        {
            Children = Children.Where(x => x is Statement);
        }

        public IEnumerable<ParserException> FindTypeErrors()
        {
            return Inner.FindTypeErrors();
        }
    }
}
