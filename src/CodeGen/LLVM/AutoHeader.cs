using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUO2C.CodeGen.LLVM
{
    public static partial class IntermediaryCodeGenerator
    {
        public static GenerationContext Header(this GenerationContext ctx, String moduleIdent, Guid guid)
        {
            ctx = ctx.Enter("; ");
            ctx.Write("Generated {0}", DateTime.Now.ToString()).Ln();
            ctx.Write("GlobalUID {0}", guid.ToString()).Ln();
            ctx.Ln();
            ctx.Write("LLVM IR file for module \"{0}\"", moduleIdent).Ln();
            ctx.Ln();
            ctx.Write("WARNING: This file is automatically").Ln();
            ctx.Write("generated and should not be edited");
            return ctx.Leave().Ln().Ln();
        }
    }
}
