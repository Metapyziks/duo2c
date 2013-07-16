﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DUO2C.Semantics;

namespace DUO2C.Nodes.Oberon2
{
    public abstract class Declaration : SubstituteNode, ITypeErrorSource, IDeclarationSource
    {
        public virtual NIdentDef IdentDef
        {
            get { return (NIdentDef) Children.First(); }
        }

        public String Identifier
        {
            get { return IdentDef.String; }
        }

        public AccessModifier Visibility
        {
            get { return IdentDef.Visibility; }
        }

        public Declaration(ParseNode original)
            : base(original, false) { }

        public IEnumerable<ParserException> FindTypeErrors(Scope scope)
        {
            return Children.SelectMany(x => (x is ITypeErrorSource)
                ? ((ITypeErrorSource) x).FindTypeErrors(scope)
                : new ParserException[0]);
        }

        public abstract void FindDeclarations(Scope scope);    
    }
}
