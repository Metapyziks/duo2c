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
        static GenerationContext Node(this GenerationContext ctx, NDesignator node)
        {
            var ident = ctx.GetDesignation(node);
            return ctx.Write(ident.ToString());
        }

        static Value GetDesignation(this GenerationContext ctx, NDesignator node)
        {
            if (node.IsRoot) {
                return new QualIdent((NQualIdent) node.Element);
            } else if (node.Operation is NPtrResolve) {
                var desig = (NDesignator) node.Element;
                var type = desig.GetFinalType(_scope);
                var temp = ctx.GetDesignation(desig);
                Value ptr = new TempIdent();
                ctx.ResolveValue(temp, ref ptr, type, false);
                return ptr;
            } else if (node.Operation is NTypeGuard) {
                var op = (NTypeGuard) node.Operation;
                var desig = (NDesignator) node.Element;
                var oldType = desig.GetFinalType(_scope);
                var newType = new UnresolvedType(op.TypeIdent.Identifier, op.TypeIdent.Module).Resolve(_scope);
                var val = ctx.GetDesignation(desig);
                Value temp = new TempIdent();
                ctx.ResolveValue(val, ref temp, oldType, false);
                val = temp;
                temp = new TempIdent();
                ctx.Assign(temp).Argument(new BitCast(false, oldType, val, newType)).EndOperation();
                return temp;
            } else if (node.Operation is NTypeTest) {
                var op = (NTypeTest) node.Operation;
                var desig = (NDesignator) node.Element;
                var val = ctx.GetDesignation(desig);
                var type = desig.GetFinalType(_scope);
                var recType = type.As<PointerType>().ResolvedType.As<RecordType>();
                var testType = _scope.GetType(op.TypeIdent.Identifier,
                    op.TypeIdent.Module).As<PointerType>().ResolvedType.As<RecordType>();

                Value temp = new TempIdent();
                ctx.ResolveValue(val, ref temp, type, false);
                val = temp;
                temp = new TempIdent();

                var isNull = new TempIdent();
                ctx.BinaryComp(isNull, "eq", type, val, Literal.GetDefault(PointerType.Null));

                var startLabel = _blockLabel;

                var end = new TempIdent();
                var initLoop = new TempIdent();
                var checkNull = new TempIdent();
                var testPtr = new TempIdent();
                ctx.Branch(isNull, end, initLoop);

                ctx.LabelMarker(initLoop);

                // Get the type of the record's vtable
                var recTablePtrType = new PointerType(GetRecordTableType(recType));

                // Find pointer to the vtable
                Value recTablePtr = new TempIdent();
                ctx.Assign(recTablePtr);
                ctx.Argument(new ElementPointer(false, type, val, 0, 0));
                ctx.EndOperation();
                temp = recTablePtr;
                
                recTablePtr = new TempIdent();
                ctx.Load(recTablePtr, PointerType.Byte, temp);
                ctx.Branch(checkNull);

                ctx.LabelMarker(checkNull);
                Value curTablePtr = new TempIdent();
                var nextTablePtr = new TempIdent();
                ctx.Phi(curTablePtr, PointerType.Byte, recTablePtr, initLoop, nextTablePtr, testPtr);

                isNull = new TempIdent();
                ctx.BinaryComp(isNull, "eq", PointerType.Byte, curTablePtr, Literal.GetDefault(PointerType.Null));
                ctx.Branch(isNull, end, testPtr);

                ctx.LabelMarker(testPtr);
                var isMatch = new TempIdent();
                ctx.BinaryComp(isMatch, "eq", PointerType.Byte, curTablePtr, new BitCast(true,
                    new PointerType(GetRecordTableType(testType)), GetRecordTableIdent(testType), PointerType.Byte));

                ctx.Conversion(PointerType.Byte, new PointerType(PointerType.Byte), ref curTablePtr);
                temp = new TempIdent();
                ctx.Assign(temp).Argument(new ElementPointer(false, new PointerType(PointerType.Byte), curTablePtr, 1)).EndOperation();
                ctx.Load(nextTablePtr, PointerType.Byte, temp);
                ctx.Branch(isMatch, end, checkNull);

                ctx.LabelMarker(end);
                var result = new TempIdent();
                ctx.Phi(result, BooleanType.Default,
                    new Literal(0.ToString()), startLabel ?? TempIdent.Zero,
                    new Literal(0.ToString()), checkNull,
                    new Literal(1.ToString()), testPtr);

                return result;
            } else if (node.Operation is NInvocation) {
                var args = ((NInvocation) node.Operation).Args;
                var proc = (NDesignator) node.Element;

                if (!proc.IsRoot && proc.Operation is NMemberAccess) {
                    var ident = ((NMemberAccess) proc.Operation).Identifier;
                    var record = (NDesignator) proc.Element;
                    var recPtrPtr = ctx.GetDesignation(record);
                    var type = record.GetFinalType(_scope);
                    var recType = type.As<PointerType>().ResolvedType.As<RecordType>();

                    // Load the pointer to the record
                    Value recPtr = new TempIdent();
                    ctx.ResolveValue(recPtrPtr, ref recPtr, type, false);

                    // Check if the pointer is null
                    var isNull = new TempIdent();
                    ctx.BinaryComp(isNull, "eq", type, recPtr, Literal.GetDefault(PointerType.Null));

                    // Prepare labels
                    var staticDispatch = _blockLabel;
                    var dynamicDispatch = new TempIdent();
                    var dispatchEnd = new TempIdent();
                    ctx.Branch(isNull, dispatchEnd, dynamicDispatch);

                    var procType = recType.GetProcedureSignature(ident);
                    var procPtrType = new PointerType(procType);

                    Value temp = new TempIdent();

                    Value staticPtr = new BoundProcedureIdent(recType.GetProcedureDefiner(ident), ident);

                    Value dynamicPtr;
                    {
                        // Dynamic dispatching
                        ctx.LabelMarker(dynamicDispatch);

                        // Get the type of the record's vtable
                        var recTablePtrType = new PointerType(GetRecordTableType(recType));

                        // Find pointer to the vtable
                        temp = new TempIdent();
                        ctx.Assign(temp);
                        ctx.Argument(new ElementPointer(false, type, recPtr, 0, 0));
                        ctx.EndOperation();

                        // Load pointer to vtable
                        var vtablePtr = new TempIdent();
                        ctx.Load(vtablePtr, PointerType.Byte, temp);
                        temp = vtablePtr;

                        // Bitcast to correct type (from i8*)
                        vtablePtr = new TempIdent();
                        ctx.Assign(vtablePtr);
                        ctx.Argument(new BitCast(false, PointerType.Byte, temp, recTablePtrType));
                        ctx.EndOperation();

                        // Find pointer to procedure
                        temp = new TempIdent();
                        ctx.Assign(temp);
                        ctx.Argument(new ElementPointer(false, recTablePtrType, vtablePtr,
                            0, 2 + recType.GetProcedureIndex(ident)));
                        ctx.EndOperation();

                        // Load pointer to procedure
                        dynamicPtr = new TempIdent();
                        ctx.Load(dynamicPtr, PointerType.Byte, temp);
                        temp = dynamicPtr;

                        // Bitcast to correct type (from i8*)
                        dynamicPtr = new TempIdent();
                        ctx.Assign(dynamicPtr);
                        ctx.Argument(new BitCast(false, PointerType.Byte, temp, procPtrType));
                        ctx.EndOperation();

                        ctx.Branch(dispatchEnd);
                    }

                    // Select whichever pointer was found
                    ctx.LabelMarker(dispatchEnd);
                    var procPtr = new TempIdent();
                    ctx.Phi(procPtr, procPtrType, staticPtr, staticDispatch, dynamicPtr, dynamicDispatch);

                    // Finally, call the procedure
                    temp = new TempIdent();
                    ctx.Call(temp, procType, procPtr, type, recPtrPtr, args);
                    return temp;
                } else {
                    var procPtr = ctx.GetDesignation(proc);
                    var procType = proc.GetFinalType(_scope).As<ProcedureType>();
                    var temp = new TempIdent();
                    ctx.Call(temp, procType, procPtr, null, null, args);
                    return temp;
                }
            } else if (node.Operation is NMemberAccess) {
                var ident = ((NMemberAccess) node.Operation).Identifier;
                var record = (NDesignator) node.Element;
                var recPtr = ctx.GetDesignation(record);
                var type = record.GetFinalType(_scope);
                var recType = type.As<RecordType>();
                var temp = new TempIdent();
                return new ElementPointer(false, new PointerType(type), recPtr,
                    0, 1 + recType.GetFieldIndex(ident));
            } else {
                throw new NotImplementedException();
            }
        }

        static GenerationContext Conversion(this GenerationContext ctx, OberonType from, OberonType to, ref Value src)
        {
            if (from.IsArray && to.IsChar) {
                var ptr = new TempIdent();
                if (src is StringLiteral) {
                    var lit = (StringLiteral) src;
                    ctx.Assign(ptr);
                    ctx.Argument(new ElementPointer(false, lit.ConstType, lit.Identifier, 0, 0));
                    ctx.EndOperation();
                } else {
                    var temp = new TempIdent();
                    ctx.Assign(temp);
                    ctx.Argument(new ElementPointer(false, new PointerType(from), src, 0, 1));
                    ctx.EndOperation();
                    ctx.Load(ptr, new PointerType(CharType.Default), temp);
                }
                src = new TempIdent();
                return ctx.Load(src, to, ptr);
            }

            if (from.Equals(to)) return ctx;

            if (src is Literal && to.IsNumeric && from.IsNumeric) {
                if (to.IsReal && from.IsInteger) {
                    src = new Literal(int.Parse(src.ToString()).ToString("e"));
                } else if (to.IsReal && from.IsReal && to.As<RealType>().Range == RealRange.LongReal && from.As<RealType>().Range == RealRange.Real) {
                    
                }
                return ctx;
            } else if (from.IsPointer && to.IsPointer) {
                if (from.As<PointerType>().ResolvedType.Equals(to.As<PointerType>().ResolvedType)) return ctx;
                var temp = new TempIdent();
                ctx.Assign(temp).Argument(new BitCast(false, from, src, to));
                ctx.EndOperation();
                src = temp;
                return ctx;
            } else if (from.IsPointer && to.IsInteger) {
                var temp = new TempIdent();
                ctx.Assign(temp).Keyword("ptrtoint").Argument(from, src).Keyword(" to").Argument(to).EndOperation();
                src = temp;
                return ctx;
            }

            var tsrc = src;
            src = new TempIdent();


            if (from.IsInteger && to.IsInteger) {
                IntegerType fi = from.As<IntegerType>(), ti = to.As<IntegerType>();
                if (fi.Range < ti.Range) {
                    return ctx.Conversion(src, "sext", from, tsrc, to);
                }
            } else if (from.IsInteger && to.IsReal) {
                return ctx.Conversion(src, "sitofp", from, tsrc, to);
            } else if (from.IsReal && to.IsReal) {
                RealType fr = from.As<RealType>(), tf = to.As<RealType>();
                if (fr.Range < tf.Range) {
                    return ctx.Conversion(src, "fpext", from, tsrc, to);
                }
            }

            throw new InvalidOperationException(String.Format("No conversion between {0} to {1} defined", from.ToString(), to.ToString()));
        }

        static GenerationContext ResolveValue(this GenerationContext ctx, Value val, ref Value dest, OberonType type, bool isConstant)
        {
            if (val is QualIdent) {
                var ident = (QualIdent) val;
                if (ident.Declaration.IsVariable) {
                    return ctx.Load(dest, type, val);
                }
            } else if (val is ElementPointer) {
                ElementPointer ptr = (ElementPointer) val;
                if (!isConstant) {
                    val = new TempIdent();
                    ctx.Assign(val).Argument(ptr).EndOperation();
                }
                return ctx.Load(dest, type, val);
            }

            dest = val;
            return ctx;
        }

        static GenerationContext Factor(this GenerationContext ctx, NFactor node, ref Value dest, OberonType type)
        {
            if (node.Inner is NDesignator) {
                var val = ctx.GetDesignation((NDesignator) node.Inner);
                var ntype = node.Inner.GetFinalType(_scope);
                if (type.IsPointer && val is QualIdent && ntype.IsArray && !type.CanTestEquality(ntype)
                    && type.As<PointerType>().ResolvedType.Equals(ntype.As<ArrayType>().ElementType)) {

                    var temp = new TempIdent();
                    ctx.Assign(temp);
                    ctx.Argument(new ElementPointer(false, new PointerType(ntype), val, 0, 1));
                    ctx.EndOperation();
                    return ctx.Load(dest, new PointerType(CharType.Default), temp);
                }
                if (type.IsPointer && (val is QualIdent || val is ElementPointer) && !type.CanTestEquality(ntype)
                    && type.CanTestEquality(new PointerType(ntype))) {

                    if (val is ElementPointer && !node.IsConstant(_scope)) {
                        dest = new TempIdent();
                        ctx.Assign(dest).Argument(val).EndOperation();
                    } else {
                        dest = val;
                    }

                    return ctx;
                }
                return ctx.ResolveValue(val, ref dest, type, node.IsConstant(_scope));
            }

            if (node.Inner is NUnary) {
                var unary = (NUnary) node.Inner;
                Value val = ctx.PrepareOperand(unary.Factor, type, new TempIdent());
                Value zero = new Literal(0.ToString());
                switch (unary.Operator) {
                    case UnaryOperator.Identity:
                        dest = val;
                        return ctx;
                    case UnaryOperator.Negation:
                        if (type.IsNumeric && val is Literal) {
                            var str = val.ToString();
                            if (str.StartsWith("-")) {
                                dest = new Literal(str.Substring(1));
                            } else {
                                dest = new Literal(String.Format("-{0}", str));
                            }
                            return ctx;
                        }
                        ctx.Conversion(IntegerType.Integer, type, ref zero);
                        return ctx.BinaryOp(dest, "sub", "fsub", type, zero, val);
                    case UnaryOperator.Not:
                        ctx.Conversion(IntegerType.Integer, type, ref zero);
                        return ctx.BinaryOp(dest, "xor", type, zero, val);
                    default:
                        throw new NotImplementedException();
                }
            }

            if (node.Inner is NExpr) {
                return ctx.Expr((NExpr) node.Inner, ref dest, type);
            }
            
            if (dest is TempIdent) {
                if (node.Inner is NNil) {
                    dest = Literal.GetDefault(PointerType.Null);
                    return ctx;
                } else if (node.Inner is NNumber) {
                    dest = new Literal((NNumber) node.Inner);
                    return ctx;
                } else if (node.Inner is NBool) {
                    dest = new Literal(node.Inner.String.ToLower());
                    return ctx;
                } else if (node.Inner is NString) {
                    GetStringIdent(node.Inner.String);
                    dest = new StringLiteral(node.Inner.String);
                    return ctx;
                } else if (node.Inner is NDesignator && ((NDesignator) node.Inner).IsRoot) {
                    dest = new QualIdent((NQualIdent) ((NDesignator) node.Inner).Element);
                    return ctx;
                }
            }

            throw new NotImplementedException("No rule to generate factor of type " + node.Inner.GetType());
        }

        static Value PrepareOperand(this GenerationContext ctx, ExpressionElement node, OberonType type, Value dest)
        {
            var temp = new TempIdent();
            var val = (Value) temp;
            var ntype = node.GetFinalType(_scope);
            if (!type.CanTestEquality(ntype) && (type.Equals(new PointerType(ntype)) || (type.IsPointer
                && ntype.IsArray && type.As<PointerType>().ResolvedType.Equals(ntype.As<ArrayType>().ElementType)))) {
                ntype = type;
            }
            if (node is NFactor) {
                ctx.Factor((NFactor) node, ref val, ntype);
            } else if (node is NTerm) {
                ctx.Term((NTerm) node, ref val, ntype);
            } else if (node is NSimpleExpr) {
                ctx.SimpleExpr((NSimpleExpr) node, ref val, ntype);
            } else if (node is NExpr) {
                ctx.Expr((NExpr) node, ref val, ntype);
            }
            ctx.Conversion(ntype, type, ref val);

            return val;
        }

        static GenerationContext Term(this GenerationContext ctx, NTerm node, ref Value dest, OberonType type)
        {
            if (node.Operator == TermOperator.None) {
                return ctx.Factor(node.Factor, ref dest, type);
            } else if (node.Operator == TermOperator.And) {
                var a = ctx.PrepareOperand(node.Prev, type, dest);
                if (node.Factor.IsConstant(_scope)) {
                    return ctx.BinaryOp(dest, "and", type, a, ctx.PrepareOperand(node.Factor, type, dest));
                } else {
                    var initBlock = _blockLabel;

                    var testB = new TempIdent();
                    var end = new TempIdent();
                    ctx.Branch(a, testB, end);

                    ctx.LabelMarker(testB);
                    var b = ctx.PrepareOperand(node.Factor, type, dest);
                    if (!(b is TempIdent)) {
                        var temp = new TempIdent();
                        ctx.Assign(temp).Argument(BooleanType.Default, b).EndOperation();
                        b = temp;
                    }
                    testB = _blockLabel;
                    ctx.Branch(end);

                    ctx.LabelMarker(end);
                    return ctx.Phi(dest, BooleanType.Default,
                        new Literal(0.ToString()), initBlock,
                        b, testB);
                }
            } else {
                Value a = ctx.PrepareOperand(node.Prev, type, dest), b = ctx.PrepareOperand(node.Factor, type, dest);
                switch (node.Operator) {
                    case TermOperator.Multiply:
                        return ctx.BinaryOp(dest, "mul", "fmul", type, a, b);
                    case TermOperator.Divide:
                        return ctx.BinaryOp(dest, "sdiv", "fdiv", type, a, b);
                    case TermOperator.IntDivide:
                        return ctx.BinaryOp(dest, "sdiv", type, a, b);
                    case TermOperator.Modulo:
                        return ctx.BinaryOp(dest, "srem", type, a, b);
                    default:
                        throw new NotImplementedException();
                }
            }
        }

        static GenerationContext SimpleExpr(this GenerationContext ctx, NSimpleExpr node, ref Value dest, OberonType type)
        {
            if (node.Operator == SimpleExprOperator.None) {
                return ctx.Term(node.Term, ref dest, type);
            } else if (node.Operator == SimpleExprOperator.Or) {
                var a = ctx.PrepareOperand(node.Prev, type, dest);
                if (node.Term.IsConstant(_scope)) {
                    return ctx.BinaryOp(dest, "or", type, a, ctx.PrepareOperand(node.Term, type, dest));
                } else {
                    var initBlock = _blockLabel;

                    var testB = new TempIdent();
                    var end = new TempIdent();
                    ctx.Branch(a, end, testB);

                    ctx.LabelMarker(testB);
                    var b = ctx.PrepareOperand(node.Term, type, dest);
                    if (!(b is TempIdent)) {
                        var temp = new TempIdent();
                        ctx.Assign(temp).Argument(BooleanType.Default, b).EndOperation();
                        b = temp;
                    }
                    testB = _blockLabel;
                    ctx.Branch(end);

                    ctx.LabelMarker(end);
                    return ctx.Phi(dest, BooleanType.Default,
                        new Literal(1.ToString()), initBlock,
                        b, testB);
                }
            } else {
                Value a = ctx.PrepareOperand(node.Prev, type, dest), b = ctx.PrepareOperand(node.Term, type, dest);
                switch (node.Operator) {
                    case SimpleExprOperator.Add:
                        return ctx.BinaryOp(dest, "add", "fadd", type, a, b);
                    case SimpleExprOperator.Subtract:
                        return ctx.BinaryOp(dest, "sub", "fsub", type, a, b);
                    default:
                        throw new NotImplementedException();
                }
            }
        }

        static GenerationContext Expr(this GenerationContext ctx, NExpr node, ref Value dest, OberonType type)
        {
            if (node.Operator == ExprOperator.None) {
                return ctx.SimpleExpr(node.SimpleExpr, ref dest, type);
            } else {
                var lt = node.Prev.GetFinalType(_scope);
                var rt = node.SimpleExpr.GetFinalType(_scope);
                OberonType ntype = lt;

                if (lt.IsNumeric && rt.IsNumeric) {
                    ntype = NumericType.Largest(lt.As<NumericType>(), rt.As<NumericType>());
                }

                Value a = ctx.PrepareOperand(node.Prev, ntype, dest), b = ctx.PrepareOperand(node.SimpleExpr, ntype, dest);
                switch (node.Operator) {
                    case ExprOperator.Equals:
                        return ctx.BinaryComp(dest, "eq", "oeq", ntype, a, b);
                    case ExprOperator.NotEquals:
                        return ctx.BinaryComp(dest, "ne", "one", ntype, a, b);
                    case ExprOperator.GreaterThan:
                        return ctx.BinaryComp(dest, "sgt", "ogt", ntype, a, b);
                    case ExprOperator.GreaterThanOrEqual:
                        return ctx.BinaryComp(dest, "sge", "oge", ntype, a, b);
                    case ExprOperator.LessThan:
                        return ctx.BinaryComp(dest, "slt", "olt", ntype, a, b);
                    case ExprOperator.LessThanOrEqual:
                        return ctx.BinaryComp(dest, "sle", "ole", ntype, a, b);
                    default:
                        throw new NotImplementedException();
                }
            }
        }
    }
}
