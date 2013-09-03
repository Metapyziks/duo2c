﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using DUO2C.Semantics;

namespace DUO2C.CodeGen.LLVM
{
    public static partial class IntermediaryCodeGenerator
    {
        class ConstArrayType : OberonType
        {
            public OberonType ElementType { get; private set; }
            public int Length { get; private set; }

            public ConstArrayType(OberonType elementType, int length)
            {
                ElementType = elementType;
                Length = length;
            }

            public override bool CanCompare(OberonType other)
            {
                return false;
            }

            public override bool CanTestEquality(OberonType other)
            {
                return false;
            }
        }

        class OpaqueType : OberonType
        {
            public OpaqueType() { }

            public override bool CanCompare(OberonType other)
            {
                return false;
            }

            public override bool CanTestEquality(OberonType other)
            {
                return false;
            }
        }

        static Dictionary<Type, MethodInfo> _typeMethods;

        static GenerationContext Type(this GenerationContext ctx, OberonType type)
        {
            if (_typeMethods == null) {
                var flags = BindingFlags.Static | BindingFlags.NonPublic;
                _typeMethods = typeof(IntermediaryCodeGenerator).GetMethods(flags).Where(x => {
                    if (x.Name != "Type" || x.ReturnType != typeof(GenerationContext)) return false;
                    var paras = x.GetParameters();
                    if (paras.Count() != 2) return false;
                    if (paras.First().ParameterType != typeof(GenerationContext)) return false;
                    if (paras.Last().ParameterType == typeof(OberonType)) return false;
                    return typeof(OberonType).IsAssignableFrom(paras.Last().ParameterType);
                }).ToDictionary(x => x.GetParameters().Last().ParameterType);
            }

            if (_typeMethods.ContainsKey(type.GetType())) {
                return (GenerationContext) _typeMethods[type.GetType()].Invoke(null, new Object[] { ctx, type.Resolve(_scope) });
            } else {
                throw new NotImplementedException(String.Format("No rule to generate IR for type '{0}' found", type));
            }
        }

        static GenerationContext Structure(this GenerationContext ctx, params OberonType[] types)
        {
            ctx.Write("{");
            bool first = true;
            foreach (var type in types) {
                if (!first) {
                    ctx.Write(", ");
                } else {
                    first = false;
                }
                ctx.Type(type);
            }
            return ctx.Write("}");
        }

        static GenerationContext Type(this GenerationContext ctx, ProcedureType type)
        {
            var returnType = type.ReturnType ?? VoidType.Default;

            ctx.Type(returnType).Write(" (").EndArguments();
            foreach (var p in type.ParamsWithReceiver) {
                ctx.Argument(p.ByReference ? new PointerType(p.Type) : p.Type, false);
            }
            return ctx.EndArguments().Write(")");
        }

        static OberonType Marshalled(this OberonType type, Scope scope)
        {
            if (type.IsArray) {
                return new PointerType(type.As<ArrayType>().ElementType.Marshalled(scope)).Resolve(scope);
            }

            if (type.IsPointer) {
                return new PointerType(type.As<PointerType>().ResolvedType.Marshalled(scope)).Resolve(scope);
            }

            return type;
        }
        
        static GenerationContext Type(this GenerationContext ctx, ExternalProcedureType type)
        {
            var returnType = type.ReturnType ?? VoidType.Default;

            ctx.Type(returnType).Write(" (").EndArguments();
            foreach (var p in type.ParamsWithReceiver) {
                var ptype = p.Type.Marshalled(_scope);
                ctx.Argument(p.ByReference ? new PointerType(ptype) : ptype, false);
            }
            return ctx.EndArguments().Write(")");
        }

        static GenerationContext Type(this GenerationContext ctx, RecordType type)
        {
            return ctx.Structure(new OberonType[] { new PointerType(IntegerType.Byte) }
                .Concat(type.FieldDecls.Select(x => x.Type)).ToArray());
        }

        static GenerationContext Type(this GenerationContext ctx, PointerType type)
        {
            return ctx.Type(type.ResolvedType).Write("*");
        }

        static GenerationContext Type(this GenerationContext ctx, ArrayType type)
        {
            if (type.IsOpen) {
                return ctx.Structure(IntegerType.Integer, new PointerType(type.ElementType));
            } else {
                return ctx.Type(new ConstArrayType(type.ElementType, type.Length));
            }
        }

        static GenerationContext Type(this GenerationContext ctx, VectorType type)
        {
            return ctx.Write("<{0} \tx \t", type.Length).Type(type.ElementType).Write("\t>");
        }

        static GenerationContext Type(this GenerationContext ctx, IntegerType type)
        {
            return ctx.Write("i{0}", (int) type.Range * 8);
        }

        static GenerationContext Type(this GenerationContext ctx, RealType type)
        {
            return ctx.Write(type.Range == RealRange.Real ? "float" : "double");
        }

        static GenerationContext Type(this GenerationContext ctx, SetType type)
        {
            return ctx.Write(new TypeIdent(SetType.Default.ToString()));
        }

        static GenerationContext Type(this GenerationContext ctx, CharType type)
        {
            return ctx.Write(new TypeIdent(CharType.Default.ToString()));
        }

        static GenerationContext Type(this GenerationContext ctx, BooleanType type)
        {
            return ctx.Write("i1");
        }

        static GenerationContext Type(this GenerationContext ctx, UnresolvedType type)
        {
            var module = type.Module ?? _module.Identifier;
            return ctx.Write(new QualIdent(type.Identifier, module));
        }

        static GenerationContext Type(this GenerationContext ctx, VoidType type)
        {
            return ctx.Write("void");
        }

        static GenerationContext Type(this GenerationContext ctx, VarArgsType type)
        {
            return ctx.Write("...");
        }

        static GenerationContext Type(this GenerationContext ctx, ConstArrayType type)
        {
            return ctx.Write("[{0} \tx \t", type.Length).Type(type.ElementType).Write("\t]");
        }

        static GenerationContext Type(this GenerationContext ctx, OpaqueType type)
        {
            return ctx.Write("opaque");
        }
    }
}
