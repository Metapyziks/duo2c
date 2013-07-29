using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using DUO2C.Nodes;
using DUO2C.Nodes.Oberon2;
using DUO2C.Semantics;

namespace DUO2C.CodeGen
{
    public static class IntermediaryCodeGenerator
    {
        abstract class Value
        {
            public override bool Equals(object obj)
            {
                if (obj == null) return false;

                if (this is TempIdent || obj is TempIdent) {
                    return this == obj;
                } else {
                    return GetType() == obj.GetType() && ToString().Equals(obj.ToString());
                }
            }

            public override int GetHashCode()
            {
                return GetType().GetHashCode() ^ ToString().GetHashCode();
            }
        }

        class Literal : Value
        {
            String _str;

            public Literal(String str)
            {
                _str = str;
            }

            public Literal(NNumber num)
            {
                _str = num.Inner.String;
            }

            public override string ToString()
            {
                return _str;
            }
        }

        class QualIdent : Value
        {
            NQualIdent _ident;

            public QualIdent(String ident)
            {
                _ident = new NQualIdent(ident, null);
            }

            public QualIdent(NQualIdent ident)
            {
                _ident = ident;
            }

            public override string ToString()
            {
                if (_ident.Module != null || _scope.GetSymbolDecl(_ident.Identifier, null).Visibility != AccessModifier.Private) {
                    return String.Format("@{0}.{1}", _ident.Module ?? _module.Identifier, _ident.Identifier);
                } else {
                    return String.Format("@{0}", _ident.Identifier);
                }
            }
        }

        class TempIdent : Value
        {
            static int _sLast = 0;

            public static void Reset()
            {
                _sLast = 0;
            }

            int _id;

            public TempIdent()
            {
                _id = 0;
            }

            public override string ToString()
            {
                if (_id == 0) {
                    _id = ++_sLast;
                }
                
                return String.Format("%{0}", _id);
            }
        }

        static Dictionary<Type, MethodInfo> _nodeGens;
        static NModule _module;
        static Scope _scope;

        public static String Generate(NModule module, Guid uniqueID)
        {
            _nodeGens = typeof(IntermediaryCodeGenerator).GetMethods(BindingFlags.Static | BindingFlags.NonPublic).Where(x => {
                var pars = x.GetParameters();
                if (x.Name == "WriteNode" && pars.Length == 2) {
                    if (pars[0].ParameterType != typeof(GenerationContext) || x.ReturnType != typeof(GenerationContext)) {
                        return false;
                    }
                    if (!pars[1].ParameterType.Extends(typeof(SubstituteNode)) || pars[1].ParameterType == typeof(SubstituteNode)) {
                        return false;
                    }
                    return true;
                } else {
                    return false;
                }
            }).ToDictionary(x => x.GetParameters().Last().ParameterType);

            var ctx = new GenerationContext();
            ctx.WriteModule(module, uniqueID);
            return ctx.GeneratedCode;
        }

