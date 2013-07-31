using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DUO2C.Nodes.Oberon2;

namespace DUO2C.Semantics
{
    public class AccessibilityException : CompilerException
    {
        public AccessibilityException(NQualIdent ident)
            : base(ParserError.Semantics, String.Format("Type '{0}' is less accessible than the "
                + "exported symbol that references it", ident.String), ident.StartIndex, ident.Length)
        { }
    }
}
