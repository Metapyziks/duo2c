using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DUO2C.Nodes.Oberon2;
using DUO2C.Semantics;

namespace DUO2C.CodeGen.LLVM
{
    public static partial class IntermediaryCodeGenerator
    {
        static bool _lastWasArg;

        static GenerationContext Assign(this GenerationContext ctx, Value dest)
        {
            _lastWasArg = false;
            if (dest is TempIdent) ((TempIdent) dest).ResolveID();
            return ctx.Write("{0} \t= ", dest);
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

        static GenerationContext EndArguments(this GenerationContext ctx)
        {
            _lastWasArg = false;
            return ctx;
        }

        static GenerationContext EndOperation(this GenerationContext ctx)
        {
            _lastWasArg = false;
            return ctx.NewLine();
        }

        static GenerationContext Load(this GenerationContext ctx, Value dest, OberonType type, Value src)
        {
            return ctx.Assign(dest).Keyword("load").Argument(new PointerType(type), src).EndOperation();
        }

        static GenerationContext Conversion(this GenerationContext ctx, Value dest, String conv, OberonType from, Value src, OberonType to)
        {
            return ctx.Assign(dest).Keyword(conv).Argument(from, src).Keyword(" \tto").Argument(to).EndOperation();
        }

        static GenerationContext BinaryOp(this GenerationContext ctx, Value dest, String op, OberonType type, Value a, Value b)
        {
            return ctx.Assign(dest).Keyword(op).Argument(type, a).Argument(b).EndOperation();
        }

        static GenerationContext BinaryOp(this GenerationContext ctx, Value dest, String intOp, String floatOp, OberonType type, Value a, Value b)
        {
            return ctx.BinaryOp(dest, type.IsReal ? floatOp : intOp, type, a, b);
        }

        static GenerationContext BinaryComp(this GenerationContext ctx, Value dest, String intComp, String floatComp, OberonType type, Value a, Value b)
        {
            return ctx.Assign(dest).Keyword(type.IsReal ? "fcmp" : "icmp").Keyword(type.IsReal ? floatComp : intComp)
                .Argument(type, a).Argument(b).EndOperation();
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
            return ctx.Keyword("br").Label(dest).EndOperation();
        }

        static GenerationContext Branch(this GenerationContext ctx, Value cond, Value ifTrue, Value ifFalse)
        {
            return ctx.Keyword("br").Argument(BooleanType.Default, cond).Label(ifTrue).Label(ifFalse).EndOperation();
        }

        static GenerationContext Select(this GenerationContext ctx, Value dest, Value cond, OberonType type, Value ifTrue, Value ifFalse)
        {
            return ctx.Assign(dest).Keyword("select").Argument(BooleanType.Default, cond).Argument(type, ifTrue).Argument(type, ifFalse).EndOperation();
        }

        static GenerationContext Call(this GenerationContext ctx, Value dest, ProcedureType procType, Value proc, params Object[] args)
        {
            return ctx.Call(dest, procType, proc, args.Where(x => x is OberonType).Cast<OberonType>().ToArray(),
                args.Where(x => x is Value).Cast<Value>().ToArray());
        }

        static GenerationContext Call(this GenerationContext ctx, Value dest, ProcedureType procType, Value proc, NExprList args)
        {
            return ctx.Call(dest, procType, proc, args.Expressions.Select(x => x.GetFinalType(_scope)).ToArray(),
                args.Expressions.Select(x => ctx.PrepareOperand(x, x.GetFinalType(_scope), new TempIdent())).ToArray());
        }

        static GenerationContext Call(this GenerationContext ctx, Value dest, ProcedureType procType, Value proc, OberonType[] argTypes, Value[] args)
        {
            for (int i = 0; i < args.Length; ++i) {
                var arg = args[i];
                var fromType = argTypes[i];
                var toType = procType.Params.Select(x => x.Type).ElementAtOrDefault(i);
                if (toType is VarArgsType || toType == null) toType = fromType;

                ctx.Conversion(fromType, toType, ref arg);
            }

            ctx.Assign(dest).Keyword("call").Type(new PointerType(procType)).Write(" \t").Write(proc).Write("\t(");
            for (int i = 0; i < args.Length; ++i) {
                ctx.Argument(argTypes[i], args[i]);
            }
            return ctx.Write(") \tnounwind").EndOperation();
        }
    }
}