        static GenerationContext WriteModule(this GenerationContext ctx, NModule module, Guid uniqueID)
        {
            ctx.Write("; Generated {0}", DateTime.Now.ToString()).NewLine();
            ctx.Write("; GlobalUID {0}", uniqueID.ToString()).NewLine();
            ctx.Write(";").NewLine();
            ctx.Write("; LLVM IR file for module \"{0}\"", module.Identifier).NewLine();
            ctx.Write(";").NewLine();
            ctx.Write("; WARNING: This file is automatically").NewLine();
            ctx.Write("; generated and should not be edited").NewLine().NewLine();

            ctx.Write("target datalayout = \"e-p:32:32:32-i1:8:8-i8:8:8-i16:16:16-i32:32:32-i64:64:64-f32:32:32-f64:64:64-f80:128:128-v64:64:64-v128:128:128-a0:0:64-f80:32:32-n8:16:32-S32\"").NewLine().NewLine();

            ctx.Write("@.printistr = private constant [3 x i8] c\"%i\\00\", align 1").NewLine();
            ctx.Write("@.printfstr = private constant [3 x i8] c\"%f\\00\", align 1").NewLine();
            ctx.Write("@.printnstr = private constant [2 x i8] c\"\\0A\\00\", align 1").NewLine();
            ctx.Write("@.truestr = private constant [5 x i8] c\"TRUE\\00\", align 1").NewLine();
            ctx.Write("@.falsestr = private constant [6 x i8] c\"FALSE\\00\", align 1").NewLine().NewLine();

            ctx.Write("declare i32 @printf(i8*, ...) nounwind").NewLine().NewLine();

            ctx.Write("; Begin type aliases").NewLine().Enter(2).NewLine();
            ctx.WriteTypeDecl("CHAR", IntegerType.Byte);
            ctx.WriteTypeDecl("SET", IntegerType.LongInt);

            _module = module;
            _scope = module.Type.Scope;

            foreach (var kv in _scope.GetTypes().Where(x => !(x.Value.Type is ProcedureType))) {
                ctx.WriteTypeDecl(kv.Key, kv.Value.Type);
            }
            ctx.Leave().Write("; End type aliases").NewLine().NewLine();

            foreach (var v in _scope.GetSymbols()) {
                if (!v.Value.Type.IsProcedure) {
                    ctx.WriteGlobalDecl(new QualIdent(v.Key), v.Value.Type);
                }
            }

            if (_scope.GetSymbols().Count() > 0) {
                ctx.NewLine();
            }

            ctx.Write("define i32 @").Write("main() {").Enter().NewLine().NewLine();
            if (module.Body != null) {
                ctx.WriteStatements(module.Body.Statements.Select(x => x.Inner));
            }
            ctx.Write("ret i32 0").NewLine().Leave().Write("}").NewLine();

            return ctx.Write("; Module end").NewLine();
        }

        static GenerationContext WriteTypeIdent(this GenerationContext ctx, String identifier)
        {
            return ctx.Write("%_type{0}", identifier);
        }

        static GenerationContext WriteGlobalDecl(this GenerationContext ctx, QualIdent ident, OberonType type)
        {
            if (type.IsBool) {
                return ctx.WriteOperation(ident, "global", type, "false");
            } else if (type.IsInteger) {
                return ctx.WriteOperation(ident, "global", type, "0");
            } else if (type.IsReal) {
                return ctx.WriteOperation(ident, "global", type, "0.0");
            } else {
                throw new NotImplementedException();
            }
        }

        static GenerationContext WriteTypeDecl(this GenerationContext ctx, String identifier, OberonType type)
        {
            ctx.Write("; {0} = ", identifier).Write(type.ToString());
            ctx.NewLine();
            ctx.WriteTypeIdent(identifier).Write(" ").Anchor().Write("= type ").WriteType(type);
            return ctx.NewLine().NewLine();
        }
        
        static GenerationContext WriteType(this GenerationContext ctx, OberonType type)
        {
            if (type is RecordType) {
                var rec = (RecordType) type;
                ctx.Write("{");
                if (rec.SuperRecordName != null) {
                    ctx.WriteTypeIdent(rec.SuperRecordName);
                    if (rec.Fields.Count() > 0) ctx.Write(", ");
                }
                foreach (var fl in rec.Fields.Where(x => !(x.Type is ProcedureType))) {
                    if (fl != rec.Fields.First()) ctx.Write(", ");
                    ctx.WriteType(fl.Type);
                }
                return ctx.Write("}");
            } else if (type is PointerType) {
                var ptr = (PointerType) type;
                return ctx.WriteType(ptr.ResolvedType).Write("*");
            } else if (type is ArrayType) {
                var at = (ArrayType) type;
                return ctx.Write("{").WriteType(IntegerType.Integer).Write(", ").WriteType(at.ElementType).Write("*}");
            } else if (type is IntegerType) {
                var it = (IntegerType) type;
                return ctx.Write("i{0}", (int) it.Range * 8);
            } else if (type is RealType) {
                var rt = (RealType) type;
                if (rt.Range == RealRange.Real) {
                    return ctx.Write("float");
                } else {
                    return ctx.Write("double");
                }
            } else if (type is SetType) {
                return ctx.WriteTypeIdent("SET");
            } else if (type is BooleanType) {
                return ctx.Write("i1");
            } else if (type is CharType) {
                return ctx.WriteTypeIdent("CHAR");
            } else if (type is UnresolvedType) {
                var ut = (UnresolvedType) type;
                return ctx.WriteTypeIdent(ut.Identifier);
            } else {
                throw new NotImplementedException("No rule to generate LLVMIR for type " + type.GetType().FullName);
            }
        }

