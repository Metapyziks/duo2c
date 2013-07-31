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
        static GenerationContext Node(this GenerationContext ctx, NDesignator node)
        {
            var ident = ctx.GetDesignation(node);
            return ctx.Write(ident.ToString());
        }

        static Value GetDesignation(this GenerationContext ctx, NDesignator node)
        {
            if (node.IsRoot) {
                return new QualIdent((NQualIdent) node.Element);
            } else {
                throw new NotImplementedException();
            }
        }

        static GenerationContext Conversion(this GenerationContext ctx, Value dest, OberonType from, OberonType to, ref Value src)
        {
            if (from.Equals(to)) return ctx;

            if (src is Literal && to.IsNumeric && from.IsNumeric) {
                if (to.IsReal && from.IsInteger) {
                    src = new Literal(src.ToString() + ".0");
                }
                return ctx;
            }

            var tsrc = src;
            src = dest = new TempIdent();

            if (from is IntegerType && to is IntegerType) {
                IntegerType fi = (IntegerType) from, ti = (IntegerType) to;
                if (fi.Range < ti.Range) {
                    return ctx.Conversion(dest, "sext", from, tsrc, to);
                }
            } else if (from is IntegerType && to is RealType) {
                return ctx.Conversion(dest, "sitofp", from, tsrc, to);
            } else if (from is RealType && to is RealType) {
                RealType fr = (RealType) from, tf = (RealType) to;
                if (fr.Range < tf.Range) {
                    return ctx.Conversion(dest, "fpext", from, tsrc, to);
                }
            }

            throw new InvalidOperationException("No conversion between " + from.ToString() + " to " + to.ToString() + "defined");
        }

        static GenerationContext Factor(this GenerationContext ctx, NFactor node, ref Value dest, OberonType type)
        {
            if (node.Inner is NDesignator && ((NDesignator) node.Inner).IsRoot) {
                return ctx.Load(dest, type, ctx.GetDesignation((NDesignator) node.Inner));
            }

            if (node.Inner is NExpr) {
                return ctx.Expr((NExpr) node.Inner, ref dest, type);
            } else if (dest is TempIdent) {
                if (node.Inner is NNumber) {
                    dest = new Literal((NNumber) node.Inner);
                    return ctx;
                } else if (node.Inner is NBool) {
                    dest = new Literal(node.Inner.String.ToLower());
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
            if (node is NFactor) {
                ctx.Factor((NFactor) node, ref val, ntype);
            } else if (node is NTerm) {
                ctx.Term((NTerm) node, ref val, ntype);
            } else if (node is NSimpleExpr) {
                ctx.SimpleExpr((NSimpleExpr) node, ref val, ntype);
            } else if (node is NExpr) {
                ctx.Expr((NExpr) node, ref val, ntype);
            }
            ctx.Conversion(temp, ntype, type, ref val);
            return val;
        }

        static GenerationContext Term(this GenerationContext ctx, NTerm node, ref Value dest, OberonType type)
        {
            if (node.Operator == TermOperator.None) {
                return ctx.Factor(node.Factor, ref dest, type);
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
                    case TermOperator.And:
                        return ctx.BinaryOp(dest, "and", type, a, b);
                    default:
                        throw new NotImplementedException();
                }
            }
        }

        static GenerationContext SimpleExpr(this GenerationContext ctx, NSimpleExpr node, ref Value dest, OberonType type)
        {
            if (node.Operator == SimpleExprOperator.None) {
                return ctx.Term(node.Term, ref dest, type);
            } else {
                Value a = ctx.PrepareOperand(node.Prev, type, dest), b = ctx.PrepareOperand(node.Term, type, dest);
                switch (node.Operator) {
                    case SimpleExprOperator.Add:
                        return ctx.BinaryOp(dest, "add", "fadd", type, a, b);
                    case SimpleExprOperator.Subtract:
                        return ctx.BinaryOp(dest, "sub", "fsub", type, a, b);
                    case SimpleExprOperator.Or:
                        return ctx.BinaryOp(dest, "or", type, a, b);
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
                    ntype = NumericType.Largest((NumericType) lt, (NumericType) rt);
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
