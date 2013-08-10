using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DUO2C.Nodes;

namespace DUO2C.Semantics
{
    public class AccessibilityException : CompilerException
    {
        public AccessibilityException(ParseNode ident)
            : base(ParserError.Semantics, String.Format("Type '{0}' is less accessible than the "
                + "exported symbol that references it", ident.String), ident.StartIndex, ident.Length)
        { }
    }
}
