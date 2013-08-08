using System;
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
        class VoidType : OberonType
        {
            public static readonly VoidType Default = new VoidType();

            public override bool CanCompare(OberonType other)
            {
                throw new NotImplementedException();
            }

            public override bool CanTestEquality(OberonType other)
            {
                throw new NotImplementedException();
            }
        }

        class VarArgsType : OberonType
        {
            public static readonly VarArgsType Default = new VarArgsType();

            public override bool CanCompare(OberonType other)
            {
                throw new NotImplementedException();
            }

            public override bool CanTestEquality(OberonType other)
            {
                throw new NotImplementedException();
            }
        }

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
                throw new NotImplementedException();
            }

            public override bool CanTestEquality(OberonType other)
            {
                throw new NotImplementedException();
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

            ctx.Type(returnType).Write(" (");
            foreach (var p in type.Params) {
                ctx.Argument(p.ByReference ? new PointerType(p.Type) : p.Type, false);
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
    }
}