        static GenerationContext WriteStatements(this GenerationContext ctx, IEnumerable<Statement> statements)
        {
            foreach (var stmnt in statements) {
                ctx.Write("; {0}", stmnt.String).Enter(0).NewLine();
                ctx.WriteNode(stmnt);
                ctx.NewLine().Leave();
            }
            return ctx;
        }

        static GenerationContext WriteAssignLeft(this GenerationContext ctx, Value ident)
        {
            return ctx.Write(ident.ToString()).Write(" ").Anchor().Write("= ");
        }

        static GenerationContext WriteConversion(this GenerationContext ctx, Value dest, String op, OberonType from, Value src, OberonType to)
        {
            return ctx.WriteAssignLeft(dest).Write("{0} ", op).Anchor().WriteType(from).Write(" ").Anchor().Write("{0} ", src).Anchor().Write("to ").WriteType(to).NewLine();
        }

        static GenerationContext WriteOperation(this GenerationContext ctx, Value dest, String op, OberonType type, params Value[] args)
        {
            ctx.WriteAssignLeft(dest).Write("{0} ", op).Anchor().WriteType(type).Write(" ").Anchor();
            foreach (var arg in args) {
                ctx.Write(arg.ToString());
                if (arg != args.Last()) {
                    ctx.Write(", ").Anchor();
                }
            }
            return ctx.NewLine();
        }

        static GenerationContext WriteOperation(this GenerationContext ctx, Value dest, String op, params Object[] args)
        {
            return ctx.WriteAssignLeft(dest).WriteOperation(op, args);
        }

        static GenerationContext WriteOperation(this GenerationContext ctx, String op, params Object[] args)
        {
            ctx.Write("{0} ", op).Anchor();
            foreach (var arg in args) {
                if (arg is OberonType) {
                    ctx.WriteType((OberonType) arg).Write(" ").Anchor();
                } else {
                    ctx.Write(arg.ToString());
                    if (arg != args.Last()) {
                        ctx.Write(", ").Anchor();
                    }
                }
            }
            return ctx.NewLine();
        }

