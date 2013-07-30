using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUO2C.CodeGen.LLVM
{
    public static class DataLayout
    {
        public static GenerationContext DataLayoutStart(this GenerationContext ctx, bool bigEndian)
        {
            return ctx.Enter(0).Write("target datalayout = \"{0}", bigEndian ? "E" : "e");
        }

        public static GenerationContext DataLayoutEnd(this GenerationContext ctx)
        {
            return ctx.Write("\"").Leave().NewLine();
        }

        static GenerationContext Seperator(this GenerationContext ctx)
        {
            return ctx.Write("-");
        }

        static GenerationContext AlignParams(this GenerationContext ctx, params int[] sizes)
        {
            return ctx.Write(String.Join(":", sizes));
        }

        public static GenerationContext StackAlign(this GenerationContext ctx, int size)
        {
            return ctx.Seperator().Write("S").AlignParams(size);
        }

        public static GenerationContext PointerAlign(this GenerationContext ctx, int n, int size)
        {
            return ctx.PointerAlign(n, size, size, size);
        }

        public static GenerationContext PointerAlign(this GenerationContext ctx, int n, int size, int abi, int pref)
        {
            return ctx.Seperator().Write("p").AlignParams(n, size, abi, pref);
        }

        public static GenerationContext IntegerAlign(this GenerationContext ctx, int size)
        {
            return ctx.IntegerAlign(size, size, size);
        }

        public static GenerationContext IntegerAlign(this GenerationContext ctx, int size, int abi, int pref)
        {
            return ctx.Seperator().Write("i").AlignParams(size, abi, pref);
        }

        public static GenerationContext VectorAlign(this GenerationContext ctx, int size)
        {
            return ctx.VectorAlign(size, size, size);
        }

        public static GenerationContext VectorAlign(this GenerationContext ctx, int size, int abi, int pref)
        {
            return ctx.Seperator().Write("v").AlignParams(size, abi, pref);
        }

        public static GenerationContext FloatAlign(this GenerationContext ctx, int size)
        {
            return ctx.FloatAlign(size, size, size);
        }

        public static GenerationContext FloatAlign(this GenerationContext ctx, int size, int abi, int pref)
        {
            return ctx.Seperator().Write("f").AlignParams(size, abi, pref);
        }

        public static GenerationContext AgregateAlign(this GenerationContext ctx, int size)
        {
            return ctx.AgregateAlign(0, 0, size);
        }

        public static GenerationContext AgregateAlign(this GenerationContext ctx, int size, int abi, int pref)
        {
            return ctx.Seperator().Write("a").AlignParams(size, abi, pref);
        }

        public static GenerationContext StackObjAlign(this GenerationContext ctx, int size)
        {
            return ctx.StackObjAlign(size, size, size);
        }

        public static GenerationContext StackObjAlign(this GenerationContext ctx, int size, int abi, int pref)
        {
            return ctx.Seperator().Write("s").AlignParams(size, abi, pref);
        }

        public static GenerationContext NativeAlign(this GenerationContext ctx, params int[] sizes)
        {
            return ctx.Seperator().Write("n").AlignParams(sizes);
        }
    }
}
