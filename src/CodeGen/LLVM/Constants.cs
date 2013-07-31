using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUO2C.CodeGen.LLVM
{
    public static partial class IntermediaryCodeGenerator
    {
        public static GenerationContext StringConstant(this GenerationContext ctx, String str, GlobalStringIdent ident)
        {
            var bytes = UTF8Encoding.UTF8.GetBytes(str);
            var length = bytes.Length + 1;

            ctx.Write("{0} \t= \tprivate \tconstant \t[{1} \tx i8] \tc\"", ident, length.ToString());
            ctx.Write(bytes.Aggregate(String.Empty, (s, x) => String.Format("{0}\\{1}", s, x.ToString("X2"))));
            return ctx.Write("\\00\"").EndOperation();
        }
    }
}
