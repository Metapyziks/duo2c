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

        static GenerationContext Argument(this GenerationContext ctx, bool align = true)
        {
            if (_lastWasArg) ctx.Write(", {0}", align ? "\t" : "");
            _lastWasArg = true;
            return ctx;
        }

        static GenerationContext Argument(this GenerationContext ctx, OberonType type, bool align = true)
        {
            return ctx.Argument(align).Type(type);
        }

        static GenerationContext Argument(this GenerationContext ctx, Value val, bool align = true)
        {
            return ctx.Argument(align).Write(val);
        }

        static GenerationContext Argument(this GenerationContext ctx, OberonType type, Value val, bool align = true)
        {
            return ctx.Argument(align).Type(type).Write(" {0}", align ? "\t" : "").Write(val);
        }

        static GenerationContext EndArguments(this GenerationContext ctx)
        {
            _lastWasArg = false;
            return ctx;
        }

        static GenerationContext EndOperation(this GenerationContext ctx)
        {
            _lastWasArg = false;
            return ctx.Ln();
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

        static GenerationContext Label(this GenerationContext ctx, TempIdent label)
        {
            return ctx.Argument().Keyword("label").Argument(label);
        }

        static TempIdent _blockLabel = null;
        static GenerationContext LabelMarker(this GenerationContext ctx, TempIdent label)
        {
            _blockLabel = label;
            var marker = String.Format("; <label>:{0}", label.ToString().Substring(1));
            var padding = Enumerable.Range(0, 50 - marker.Length).Aggregate(String.Empty, (s, x) => s + " ");
            ctx.Ln().Write("\r{0}{1}; preds = ", marker, padding);
            
            return ctx.Write(() => {
                if (label.Predecessors.Count() == 0) {
                    return "%0";
                } else {
                    var preds = (IEnumerable<TempIdent>) label.Predecessors.OrderBy(x => x == null ? 0 : x.ID);
                    return String.Join(", ", preds.Select(x => x == null ? "%0" : x.ToString()));
                }
            }).EndOperation();
        }

        static GenerationContext Branch(this GenerationContext ctx, TempIdent dest)
        {
            dest.AddPredecessor(_blockLabel);
            return ctx.Keyword("br").Label(dest).EndOperation();
        }

        static GenerationContext Branch(this GenerationContext ctx, Value cond, TempIdent ifTrue, TempIdent ifFalse)
        {
            ifTrue.AddPredecessor(_blockLabel);
            ifFalse.AddPredecessor(_blockLabel);
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
            var argDefns = procType.Params.ToArray();
            var argExprs = args != null ? args.Expressions.ToArray() : new NExpr[0];
            var argTypes = new OberonType[argExprs.Length];
            var argValus = new Value[argExprs.Length];
            for (int i = 0; i < argTypes.Length; ++i) {
                if (argDefns[i].ByReference) {
                    argTypes[i] = new PointerType(argDefns[i].Type);
                } else {
                    argTypes[i] = argDefns[i].Type;
                }
                argValus[i] = ctx.PrepareOperand(argExprs[i], argTypes[i], new TempIdent());
            }

            return ctx.Call(dest, procType, proc, argTypes, argValus);
        }

        static GenerationContext Call(this GenerationContext ctx, Value dest, ProcedureType procType, Value proc, OberonType[] argTypes, Value[] args)
        {
            if (procType.ReturnType != null) ctx.Assign(dest);
            ctx.Keyword("call").Type(new PointerType(procType)).Write(" ").Write(proc).Write("(");
            for (int i = 0; i < args.Length; ++i) {
                ctx.Argument(argTypes[i], args[i], false);
            }
            return ctx.Write(") nounwind").EndOperation();
        }

        static GenerationContext Assign(this GenerationContext ctx, Value dest, OberonType type, NExpr expression)
        {
            return ctx.Assign(dest, type, ctx.PrepareOperand(expression, type, new TempIdent()));
        }

        static GenerationContext Assign(this GenerationContext ctx, Value dest, OberonType type, Value src)
        {
            if (dest is QualIdent && ((QualIdent) dest).Declaration.IsVariable) {
                return ctx.Keyword("store").Argument(type, src).Argument(new PointerType(type), dest).EndOperation();
            } else {
                throw new NotImplementedException("Cannot assign to a non-variable");
            }
        }
    }
}
