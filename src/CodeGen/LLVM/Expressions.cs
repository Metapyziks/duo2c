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
                    return ctx.WriteConversion(dest, "sext", from, tsrc, to);
                }
            } else if (from is IntegerType && to is RealType) {
                return ctx.WriteConversion(dest, "sitofp", from, tsrc, to);
            } else if (from is RealType && to is RealType) {
                RealType fr = (RealType) from, tf = (RealType) to;
                if (fr.Range < tf.Range) {
                    return ctx.WriteConversion(dest, "fpext", from, tsrc, to);
                }
            }

            throw new InvalidOperationException("No conversion between " + from.ToString() + " to " + to.ToString() + "defined");
        }

        static GenerationContext Factor(this GenerationContext ctx, NFactor node, ref Value dest, OberonType type)
        {
            if (node.Inner is NDesignator && ((NDesignator) node.Inner).IsRoot) {
                return ctx.WriteOperation(dest, "load", new PointerType(type), new QualIdent((NQualIdent) ((NDesignator) node.Inner).Element));
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
                Value l = ctx.PrepareOperand(node.Prev, type, dest), r = ctx.PrepareOperand(node.Factor, type, dest);
                switch (node.Operator) {
                    case TermOperator.Multiply:
                        return ctx.WriteOperation(dest, (type.IsReal ? "fmul" : "mul"), type, l, r);
                    case TermOperator.Divide:
                        return ctx.WriteOperation(dest, "fdiv", type, l, r);
                    case TermOperator.IntDivide:
                        return ctx.WriteOperation(dest, "sdiv", type, l, r);
                    case TermOperator.Modulo:
                        return ctx.WriteOperation(dest, "srem", type, l, r);
                    case TermOperator.And:
                        return ctx.WriteOperation(dest, "and", type, l, r);
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
                Value l = ctx.PrepareOperand(node.Prev, type, dest), r = ctx.PrepareOperand(node.Term, type, dest);
                switch (node.Operator) {
                    case SimpleExprOperator.Add:
                        return ctx.WriteOperation(dest, (type.IsReal ? "fadd" : "add"), type, l, r);
                    case SimpleExprOperator.Subtract:
                        return ctx.WriteOperation(dest, (type.IsReal ? "fsub" : "sub"), type, l, r);
                    case SimpleExprOperator.Or:
                        return ctx.WriteOperation(dest, "or", type, l, r);
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

                Value l = ctx.PrepareOperand(node.Prev, ntype, dest), r = ctx.PrepareOperand(node.SimpleExpr, ntype, dest);
                switch (node.Operator) {
                    case ExprOperator.Equals:
                        return ctx.WriteOperation(dest, (ntype.IsReal ? "fcmp oeq" : "icmp eq"), ntype, l, r);
                    case ExprOperator.NotEquals:
                        return ctx.WriteOperation(dest, (ntype.IsReal ? "fcmp one" : "icmp ne"), ntype, l, r);
                    case ExprOperator.GreaterThan:
                        return ctx.WriteOperation(dest, (ntype.IsReal ? "fcmp ogt" : "icmp sgt"), ntype, l, r);
                    case ExprOperator.GreaterThanOrEqual:
                        return ctx.WriteOperation(dest, (ntype.IsReal ? "fcmp oge" : "icmp sge"), ntype, l, r);
                    case ExprOperator.LessThan:
                        return ctx.WriteOperation(dest, (ntype.IsReal ? "fcmp olt" : "icmp slt"), ntype, l, r);
                    case ExprOperator.LessThanOrEqual:
                        return ctx.WriteOperation(dest, (ntype.IsReal ? "fcmp ole" : "icmp sle"), ntype, l, r);
                    default:
                        throw new NotImplementedException();
                }
            }
        }
    }
}
