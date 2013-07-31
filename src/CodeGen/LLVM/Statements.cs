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
        static GenerationContext Node(this GenerationContext ctx, NAssignment node)
        {
            var dest = ctx.GetDesignation(node.Assignee);
            var temp = (Value) new TempIdent();
            var type = node.Assignee.GetFinalType(_scope);
            temp = ctx.PrepareOperand(node.Expression, type, temp);
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

            ctx.WriteStatements(node.ThenBody.Statements.Select(x => x.Inner));
            ctx.Branch(ifend);

            if (node.ElseBody != null) {
                ctx.LabelMarker(iffalse).NewLine();
                ctx.WriteStatements(node.ElseBody.Statements.Select(x => x.Inner));
                ctx.Branch(ifend);
            }

            return ctx.LabelMarker(ifend);
        }

        static GenerationContext Node(this GenerationContext ctx, NWhileLoop node)
        {
            var cond = (Value) new TempIdent();
            var condstart = new TempIdent();
            var bodystart = new TempIdent();
            var bodyend = new TempIdent();

            ctx.Branch(condstart);

            ctx.LabelMarker(condstart);
            ctx.Expr(node.Condition, ref cond, BooleanType.Default);
            ctx.Branch(cond, bodystart, bodyend);

            ctx.LabelMarker(bodystart);
            ctx.WriteStatements(node.Body.Statements.Select(x => x.Inner));
            ctx.Branch(bodyend);

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
                if (arg != null) {
                    src = ctx.PrepareOperand(arg, type, null);
                }
                switch (elem.Identifier) {
                    case "Boolean":
                        var temp = new TempIdent();
                        var trueStr = GetStringIdent("TRUE");
                        var falseStr = GetStringIdent("FALSE");
                        ctx.Assign(temp).Keyword("select").Argument(type, src).Keyword("i8* getelementptr inbounds ([5 x i8]* " + trueStr + ", i32 0, i32 0),", "i8* getelementptr inbounds ([6 x i8]* " + falseStr + ", i32 0, i32 0)");
                        return ctx.Assign(tmp).Keyword("call").Type(IntegerType.Integer).Write(" (i8*, ...)* @printf(i8* {0}) nounwind", temp).NewLine();
                    case "Byte":
                    case "ShortInt":
                    case "Integer":
                    case "LongInt":
                        var intStr = GetStringIdent("%i");
                        return ctx.Assign(tmp).Keyword("call").Type(IntegerType.Integer).Write(" (i8*, ...)* @printf(i8* getelementptr inbounds ([3 x i8]* " + intStr + ", i32 0, i32 0), ").Type(type).Write(" {0}) nounwind", src).NewLine();
                    case "Real":
                    case "LongReal":
                        var floatStr = GetStringIdent("%f");
                        return ctx.Assign(tmp).Keyword("call").Type(IntegerType.Integer).Write(" (i8*, ...)* @printf(i8* getelementptr inbounds ([3 x i8]* " + floatStr + ", i32 0, i32 0), ").Type(type).Write(" {0}) nounwind", src).NewLine();
                    case "Ln":
                        var nlStr = GetStringIdent("\n");
                        return ctx.Assign(tmp).Keyword("call").Type(IntegerType.Integer).Write(" (i8*, ...)* @printf(i8* getelementptr inbounds ([2 x i8]* " + nlStr + ", i32 0, i32 0)) nounwind").NewLine();
                }
            }

            throw new NotImplementedException("Invocation statements not yet implented");
        }
    }
}
