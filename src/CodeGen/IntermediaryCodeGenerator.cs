using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DUO2C.Semantics;

namespace DUO2C.CodeGen
{
    public static class IntermediaryCodeGenerator
    {
        public static String Generate(ModuleType module)
        {
            var ctx = new GenerationContext();
            ctx.WriteModule(module);
            return ctx.GeneratedCode;
        }

        static GenerationContext WriteModule(this GenerationContext ctx, ModuleType module)
        {
            ctx.Write("; Generated {0}", DateTime.Now.ToString()).NewLine();
            ctx.Write("; GlobalUID {0}", Guid.NewGuid().ToString()).NewLine();
            ctx.Write(";").NewLine();
            ctx.Write("; LLVM IR file for module \"{0}\"", module.Identifier).NewLine();
            ctx.Write(";").NewLine();
            ctx.Write("; WARNING: This file is automatically").NewLine();
            ctx.Write("; generated and should not be edited").NewLine().NewLine();

            ctx.Write("; Begin type aliases").NewLine().Enter();
            ctx.WriteTypeDecl("CHAR", IntegerType.ShortInt);
            ctx.WriteTypeDecl("SET", IntegerType.LongInt);

            foreach (var kv in module.Scope.GetTypes()) {
                ctx.WriteTypeDecl(kv.Key, kv.Value.Type);
            }
            ctx.Leave().NewLine().Write("; End type aliases").NewLine();

            return ctx.Write("; Module end").NewLine();
        }

        static GenerationContext WriteTypeDecl(this GenerationContext ctx, String identifier, OberonType type)
        {
            ctx.NewLine();
            ctx.Write("; {0} = ", identifier).Write(type.ToString());
            ctx.NewLine();
            ctx.Write("%t_{0} = type ", identifier).WriteType(type);
            return ctx.NewLine();
        }
        
        static GenerationContext WriteType(this GenerationContext ctx, OberonType type)
        {
            if (type is RecordType) {
                var rec = (RecordType) type;
                ctx.Write("{");
                if (rec.SuperRecordName != null) {
                    ctx.Write("%t_{0}", rec.SuperRecordName);
                    if (rec.Fields.Count() > 0) ctx.Write(", ");
                }
                foreach (var fl in rec.Fields) {
                    if (fl != rec.Fields.First()) ctx.Write(", ");
                    ctx.WriteType(fl.Type);
                }
                return ctx.Write("}");
            } else if (type is PointerType) {
                var ptr = (PointerType) type;
                return ctx.WriteType(ptr.ResolvedType).Write("*");
            } else if (type is ArrayType) {
                var at = (ArrayType) type;
                return ctx.Write("{").WriteType(IntegerType.Integer).Write(", ").WriteType(at.ElementType).Write("}");
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
                return ctx.Write("%t_SET");
            } else if (type is BooleanType) {
                return ctx.Write("i1");
            } else if (type is CharType) {
                return ctx.Write("%t_CHAR");
            } else if (type is UnresolvedType) {
                var ut = (UnresolvedType) type;
                return ctx.Write("%t_{0}", ut.Identifier);
            } else {
                throw new NotImplementedException("No rule to generate LLVMIR for type " + type.GetType().FullName);
            }
        }
    }
}
