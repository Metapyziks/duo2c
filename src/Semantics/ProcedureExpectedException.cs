using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DUO2C.Nodes;

namespace DUO2C.Semantics
{
    public class ProcedureExpectedException : CompilerException
    {
        public ProcedureExpectedException(OberonType type, ParseNode node)
            : base(ParserError.Semantics, String.Format("Procedure expected, found {0}",
                type), node.StartIndex, node.Length) { }
    }
}
