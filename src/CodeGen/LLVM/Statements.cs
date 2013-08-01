﻿using System;
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

        static GenerationContext Statements(this GenerationContext ctx, NStatementSeq block)
        {
            foreach (var stmnt in block.Statements.Select(x => x.Inner)) {
#if DEBUG
                ctx.Write("; {0}", stmnt.String);
#endif
                ctx.Enter(0).NewLine().Node(stmnt).NewLine().Leave();
            }
            return ctx;
        }

        static Stack<Value> _exitLabels = new Stack<Value>();
        static GenerationContext PushExitLabel(this GenerationContext ctx, Value label)
        {
            _exitLabels.Push(label);
            return ctx;
        }

        static Value ExitLabel
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

        static GenerationContext Node(this GenerationContext ctx, NAssignment node)
        {
            var dest = ctx.GetDesignation(node.Assignee);
            var type = node.Assignee.GetFinalType(_scope);
            var temp = ctx.PrepareOperand(node.Expression, type, new TempIdent());

            return ctx.Keyword("store").Argument(type, temp).Argument(new PointerType(type), dest).NewLine();
        }

        static GenerationContext Node(this GenerationContext ctx, NIfThenElse node)
        {
            var cond = (Value) new TempIdent();
            var iftrue = new TempIdent();
            var iffalse = node.ElseBody != null ? new TempIdent() : null;
            var ifend = new TempIdent();            

            ctx.Expr(node.Condition, ref cond, BooleanType.Default);
            ctx.Branch(cond, iftrue, iffalse ?? ifend);

            ctx.LabelMarker(iftrue).NewLine();
            ctx.Statements(node.ThenBody);
            if (!node.ThenBody.EndsInBranch()) ctx.Branch(ifend);

            if (node.ElseBody != null) {
                ctx.LabelMarker(iffalse).NewLine();
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

            ctx.LabelMarker(start).NewLine();
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

            ctx.LabelMarker(condstart).NewLine();
            ctx.Expr(node.Condition, ref cond, BooleanType.Default);
            ctx.Branch(cond, bodystart, bodyend);

            ctx.LabelMarker(bodystart).NewLine();
            ctx.PushExitLabel(bodyend).Statements(node.Body).PopExitLabel();
            ctx.Branch(condstart);

            return ctx.LabelMarker(bodyend);
        }

        static GenerationContext Node(this GenerationContext ctx, NRepeatUntil node)
        {
            var cond = (Value) new TempIdent();
            var bodystart = new TempIdent();
            var condstart = new TempIdent();
            var condend = new TempIdent();

            ctx.Branch(bodystart);

            ctx.LabelMarker(bodystart).NewLine();
            ctx.PushExitLabel(condend).Statements(node.Body).PopExitLabel();
            ctx.Branch(condstart);

            ctx.LabelMarker(condstart).NewLine();
            ctx.Expr(node.Condition, ref cond, BooleanType.Default);
            ctx.Branch(cond, condend, bodystart);

            return ctx.LabelMarker(condend);
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
                if (arg != null) {
                    src = ctx.PrepareOperand(arg, type, null);
                }
                switch (elem.Identifier) {
                    case "Boolean":
                        var temp = new TempIdent();
                        ctx.Select(temp, src, new PointerType(CharType.Default),
                            new ElementPointer(true, GetStringType("TRUE"), GetStringIdent("TRUE"), 0, 0),
                            new ElementPointer(true, GetStringType("FALSE"), GetStringIdent("FALSE"), 0, 0));
                        return ctx.Call(new TempIdent(), _printfProcType, _printfProc,
                            new PointerType(CharType.Default), temp);
                    case "Byte":
                    case "ShortInt":
                    case "Integer":
                    case "LongInt":
                        return ctx.Call(new TempIdent(), _printfProcType, _printfProc,
                            new PointerType(CharType.Default),
                            new ElementPointer(true, GetStringType("%i"), GetStringIdent("%i"), 0, 0),
                            IntegerType.LongInt, src);
                    case "Real":
                    case "LongReal":
                        return ctx.Call(new TempIdent(), _printfProcType, _printfProc,
                            new PointerType(CharType.Default),
                            new ElementPointer(true, GetStringType("%f"), GetStringIdent("%f"), 0, 0),
                            RealType.LongReal, src);
                    case "Ln":
                        return ctx.Call(new TempIdent(), _printfProcType, _printfProc,
                            new PointerType(CharType.Default),
                            new ElementPointer(true, GetStringType("\n"), GetStringIdent("\n"), 0, 0));
                }
            }

            throw new NotImplementedException("Invocation statements not yet implented");
        }
    }
}
