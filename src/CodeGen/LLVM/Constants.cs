using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUO2C.CodeGen.LLVM
{
    public static partial class IntermediaryCodeGenerator
    {
        public static GenerationContext StringConstant(this GenerationContext ctxt, String str, GlobalStringIdent ident)
        {
            var bytes = UTF8Encoding.UTF8.GetBytes(str);
            var length = bytes.Length + 1;

            ctxt.Write("{0} \t= \tprivate \tconstant \t[", ident).Write(length.ToString()).Write(" \tx i8] \tc\"");
            ctxt.Write(bytes.Aggregate(String.Empty, (s, x) => String.Format("{0}\\{1}", s, x.ToString("X2"))));
            return ctxt.Write("\\00\"").NewLine();
        }
    }
}
