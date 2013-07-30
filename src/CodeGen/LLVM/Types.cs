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
                    return paras.Last().ParameterType.Extends(typeof(OberonType));
                }).ToDictionary(x => x.GetParameters().Last().ParameterType);
            }

            if (_typeMethods.ContainsKey(type.GetType())) {
                return (GenerationContext) _typeMethods[type.GetType()].Invoke(null, new Object[] { ctx, type });
            } else {
                throw new NotImplementedException(String.Format("No rule to generate IR for type '{0}' found", type));
            }
        }

        static GenerationContext Structure(this GenerationContext ctx, params OberonType[] types)
        {
            ctx.Write("{");
            foreach (var type in types) {
                if (type != types.First()) ctx.Write(", ");
                ctx.Type(type);
            }
            return ctx.Write("}");
        }

        static GenerationContext Type(this GenerationContext ctx, RecordType type)
        {
            var types = new List<OberonType>();

            if (type.SuperRecordName != null) types.Add(new UnresolvedType(type.SuperRecordName));

            types.AddRange(type.Fields.Where(x => !(x.Type is ProcedureType)).Select(x => x.Type));

            return ctx.Structure(types.ToArray());
        }

        static GenerationContext Type(this GenerationContext ctx, PointerType type)
        {
            return ctx.Type(type.ResolvedType).Write("*");
        }

        static GenerationContext Type(this GenerationContext ctx, ArrayType type)
        {
            return ctx.Structure(IntegerType.Integer, new PointerType(type.ElementType));
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
            return ctx.Type(new UnresolvedType("SET"));
        }

        static GenerationContext Type(this GenerationContext ctx, CharType type)
        {
            return ctx.Type(new UnresolvedType("CHAR"));
        }

        static GenerationContext Type(this GenerationContext ctx, BooleanType type)
        {
            return ctx.Write("i1");
        }

        static GenerationContext Type(this GenerationContext ctx, UnresolvedType type)
        {
            var module = type.Module ?? _module.Identifier;
            return ctx.Write("%type.{0}.{1}", module, type.Identifier);
        }
    }
}
