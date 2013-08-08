using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DUO2C.Nodes;
using DUO2C.Nodes.Oberon2;
using DUO2C.Semantics;

namespace DUO2C.CodeGen
{
    public static class SymbolCodeGenerator
    {
        public static String Generate(ModuleType module, Guid uniqueID)
        {
            var ctx = new GenerationContext();
            ctx.WriteModule(module, uniqueID);
            return ctx.ToString();
        }

        static GenerationContext WriteModule(this GenerationContext ctx, ModuleType module, Guid uniqueID)
        {
            ctx.Write("(**").Ln();
            ctx.Write(" *  Generated {0}", DateTime.Now.ToString()).Ln();
            ctx.Write(" *  GlobalUID {0}", uniqueID.ToString()).Ln();
            ctx.Write(" *").Ln();
            ctx.Write(" *  Exported symbol file for module \"{0}\"", module.Identifier).Ln();
            ctx.Write(" *").Ln();
            ctx.Write(" *  WARNING: This file is automatically").Ln();
            ctx.Write(" *  generated and should not be edited").Ln();
            ctx.Write("**)").Ln().Ln();
            ctx = ctx.Write("MODULE {0};", module.Identifier).Ln().Enter();

            ctx = ctx.Write("TYPE").Ln().Enter();
            foreach (var kv in module.Scope.GetTypes().Where(x => x.Value.Visibility != AccessModifier.Private)) {
                kv.Value.Type.Resolve(module.Scope);
                ctx.WriteTypeDecl(module, kv.Key, kv.Value.Type, kv.Value.Visibility);
            }
            ctx = ctx.Leave();

            ctx = ctx.Write("VAR").Ln().Enter();
            foreach (var kv in module.Scope.GetSymbols().Where(x => x.Value.Visibility != AccessModifier.Private)) {
                ctx.WriteVarDecl(module, kv.Key, kv.Value.Type, kv.Value.Visibility);
            }
            ctx = ctx.Leave();

            return ctx.Leave().Write("END {0}.", module.Identifier).Ln();
        }

        static GenerationContext WriteAccessModifier(this GenerationContext ctx, AccessModifier visibility)
        {
            return ctx.Write(" ").Anchor().Write(visibility == AccessModifier.Public ? "*" : visibility == AccessModifier.ReadOnly ? "-" : "");
        }

        static GenerationContext WriteTypeDecl(this GenerationContext ctx, ModuleType module, String identifier, OberonType type, AccessModifier visibility)
        {
            return ctx.Write(identifier).WriteAccessModifier(visibility).Anchor().Write(" = ").WriteType(module, type).Write(";").Ln();
        }

        static GenerationContext WriteVarDecl(this GenerationContext ctx, ModuleType module, String identifier, OberonType type, AccessModifier visibility)
        {
            return ctx.Write(identifier).WriteAccessModifier(visibility).Anchor().Write(" : ").WriteType(module, type).Write(";").Ln();
        }

        static GenerationContext WriteType(this GenerationContext ctx, ModuleType module, OberonType type)
        {
            if (type is UnresolvedType) {
                var ut = (UnresolvedType) type;
                return ctx.Write("{0}.{1}", ut.Module ?? module.Identifier, ut.Identifier);
            } else if (type is RecordType) {
                var rec = (RecordType) type;
                ctx.Write("RECORD");
                if (rec.SuperRecordName != null) {
                    ctx.Write(" ({0})", rec.SuperRecordName);
                }
                ctx = ctx.Ln().Enter();

                foreach (var kv in rec.Fields.Where(x => x.Value.Visibility != AccessModifier.Private)) {
                    ctx.WriteVarDecl(module, kv.Key, kv.Value.Type, kv.Value.Visibility);
                }

                return ctx.Leave().Write("END");
            } else if (type is ProcedureType) {
                var proc = (ProcedureType) type;
                ctx.Write("PROCEDURE");

                if (proc.ReturnType != null || proc.Params.Length > 0) {
                    ctx.Write(" (");
                    foreach (var para in proc.Params) {
                        ctx.Write("{0} : ", para.Identifier).WriteType(module, para.Type);
                        if (para != proc.Params.Last()) {
                            ctx.Write("; ");
                        }
                    }
                    ctx.Write(")");

                    if (proc.ReturnType != null) {
                        ctx.Write(" : ").WriteType(module, proc.ReturnType);
                    }
                }
                return ctx;
            } else {
                return ctx.Write(type.ToString());
            }
        }
    }
}
