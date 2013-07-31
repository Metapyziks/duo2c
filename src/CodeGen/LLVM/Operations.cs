using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DUO2C.Semantics;

namespace DUO2C.CodeGen.LLVM
{
    public static partial class IntermediaryCodeGenerator
    {
        static bool _lastWasArg;

        static GenerationContext Assign(this GenerationContext ctx, Value dest)
        {
            if (dest is TempIdent) ((TempIdent) dest).ResolveID();
            return ctx.Write("{0} \t= \t", dest);
        }

        static GenerationContext Keyword(this GenerationContext ctx, params string[] keywords)
        {
            _lastWasArg = false;
            return ctx.Write(String.Join(" \t", keywords)).Write(" \t");
        }

        static GenerationContext Argument(this GenerationContext ctx)
        {
            if (_lastWasArg) ctx.Write(", \t");
            _lastWasArg = true;
            return ctx;
        }

        static GenerationContext Argument(this GenerationContext ctx, OberonType type)
        {
            return ctx.Argument().Type(type);
        }

        static GenerationContext Argument(this GenerationContext ctx, Value val)
        {
            return ctx.Argument().Write(val);
        }

        static GenerationContext Argument(this GenerationContext ctx, OberonType type, Value val)
        {
            return ctx.Argument().Type(type).Write(" \t").Write(val);
        }

        static GenerationContext Global(this GenerationContext ctx, Value dest, OberonType type, bool isPublic)
        {
            ctx.Assign(dest);
            if (!isPublic) ctx.Keyword("private");
            return ctx.Keyword("global").Argument(type, Literal.GetDefault(type)).NewLine();
        }

        static GenerationContext Load(this GenerationContext ctx, Value dest, OberonType type, Value src)
        {
            return ctx.Assign(dest).Keyword("load").Argument(new PointerType(type), src).NewLine();
        }

        static GenerationContext Conversion(this GenerationContext ctx, Value dest, String conv, OberonType from, Value src, OberonType to)
        {
            return ctx.Assign(dest).Keyword(conv).Argument(from, src).Keyword("to").Argument(to).NewLine();
        }

        static GenerationContext BinaryOp(this GenerationContext ctx, Value dest, String op, OberonType type, Value a, Value b)
        {
            return ctx.Assign(dest).Keyword(op).Argument(type, a).Argument(b).NewLine();
        }

        static GenerationContext BinaryOp(this GenerationContext ctx, Value dest, String intOp, String floatOp, OberonType type, Value a, Value b)
        {
            return ctx.BinaryOp(dest, type.IsReal ? floatOp : intOp, type, a, b);
        }

        static GenerationContext BinaryComp(this GenerationContext ctx, Value dest, String intComp, String floatComp, OberonType type, Value a, Value b)
        {
            return ctx.Assign(dest).Keyword(type.IsReal ? "fcmp" : "icmp").Keyword(type.IsReal ? floatComp : intComp)
                .Argument(type, a).Argument(b).NewLine();
        }

        static GenerationContext Label(this GenerationContext ctx, Value label)
        {
            return ctx.Argument().Keyword("label").Argument(label);
        }

        static GenerationContext LabelMarker(this GenerationContext ctx, Value label)
        {
            return ctx.NewLine().Write("\r; <label>:{0}", label.ToString().Substring(1)).NewLine();
        }

        static GenerationContext Branch(this GenerationContext ctx, Value dest)
        {
            return ctx.Keyword("br").Label(dest).NewLine();
        }

        static GenerationContext Branch(this GenerationContext ctx, Value cond, Value ifTrue, Value ifFalse)
        {
            return ctx.Keyword("br").Argument(BooleanType.Default, cond).Label(ifTrue).Label(ifFalse).NewLine();
        }
    }
}
