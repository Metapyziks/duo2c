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
        static bool EndsInBranch(this NStatementSeq block)
        {
            var stmnts = block.Statements;
            return stmnts.Count() > 0 && (stmnts.Last().Inner is NExit || stmnts.Last().Inner is NReturn);
        }

        static OberonType _returnType;
        static GenerationContext Procedure(this GenerationContext ctx, NProcDecl proc)
        {
            ctx.Keyword("define");

            ProcedureType type;

            if (proc.Receiver != null) {
                type = _scope.GetType(proc.Receiver.TypeName).As<RecordType>()
                    .GetFieldType(proc.Identifier).As<ProcedureType>();
            } else {
                type = _scope.GetSymbol(proc.Identifier).As<ProcedureType>();
            }

            _returnType = type.ReturnType ?? VoidType.Default;

            if (type.ReturnType != null) {
                ctx.Type(type.ReturnType);
            } else {
                ctx.Type(VoidType.Default);
            }

            var ident = proc.Receiver != null
                ? new BoundProcedureIdent(_scope.GetType(proc.Receiver.TypeName).As<RecordType>(), proc.Identifier)
                : (Value) new QualIdent(proc.Identifier);

            ctx.Write(" \t{0}\t(", ident);
            ctx.PushScope(proc.Scope);
            foreach (var p in type.Params) {
                if (p.ByReference) {
                    ctx.Argument(new PointerType(p.Type), new QualIdent(p.Identifier));
                } else {
                    ctx.Argument(p.Type, new QualIdent("$" + p.Identifier));
                }
            }
            ctx.Write(") \t").Keyword("nounwind").Write("{");
            ctx = ctx.Enter().Ln().Ln();
            
            TempIdent.Reset();

            ctx = ctx.Enter(0);
            foreach (var p in type.Params.Where(x => !x.ByReference)) {
                ctx.Local(new QualIdent(p.Identifier), p.Type);
                ctx.Assign(new QualIdent(p.Identifier), p.Type, new QualIdent("$" + p.Identifier));
            }
            ctx = ctx.Leave().Ln().Ln();

            ctx = ctx.Enter(0);
            foreach (var decl in proc.Scope.GetSymbols()) {
                if (type.Params.Any(x => x.Identifier == decl.Key)) continue;

                ctx.Local(new QualIdent(decl.Key), decl.Value.Type);
            }
            ctx = ctx.Leave().Ln().Ln();
            
            ctx.Statements(proc.Statements);
            ctx.PopScope();
            
            if (_returnType is VoidType && !proc.Statements.EndsInBranch()) {
                ctx.Keyword("ret", "void").EndOperation();
            }

            _returnType = null;

            return ctx.Leave().Write("}").Ln();
        }

        static GenerationContext Statements(this GenerationContext ctx, NStatementSeq block)
        {
            foreach (var stmnt in block.Statements.Select(x => x.Inner)) {
#if DEBUG
                ctx.Write("; {0}", stmnt.String);
#endif
                ctx.Enter(0).Ln().Node(stmnt).Ln().Leave();
            }
            return ctx;
        }

        static Stack<TempIdent> _exitLabels = new Stack<TempIdent>();
        static GenerationContext PushExitLabel(this GenerationContext ctx, TempIdent label)
        {
            _exitLabels.Push(label);
            return ctx;
        }

        static TempIdent ExitLabel
        {
            get { return _exitLabels.Count > 0 ? _exitLabels.Peek() : null; }
        }

        static GenerationContext PopExitLabel(this GenerationContext ctx)
        {
            _exitLabels.Pop();
            return ctx;
        }

        static GenerationContext Node(this GenerationContext ctx, NExit node)
        {
            return ctx.Branch(ExitLabel);
        }

        static GenerationContext Node(this GenerationContext ctx, NReturn node)
        {
            if (_returnType is VoidType) {
                return ctx.Keyword("ret").Argument(_returnType).EndOperation();
            } else {
                var val = ctx.PrepareOperand(node.Expression, _returnType, new TempIdent());
                return ctx.Keyword("ret").Argument(_returnType, val);
            }
        }

        static GenerationContext Node(this GenerationContext ctx, NAssignment node)
        {
            var dest = ctx.GetDesignation(node.Assignee);
            var type = node.Assignee.GetFinalType(_scope);
            return ctx.Assign(dest, type, node.Expression);
        }

        static GenerationContext Node(this GenerationContext ctx, NIfThenElse node)
        {
            var cond = (Value) new TempIdent();
            var iftrue = new TempIdent();
            var iffalse = node.ElseBody != null ? new TempIdent() : null;
            var ifend = new TempIdent();            

            ctx.Expr(node.Condition, ref cond, BooleanType.Default);
            ctx.Branch(cond, iftrue, iffalse ?? ifend);

            ctx.LabelMarker(iftrue).Ln();
            ctx.Statements(node.ThenBody);
            if (!node.ThenBody.EndsInBranch()) ctx.Branch(ifend);

            if (node.ElseBody != null) {
                ctx.LabelMarker(iffalse).Ln();
                ctx.Statements(node.ElseBody);
                if (!node.ElseBody.EndsInBranch()) ctx.Branch(ifend);
            }

            return ctx.LabelMarker(ifend);
        }

        static GenerationContext Node(this GenerationContext ctx, NUncondLoop node)
        {
            var start = new TempIdent();
            var end = new TempIdent();

            ctx.Branch(start);

            ctx.LabelMarker(start).Ln();
            ctx.PushExitLabel(end).Statements(node.Body).PopExitLabel();
            ctx.Branch(start);

            return ctx.LabelMarker(end);
        }

        static GenerationContext Node(this GenerationContext ctx, NWhileLoop node)
        {
            var cond = (Value) new TempIdent();
            var condstart = new TempIdent();
            var bodystart = new TempIdent();
            var bodyend = new TempIdent();

            ctx.Branch(condstart);

            ctx.LabelMarker(condstart).Ln();
            ctx.Expr(node.Condition, ref cond, BooleanType.Default);
            ctx.Branch(cond, bodystart, bodyend);

            ctx.LabelMarker(bodystart).Ln();
            ctx.PushExitLabel(bodyend).Statements(node.Body).PopExitLabel();
            ctx.Branch(condstart);

            return ctx.LabelMarker(bodyend);
        }

        static GenerationContext Node(this GenerationContext ctx, NRepeatUntil node)
        {
            var cond = (Value) new TempIdent();
            var bodystart = new TempIdent();
            var condend = new TempIdent();

            ctx.Branch(bodystart);

            ctx.LabelMarker(bodystart).Ln();
            ctx.PushExitLabel(condend).Statements(node.Body).PopExitLabel();

            ctx.Expr(node.Condition, ref cond, BooleanType.Default);
            ctx.Branch(cond, condend, bodystart);

            return ctx.LabelMarker(condend);
        }

        static GenerationContext Node(this GenerationContext ctx, NForLoop node)
        {
            var final = (Value) new TempIdent();
            var cond = (Value) new TempIdent();
            var incr = (Value) new TempIdent();
            var condstart = new TempIdent();
            var bodystart = new TempIdent();
            var bodyend = new TempIdent();

            var iter = new QualIdent(node.IteratorName);
            ctx.Assign(iter, iter.Declaration.Type, node.Initial);
            ctx.Branch(condstart);

            ctx.LabelMarker(condstart).Ln();
            ctx.Expr(node.Final, ref final, iter.Declaration.Type);
            Value temp = new TempIdent();
            ctx.ResolveValue(iter, ref temp, iter.Declaration.Type);
            ctx.BinaryComp(cond, "sgt", "ogt", iter.Declaration.Type, temp, final);
            ctx.Branch(cond, bodyend, bodystart);

            ctx.LabelMarker(bodystart).Ln();
            ctx.PushExitLabel(bodyend).Statements(node.Body).PopExitLabel();
            temp = new TempIdent();
            ctx.ResolveValue(iter, ref temp, iter.Declaration.Type);
            ctx.BinaryOp(incr, "add", "fadd", iter.Declaration.Type, temp, new Literal(1.ToString()));
            ctx.Assign(iter, iter.Declaration.Type, incr);
            ctx.Branch(condstart);

            return ctx.LabelMarker(bodyend);
        }

        static GenerationContext Node(this GenerationContext ctx, NInvocStmnt node)
        {
            var tmp = new TempIdent();

            var desig = node.Invocation.Element as NDesignator;
            var elem = desig != null ? desig.Element as NQualIdent : null;

            // Temporary print hack
            if (elem != null && elem.Module == "Out") {
                var invoc = (NInvocation) node.Invocation.Operation;
                var arg = invoc.Args != null ? invoc.Args.Expressions.FirstOrDefault() : null;
                Value src = null;
                var type = arg != null ? _scope.GetSymbol(elem.Identifier, elem.Module).As<ProcedureType>().Params.First().Type : PointerType.NilPointer;
                switch (elem.Identifier) {
                    case "Boolean":
                        var temp = new TempIdent();
                        src = ctx.PrepareOperand(arg, type, null);
                        ctx.Select(temp, src, new PointerType(CharType.Default),
                            new ElementPointer(true, GetStringType("TRUE"), GetStringIdent("TRUE"), 0, 0),
                            new ElementPointer(true, GetStringType("FALSE"), GetStringIdent("FALSE"), 0, 0));
                        return ctx.Call(new TempIdent(), _printfProcType, _printfProc,
                            new PointerType(CharType.Default), temp);
                    case "Byte":
                    case "ShortInt":
                    case "Integer":
                    case "LongInt":
                        src = ctx.PrepareOperand(arg, type, null);
                        return ctx.Call(new TempIdent(), _printfProcType, _printfProc,
                            new PointerType(CharType.Default),
                            new ElementPointer(true, GetStringType("%i"), GetStringIdent("%i"), 0, 0),
                            IntegerType.LongInt, src);
                    case "Real":
                    case "LongReal":
                        src = ctx.PrepareOperand(arg, type, null);
                        return ctx.Call(new TempIdent(), _printfProcType, _printfProc,
                            new PointerType(CharType.Default),
                            new ElementPointer(true, GetStringType("%g"), GetStringIdent("%g"), 0, 0),
                            RealType.LongReal, src);
                    case "String":
                        type = type.As<ArrayType>();
                        var ptr = new TempIdent();
                        if (arg.IsConstant(_scope)) {
                            src = ctx.PrepareOperand(arg, type, null);
                            if (src is StringLiteral) {
                                var lit = (StringLiteral) src;
                                ctx.Assign(ptr);
                                ctx.Argument(new ElementPointer(false, lit.ConstType, lit.Identifier, 0, 0));
                                ctx.EndOperation();
                                src = ptr;
                            } else {
                                throw new InvalidOperationException();
                            }
                        } else {
                            src = ctx.PrepareOperand(arg, new PointerType(type), null);
                            ctx.Assign(ptr);
                            ctx.Argument(new ElementPointer(false, new PointerType(type), src, 0, 1));
                            ctx.EndOperation();
                            src = new TempIdent();
                            ctx.Load(src, new PointerType(CharType.Default), ptr);
                        }
                        return ctx.Call(new TempIdent(), _printfProcType, _printfProc,
                            new PointerType(CharType.Default),
                            new ElementPointer(true, GetStringType("%s"), GetStringIdent("%s"), 0, 0),
                            new PointerType(CharType.Default), src);
                    case "Ln":
                        return ctx.Call(new TempIdent(), _printfProcType, _printfProc,
                            new PointerType(CharType.Default),
                            new ElementPointer(true, GetStringType("\n"), GetStringIdent("\n"), 0, 0));
                }
            }

            ctx.GetDesignation(node.Invocation);
            return ctx;
        }
    }
}
