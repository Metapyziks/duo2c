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
        static GenerationContext Node(this GenerationContext ctx, NAssignment node)
        {
            var dest = ctx.GetDesignation(node.Assignee);
            var temp = (Value) new TempIdent();
            var type = node.Assignee.GetFinalType(_scope);
            temp = ctx.PrepareOperand(node.Expression, type, temp);
            return ctx.WriteOperation("store", type, temp, new PointerType(type), dest);
        }

        static GenerationContext Node(this GenerationContext ctx, NIfThenElse node)
        {
            var cond = (Value) new TempIdent();
            var iftrue = new TempIdent();
            var iffalse = new TempIdent();
            var ifend = new TempIdent();            

            ctx.Expr(node.Condition, ref cond, BooleanType.Default);
            ctx.Write("br ").Type(BooleanType.Default).Write(" {0}, label ", cond).Write(() => iftrue.ToString());
            ctx.Write(", label ").Write(() => node.ElseBody != null ? iffalse.ToString() : ifend.ToString()).NewLine();

            ctx.NewLine().Write("\r; <label>:{0}", iftrue.ID).NewLine().NewLine();

            ctx.WriteStatements(node.ThenBody.Statements.Select(x => x.Inner));
            ctx.Write("br label ").Write(() => ifend.ToString()).NewLine();

            if (node.ElseBody != null) {
                ctx.NewLine().Write("\r; <label>:{0}", iffalse.ID).NewLine().NewLine();
                ctx.WriteStatements(node.ElseBody.Statements.Select(x => x.Inner));
                ctx.Write("br label ").Write(() => ifend.ToString()).NewLine();

                return ctx.NewLine().Write("\r; <label>:{0}", ifend.ID).NewLine();
            } else {
                return ctx.NewLine().Write("\r; <label>:{0}", ifend.ID).NewLine();
            }
        }

        static GenerationContext Node(this GenerationContext ctx, NWhileLoop node)
        {
            var cond = (Value) new TempIdent();
            var condstart = new TempIdent();
            var bodystart = new TempIdent();
            var bodyend = new TempIdent();

            ctx.Write("br label ").Write(() => condstart.ToString()).NewLine();

            ctx.NewLine().Write("\r; <label>:{0}", condstart.ID).NewLine().NewLine();

            ctx.Expr(node.Condition, ref cond, BooleanType.Default);
            ctx.Write("br ").Type(BooleanType.Default).Write(" {0}, label ", cond).Write(() => bodystart.ToString());
            ctx.Write(", label ").Write(() => bodyend.ToString()).NewLine();

            ctx.NewLine().Write("\r; <label>:{0}", bodystart.ID).NewLine().NewLine();

            ctx.WriteStatements(node.Body.Statements.Select(x => x.Inner));
            ctx.Write("br label ").Write(() => condstart.ToString()).NewLine();

            return ctx.NewLine().Write("\r; <label>:{0}", bodyend.ID).NewLine();
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
