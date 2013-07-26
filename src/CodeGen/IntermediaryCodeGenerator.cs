﻿using System;
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
        abstract class Ident
        {

        }

        class QualIdent : Ident
        {
            NQualIdent _ident;

            public QualIdent(NQualIdent ident)
            {
                _ident = ident;
            }

            public override string ToString()
            {
                return String.Format("%{0}", _ident.Identifier);
            }
        }

        class TempIdent : Ident, IDisposable
        {
            static SortedSet<int> _sUsed = new SortedSet<int>();

            public static void Reset()
            {
                _sUsed.Clear();
            }

            public static TempIdent Create()
            {
                int id = 0;
                foreach (int used in _sUsed) {
                    if (id == used) ++id;
                }

                _sUsed.Add(id);
                return new TempIdent(id);
            }

            int _id;

            private TempIdent(int id)
            {
                _id = id;
            }

            public override string ToString()
            {
                if (_id > -1) {
                    return String.Format("%_temp{0}", _id);
                } else {
                    throw new ObjectDisposedException("TempIdent");
                }
            }
        
            public void Dispose()
            {
                _sUsed.Remove(_id);
 	            _id = -1;
            }
        }

        static Dictionary<Type, MethodInfo> _nodeGens;
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

            ctx.Write("; Begin type aliases").NewLine().Enter(2).NewLine();
            ctx.WriteTypeDecl("CHAR", IntegerType.ShortInt);
            ctx.WriteTypeDecl("SET", IntegerType.LongInt);

            _scope = module.Type.Scope;

            foreach (var kv in _scope.GetTypes().Where(x => !(x.Value.Type is ProcedureType))) {
                ctx.WriteTypeDecl(kv.Key, kv.Value.Type);
            }
            ctx.Leave().Write("; End type aliases").NewLine().NewLine();

            if (module.Body != null) {
                ctx.WriteStatements(module.Body.Statements.Select(x => x.Inner));
            }

            return ctx.Write("; Module end").NewLine();
        }

        static GenerationContext WriteTypeIdent(this GenerationContext ctx, String identifier)
        {
            return ctx.Write("%_type{0}", identifier);
        }

        static GenerationContext WriteTypeDecl(this GenerationContext ctx, String identifier, OberonType type)
        {
            ctx.Write("; {0} = ", identifier).Write(type.ToString());
            ctx.NewLine();
            ctx.WriteTypeIdent(identifier).Anchor().Write(" = type ").WriteType(type);
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
            ctx.Write("; Begin statements").NewLine().Enter(2).NewLine();

            foreach (var stmnt in statements) {
                ctx.Write("; {0}", stmnt.String).NewLine();
                ctx.WriteNode(stmnt);
                ctx.NewLine();
            }

            return ctx.Leave().Write("; End statements").NewLine().NewLine();
        }

        static GenerationContext WriteAssignLeft(this GenerationContext ctx, Ident ident)
        {
            return ctx.Write(ident.ToString()).Anchor().Write(" = ").Anchor();
        }

        static GenerationContext WriteConversion(this GenerationContext ctx, Ident dest, String op, OberonType from, Ident src, OberonType to)
        {
            return ctx.WriteAssignLeft(dest).Write("{0} ", op).Anchor().WriteType(from).Write(" ").Anchor().Write("{0} ", src).Anchor().Write("to ").WriteType(to).NewLine();
        }

        static GenerationContext WriteOperation(this GenerationContext ctx, Ident dest, String op, OberonType type, params Ident[] args)
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

        static GenerationContext WriteConversion(this GenerationContext ctx, OberonType from, OberonType to, Ident ident)
        {
            if (from is IntegerType && to is IntegerType) {
                IntegerType fi = (IntegerType) from, ti = (IntegerType) to;
                if (fi.Range == ti.Range) {
                    return ctx;
                } else if (fi.Range < ti.Range) {
                    return ctx.WriteConversion(ident, "zext", from, ident, to);
                } else {
                    return ctx.WriteConversion(ident, "trunc", from, ident, to);
                }
            } else {
                throw new InvalidOperationException("No conversion between " + from.ToString() + " to " + to.ToString() + "defined");
            }
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

        static GenerationContext WriteFactor(this GenerationContext ctx, NFactor node, Ident dest)
        {
            if (node.Inner is NExpr) {
                return ctx.WriteExpr((NExpr) node.Inner, dest);
            } else {
                return ctx.WriteAssignLeft(dest).WriteNode(node.Inner).NewLine();
            }
        }

        static GenerationContext WriteTerm(this GenerationContext ctx, NTerm node, Ident dest)
        {
            if (node.Operator == TermOperator.None) {
                return ctx.WriteFactor(node.Factor, dest);
            } else {
                throw new NotImplementedException();
            }
        }

        static GenerationContext WriteSimpleExpr(this GenerationContext ctx, NSimpleExpr node, Ident dest)
        {
            if (node.Operator == SimpleExprOperator.None) {
                return ctx.WriteTerm(node.Term, dest);
            } else {
                using (TempIdent left = TempIdent.Create(), right = TempIdent.Create()) {
                    ctx.WriteSimpleExpr(node.Prev, left);
                    ctx.WriteTerm(node.Term, right);

                    var type = node.GetFinalType(_scope);

                    ctx.WriteConversion(node.Prev.GetFinalType(_scope), type, left);
                    ctx.WriteConversion(node.Term.GetFinalType(_scope), type, right);

                    switch (node.Operator) {
                        case SimpleExprOperator.Add:
                            return ctx.WriteOperation(dest, "add", type, left, right);
                        case SimpleExprOperator.Subtract:
                            return ctx.WriteOperation(dest, "sub", type, left, right);
                        default:
                            throw new NotImplementedException();
                    }
                }
            }
        }

        static GenerationContext WriteExpr(this GenerationContext ctx, NExpr node, Ident dest)
        {
            if (node.Operator == ExprOperator.None) {
                return ctx.WriteSimpleExpr(node.SimpleExpr, dest);
            } else {
                throw new NotImplementedException();
            }
        }

        static Ident GetDesignation(this GenerationContext ctx, NDesignator node)
        {
            if (node.IsRoot) {
                return new QualIdent((NQualIdent) node.Element);
            } else {
                throw new NotImplementedException();
            }
        }

        static GenerationContext WriteNode(this GenerationContext ctx, NAssignment node)
        {
            return ctx.WriteExpr(node.Expression, ctx.GetDesignation(node.Assignee));
        }
    }
}