        static GenerationContext WriteConversion(this GenerationContext ctx, Value dest, OberonType from, OberonType to, ref Value src)
        {
            if (from.Equals(to)) return ctx;

            if (src is Literal && from.IsInteger && to.IsReal) {
                src = new Literal(src.ToString() + ".0");
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

        static GenerationContext WriteNode(this GenerationContext ctx, SubstituteNode node)
        {
            if (_nodeGens.ContainsKey(node.GetType())) {
                return (GenerationContext) _nodeGens[node.GetType()].Invoke(null, new object[] { ctx, node });
            } else {
                throw new NotImplementedException(String.Format("No method of generating LLVM IR for nodes of type '{0}' found", node.GetType().Name));
            }
        }

        static GenerationContext WriteNode(this GenerationContext ctx, NInteger node)
        {
            return ctx.Write(node.Value.ToString());
        }

        static GenerationContext WriteNode(this GenerationContext ctx, NNumber node)
        {
            return ctx.WriteNode(node.Inner);
        }

        static GenerationContext WriteNode(this GenerationContext ctx, NDesignator node)
        {
            var ident = ctx.GetDesignation(node);
            return ctx.Write(ident.ToString());
        }

        static GenerationContext WriteAssignHack(this GenerationContext ctx, Value dest, OberonType type, Value val)
        {
            return ctx.WriteOperation(dest, "select", BooleanType.Default, "true", type, val, type, "undef");
        }

        static GenerationContext WriteFactor(this GenerationContext ctx, NFactor node, ref Value dest, OberonType type)
        {
            if (node.Inner is NDesignator && ((NDesignator) node.Inner).IsRoot) {
                return ctx.WriteOperation(dest, "load", new PointerType(type), new QualIdent((NQualIdent) ((NDesignator) node.Inner).Element));
            }

            if (node.Inner is NExpr) {
                return ctx.WriteExpr((NExpr) node.Inner, ref dest, type);
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
            } else if (node.Inner is NNumber) {
                return ctx.WriteAssignHack(dest, type, new Literal((NNumber) node.Inner));
            } else if (node.Inner is NBool) {
                return ctx.WriteAssignHack(dest, type, new Literal("0"));
            }

            throw new NotImplementedException("No rule to generate factor of type " + node.Inner.GetType());
        }

        static Value PrepareOperand(this GenerationContext ctx, ExpressionElement node, OberonType type, Value dest)
        {
            var temp = new TempIdent();
            var val = (Value) temp;
            var ntype = node.GetFinalType(_scope);
            if (node is NFactor) {
                ctx.WriteFactor((NFactor) node, ref val, ntype);
            } else if (node is NTerm) {
                ctx.WriteTerm((NTerm) node, ref val, ntype);
            } else if (node is NSimpleExpr) {
                ctx.WriteSimpleExpr((NSimpleExpr) node, ref val, ntype);
            } else if (node is NExpr) {
                ctx.WriteExpr((NExpr) node, ref val, ntype);
            }
            ctx.WriteConversion(temp, ntype, type, ref val);
            if (val.Equals(dest)) {
                ctx.WriteAssignHack(temp, type, val);
                return temp;
            }
            return val;
        }

        static GenerationContext WriteTerm(this GenerationContext ctx, NTerm node, ref Value dest, OberonType type)
        {
            if (node.Operator == TermOperator.None) {
                return ctx.WriteFactor(node.Factor, ref dest, type);
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

        static GenerationContext WriteSimpleExpr(this GenerationContext ctx, NSimpleExpr node, ref Value dest, OberonType type)
        {
            if (node.Operator == SimpleExprOperator.None) {
                return ctx.WriteTerm(node.Term, ref dest, type);
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

        static GenerationContext WriteExpr(this GenerationContext ctx, NExpr node, ref Value dest, OberonType type)
        {
            if (node.Operator == ExprOperator.None) {
                return ctx.WriteSimpleExpr(node.SimpleExpr, ref dest, type);
            } else {
                throw new NotImplementedException();
            }
        }

        static Value GetDesignation(this GenerationContext ctx, NDesignator node)
        {
            if (node.IsRoot) {
                return new QualIdent((NQualIdent) node.Element);
            } else {
                throw new NotImplementedException();
            }
        }

        static GenerationContext WriteNode(this GenerationContext ctx, NAssignment node)
        {
            var dest = ctx.GetDesignation(node.Assignee);
            var temp = (Value) new TempIdent();
            var type = node.Assignee.GetFinalType(_scope);
            ctx.WriteExpr(node.Expression, ref temp, type);
            return ctx.WriteOperation("store", type, temp, new PointerType(type), dest);
        }

        static GenerationContext WriteNode(this GenerationContext ctx, NInvocStmnt node)
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
                        ctx.WriteOperation(temp, "select", type, src, "i8* getelementptr inbounds ([5 x i8]* @.truestr, i32 0, i32 0)", "i8* getelementptr inbounds ([6 x i8]* @.falsestr, i32 0, i32 0)");
                        return ctx.WriteAssignLeft(tmp).Write("call ").WriteType(IntegerType.Integer).Write(" (i8*, ...)* @printf(i8* {0}) nounwind", temp).NewLine();
                    case "Byte":
                    case "ShortInt":
                    case "Integer":
                    case "LongInt":
                        return ctx.WriteAssignLeft(tmp).Write("call ").WriteType(IntegerType.Integer).Write(" (i8*, ...)* @printf(i8* getelementptr inbounds ([3 x i8]* @.printistr, i32 0, i32 0), ").WriteType(type).Write(" {0}) nounwind", src).NewLine();
                    case "Real":
                    case "LongReal":
                        return ctx.WriteAssignLeft(tmp).Write("call ").WriteType(IntegerType.Integer).Write(" (i8*, ...)* @printf(i8* getelementptr inbounds ([3 x i8]* @.printfstr, i32 0, i32 0), ").WriteType(type).Write(" {0}) nounwind", src).NewLine();
                    case "Ln":
                        return ctx.WriteAssignLeft(tmp).Write("call ").WriteType(IntegerType.Integer).Write(" (i8*, ...)* @printf(i8* getelementptr inbounds ([2 x i8]* @.printnstr, i32 0, i32 0)) nounwind").NewLine();
                }
            }

            throw new NotImplementedException("Invocation statements not yet implented");
        }
    }
}
