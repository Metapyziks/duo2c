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
            int length = 1;
            ctxt.Write(ident.ToString() + " ").Anchor()
                .Write("= ").Anchor().Write("private ").Anchor().Write("constant ").Anchor()
                .Write("[").Write(() => length.ToString()).Write(" ").Anchor().Write("x i8] ").Anchor()
                .Write("c\"");

            for (int i = 0; i < str.Length; ++i) {
                uint c = str[i];
                do {
                    var hex = (c & 0xff).ToString("X2");
                    ctxt.Write("\\{0}", hex);
                    ++length;
                    c >>= 8;
                } while (c > 0);
            }

            return ctxt.Write("\\00\"").NewLine();
        }
    }
}
