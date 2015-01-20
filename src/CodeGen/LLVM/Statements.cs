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
        static GenerationContext Procedure(this GenerationContext ctx, Value ident,
            ProcedureType type, Scope scope, Action<GenerationContext> body)
        {
            ctx.Keyword("define");

            _returnType = type.ReturnType ?? VoidType.Default;
            ctx.Type(_returnType);
            
            ctx.Write(" \t{0}\t(", ident);
            ctx.PushScope(scope);
            foreach (var p in type.ParamsWithReceiver) {
                if (p.ByReference) {
                    ctx.Argument(new PointerType(p.Type), new QualIdent(p.Identifier));
                } else {
                    ctx.Argument(p.Type, new QualIdent("$" + p.Identifier));
                }
            }
            ctx.Write(") \t").Keyword("nounwind").Write("{");
            ctx = ctx.Enter().Ln().Ln();

            TempIdent.Reset();

            if (type.ParamsWithReceiver.Count(x => !x.ByReference) > 0) {
                ctx = ctx.Enter(0);
                foreach (var p in type.ParamsWithReceiver.Where(x => !x.ByReference)) {
                    ctx.Local(new QualIdent(p.Identifier), p.Type);
                    ctx.Assign(new QualIdent(p.Identifier), p.Type, new QualIdent("$" + p.Identifier));
                    ctx.Ln();
                }
                ctx = ctx.Leave().Ln();
            }

            if (scope.GetSymbols(false).Count(x => !x.Value.IsParameter) > 0) {
                ctx = ctx.Enter(0);
                foreach (var decl in scope.GetSymbols(false).Where(x => !x.Value.IsParameter)) {
                    ctx.Local(new QualIdent(decl.Key), decl.Value.Type);
                }
                ctx = ctx.Leave().Ln().Ln();
            }

            body(ctx);

            ctx.PopScope();

            _returnType = null;

            return ctx.Leave().Write("}").Ln().Ln();
        }

        static GenerationContext Procedure(this GenerationContext ctx, NForwardDecl proc)
        {
            ProcedureType type;
            RecordType receiverType = null;

            if (proc.Receiver != null) {
                receiverType = _scope.GetType(proc.Receiver.TypeName).As<PointerType>().ResolvedType.As<RecordType>();
                type = receiverType.GetProcedureSignature(proc.Identifier).As<ProcedureType>();
            } else {
                type = _scope.GetSymbol(proc.Identifier).As<ProcedureType>();
            }

            var ident = proc.Receiver != null
                ? new BoundProcedureIdent(receiverType, proc.Identifier)
                : (Value) new QualIdent(proc.Identifier);

            if (proc is NProcDecl) {
                var decl = (NProcDecl) proc;
                return ctx.Procedure(ident, type, decl.Scope, (context) => {
                    context.Statements(decl.Statements);

                    if (_returnType is VoidType && !decl.Statements.EndsInBranch()) {
                        context.Keyword("ret", "void").EndOperation();
                    }
                });
            } else {
                return ctx.Global(ident, type);
            }
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
                var val = ctx.PrepareOperand(node.Expression, _returnType);
                return ctx.Keyword("ret").Argument(_returnType, val);
            }
        }

        static GenerationContext Node(this GenerationContext ctx, NAssignment node)
        {
            var dest = ctx.GetDesignation(node.Assignee);
            var type = node.Assignee.GetFinalType(_scope);

            if (dest is VectorElement) {
                var vecElem = (VectorElement) dest;
                var vecDesig = (NDesignator) node.Assignee.Element;
                var vec = ctx.GetDesignation(vecDesig);
                var vecType = vecDesig.GetFinalType(_scope).As<VectorType>();
                var orig = vecElem.Vector;
                var value = ctx.PrepareOperand(node.Expression, vecType.ElementType);
                var temp = new InsertElement(false, vecType, orig, value, vecElem.Index);
                value = new TempIdent();
                ctx.Assign(value).Argument(temp).EndOperation();
                return ctx.Assign(vec, vecType, value);
            } else {
                return ctx.Assign(dest, type, node.Expression);
            }
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

            ctx.LabelMarker(ifend);

            if (node.ThenBody.EndsInBranch() && (node.ElseBody != null && node.ElseBody.EndsInBranch())) {
                ctx.Keyword("unreachable").EndOperation();
            }

            return ctx;
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
            ctx.ResolveValue(iter, ref temp, iter.Declaration.Type, false);
            ctx.BinaryComp(cond, "sgt", "ogt", iter.Declaration.Type, temp, final);
            ctx.Branch(cond, bodyend, bodystart);

            ctx.LabelMarker(bodystart).Ln();
            ctx.PushExitLabel(bodyend).Statements(node.Body).PopExitLabel();
            temp = new TempIdent();
            ctx.ResolveValue(iter, ref temp, iter.Declaration.Type, false);
            ctx.BinaryOp(incr, "add", "fadd", iter.Declaration.Type, temp, new Literal(1));
            ctx.Assign(iter, iter.Declaration.Type, incr);
            ctx.Branch(condstart);

            return ctx.LabelMarker(bodyend);
        }

        static GenerationContext Node(this GenerationContext ctx, NInvocStmnt node)
        {
            var desig = node.Invocation.Element as NDesignator;
            var elem = desig != null ? desig.Element as NQualIdent : null;

            // Probably temporary hack
            if (elem != null && elem.String == "NEW") {
                var invoc = (NInvocation) node.Invocation.Operation;
                var target = invoc.Args.Expressions.First();
                var targetType = target.GetFinalType(_scope);

                if (targetType.IsPointer) {
                    var ptrType = targetType.As<PointerType>();
                    var ptr = ctx.PrepareOperand(target, new PointerType(ptrType));
                    var type = ptrType.ResolvedType;

                    Value temp = new TempIdent();
                    ctx.Assign(temp).Argument(new ElementPointer(false, ptrType, Literal.GetDefault(ptrType), 1)).EndOperation();
                    var size = new TempIdent();
                    ctx.Assign(size).Keyword("ptrtoint").Argument(ptrType, temp).Keyword(" to").Argument(IntegerType.Integer).EndOperation();

                    temp = new TempIdent();
                    ctx.Call(temp, _gcMallocProcType, _gcMallocProc, IntegerType.Integer, size);
                    ctx.Conversion(PointerType.Byte, ptrType, ref temp);
                    ctx.Store(temp, type, Literal.GetDefault(type));
                    ctx.Store(ptr, ptrType, temp);
                } else {
                    var arrayType = targetType.As<ArrayType>();
                    var arrayPtrType = new PointerType(arrayType);

                    Value ptr = ctx.PrepareOperand(target, arrayPtrType);
                    Value temp = null;

                    IEnumerable<OberonType> types = new List<OberonType>();
                    OberonType inner = arrayType;
                    for (;;) {
                        ((List<OberonType>) types).Add(inner);
                        if (inner.IsArray) {
                            inner = inner.As<ArrayType>().ElementType;
                        } else {
                            break;
                        }
                    }

                    var lengths = invoc.Args.Expressions.Skip(1).Select(x =>
                        ctx.PrepareOperand(x, IntegerType.Integer)).ToArray();

                    Value prev = new Literal(1);
                    var totLengths = lengths.Select(x => {
                        temp = new TempIdent();
                        ctx.BinaryOp(temp, "mul", IntegerType.Integer, prev, x);
                        prev = temp;
                        return temp;
                    }).ToArray();

                    Value prevPtr = null;
                    PointerType prevRootPtrType = null;

                    for (int i = types.Count() - 1; i > 0; --i) {
                        var rootType = types.ElementAt(i);
                        var rootPtrType = new PointerType(rootType);
                        StaticArrayType allocatedType = null;
                        for (int j = 0; j < i; ++j) {
                            var lengthValue = lengths[i - j - 1];
                            allocatedType = new StaticArrayType(allocatedType ?? rootType, lengthValue);
                        }
                        var allocatedPtrType = new PointerType(allocatedType);

                        temp = new TempIdent();
                        Value elemSize = new TempIdent();
                        Value size = new TempIdent();

                        ctx.Assign(temp).Argument(new ElementPointer(false, rootPtrType, Literal.GetDefault(rootPtrType), 1)).EndOperation();
                        ctx.Assign(elemSize).Keyword("ptrtoint").Argument(rootPtrType, temp).Keyword(" to").Argument(IntegerType.Integer).EndOperation();
                        ctx.BinaryOp(size, "mul", IntegerType.Integer, elemSize, totLengths[i - 1]);

                        temp = new TempIdent();
                        ctx.Call(temp, _gcMallocProcType, _gcMallocProc, IntegerType.Integer, size);

                        ctx.Conversion(PointerType.Byte, rootPtrType, ref temp);
                        Value rootPtr = temp;

                        if (!rootType.IsArray) {
                            ctx.Conversion(rootPtrType, allocatedPtrType, ref temp);
                            ctx.Store(temp, allocatedType, Literal.GetDefault(allocatedType));
                        } else {
                            var preLoop = _blockLabel;
                            var loopStart = new TempIdent();
                            var loopEnd = new TempIdent();

                            ctx.Branch(loopStart);

                            Value iter = new TempIdent();
                            Value next = new TempIdent();

                            ctx.LabelMarker(loopStart);
                            ctx.Phi(iter, IntegerType.Integer, new Literal(0), preLoop, next, loopStart);
                            ctx.BinaryOp(next, "add", IntegerType.Integer, iter, new Literal(1));

                            Value srcPtr = new TempIdent();
                            Value destPtr = new TempIdent();

                            temp = new TempIdent();
                            ctx.BinaryOp(temp, "mul", IntegerType.Integer, iter, lengths[i]);

                            ctx.Assign(srcPtr).Argument(new ElementPointer(false, prevRootPtrType, prevPtr, temp)).EndOperation();
                            ctx.Assign(destPtr).Argument(new ElementPointer(false, rootPtrType, rootPtr, iter)).EndOperation();

                            temp = new TempIdent();
                            ctx.OpenArray(temp, rootType.As<ArrayType>().ElementType, lengths[i], srcPtr);
                            ctx.Store(destPtr, rootType, temp);

                            Value cond = new TempIdent();

                            ctx.BinaryComp(cond, "slt", IntegerType.Integer, next, totLengths[i - 1]);
                            ctx.Branch(cond, loopStart, loopEnd);

                            ctx.LabelMarker(loopEnd);
                        }

                        prevPtr = rootPtr;
                        prevRootPtrType = rootPtrType;
                    }

                    temp = new TempIdent();
                    ctx.OpenArray(temp, types.ElementAt(0).As<ArrayType>().ElementType, lengths[0], prevPtr);
                    ctx.Store(ptr, types.ElementAt(0), temp);
                }

                return ctx;
            }

            if (elem != null && (elem.String == "VECLOAD" || elem.String == "VECSTORE")) {
                var invoc = (NInvocation) node.Invocation.Operation;
                var arrayExpr = invoc.Args.Expressions.First();
                var arrayType = arrayExpr.GetFinalType(_scope).As<ArrayType>();
                var elemPtrType = new PointerType(arrayType.ElementType);

                var vectorExpr = invoc.Args.Expressions.ElementAt(1);
                var vectorType = vectorExpr.GetFinalType(_scope).As<VectorType>();
                var vectorPtrType = new PointerType(vectorType);
                var vectorPtr = ctx.PrepareOperand(vectorExpr, vectorPtrType);
    
                Value index;            
                if (invoc.Args.Expressions.Count() == 3) {
                    var indexExpr = invoc.Args.Expressions.ElementAt(2);
                    index = ctx.PrepareOperand(indexExpr, IntegerType.Integer);
                } else {
                    index = new Literal(0);
                }
                
                bool nonZeroIndex = !(index is Literal) || ((Literal) index).IntegerValue > 0;

                OberonType arrayPtrType;
                Value arrayPtr;
                if (arrayType.IsOpen) {
                    arrayPtrType = new PointerType(new ArrayType(arrayType.ElementType, vectorType.Length));

                    var temp = ctx.PrepareOperand(arrayExpr, arrayType);
                    arrayPtr = new TempIdent();
                    ctx.Assign(arrayPtr).Argument(new ExtractValue(false, arrayType, temp, 1)).EndOperation();
                    ctx.Conversion(elemPtrType, arrayPtrType, ref arrayPtr);
                } else {
                    arrayPtrType = new PointerType(arrayType);

                    arrayPtr = ctx.PrepareOperand(arrayExpr, arrayPtrType);
                }


                if (nonZeroIndex) {
                    var temp = arrayPtr;
                    arrayPtr = new TempIdent();
                    ctx.Conversion(arrayPtrType, elemPtrType, ref temp);
                    ctx.Assign(arrayPtr).Argument(new ElementPointer(false, elemPtrType, temp, index)).EndOperation();
                    ctx.Conversion(elemPtrType, vectorPtrType, ref arrayPtr);
                } else {
                    ctx.Conversion(arrayPtrType, vectorPtrType, ref arrayPtr);
                }
                
                var vector = new TempIdent();

                if (elem.String == "VECLOAD") {
                    ctx.Load(vector, vectorType, arrayPtr);
                    ctx.Store(vectorPtr, vectorType, vector);
                } else {
                    ctx.Load(vector, vectorType, vectorPtr);
                    ctx.Store(arrayPtr, vectorType, vector);
                }

                return ctx;
            }

            // Temporary print hack
            if (elem != null && elem.Module == "Out") {
                var invoc = (NInvocation) node.Invocation.Operation;
                var arg = invoc.Args != null ? invoc.Args.Expressions.FirstOrDefault() : null;
                Value src = null;
                var type = arg != null ? _scope.GetSymbol(elem.Identifier, elem.Module).As<ProcedureType>().Params.First().Type : PointerType.Null;
                switch (elem.Identifier) {
                    case "Boolean":
                        var temp = new TempIdent();
                        src = ctx.PrepareOperand(arg, type);
                        ctx.Select(temp, src, new PointerType(CharType.Default),
                            new ElementPointer(true, GetStringType("TRUE"), GetStringIdent("TRUE"), 0, 0),
                            new ElementPointer(true, GetStringType("FALSE"), GetStringIdent("FALSE"), 0, 0));
                        return ctx.Call(new TempIdent(), _printfProcType, _printfProc,
                            new PointerType(CharType.Default), temp);
                    case "Byte":
                    case "ShortInt":
                    case "Integer":
                    case "LongInt":
                        src = ctx.PrepareOperand(arg, type);
                        return ctx.Call(new TempIdent(), _printfProcType, _printfProc,
                            new PointerType(CharType.Default),
                            new ElementPointer(true, GetStringType("%i"), GetStringIdent("%i"), 0, 0),
                            IntegerType.LongInt, src);
                    case "Real":
                    case "LongReal":
                        src = ctx.PrepareOperand(arg, type);
                        return ctx.Call(new TempIdent(), _printfProcType, _printfProc,
                            new PointerType(CharType.Default),
                            new ElementPointer(true, GetStringType("%g"), GetStringIdent("%g"), 0, 0),
                            RealType.LongReal, src);
                    case "String":
                        type = type.As<ArrayType>();
                        var ptr = new TempIdent();
                        if (arg.IsConstant(_scope)) {
                            src = ctx.PrepareOperand(arg, type, true);
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
                            src = ctx.PrepareOperand(arg, new PointerType(type));
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
