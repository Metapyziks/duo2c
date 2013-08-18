﻿using System;
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

            var types = module.Scope.GetTypes(false).Where(x => x.Value.Visibility != AccessModifier.Private);
            if (types.Any(x => true)) {
                ctx = ctx.Write("TYPE").Ln().Enter();
                foreach (var kv in types) {
                    kv.Value.Type.Resolve(module.Scope);
                    ctx.WriteTypeDecl(module, kv.Key, kv.Value.Type, kv.Value.Visibility);
                }
                ctx = ctx.Leave().Ln().Ln();
            }

            var vars = module.Scope.GetSymbols(false).Where(x => x.Value.Visibility != AccessModifier.Private && !x.Value.Type.IsProcedure && !x.Value.IsConstant);
            if (vars.Any(x => true)) {
                ctx = ctx.Write("VAR").Ln().Enter();
                foreach (var kv in vars) {
                    ctx.WriteVarDecl(module, kv.Key, kv.Value.Type, kv.Value.Visibility);
                }
                ctx = ctx.Leave().Ln().Ln();
            }

            var consts = module.Module.Declarations.Constants.Where(x => x.IdentDef.Visibility != AccessModifier.Private);
            if (consts.Any(x => true)) {
                ctx = ctx.Write("CONST").Ln().Enter();
                foreach (var cval in consts) {
                    ctx.WriteConstDecl(module, cval.Identifier, cval.ConstExpr.String, AccessModifier.Public);
                }
                ctx = ctx.Leave().Ln().Ln();
            }

            var procs = module.Scope.GetSymbols(false).Where(x => x.Value.Visibility != AccessModifier.Private && x.Value.Type.IsProcedure);
            foreach (var kv in procs) {
                ctx.WriteProcDecl(module, kv.Key, kv.Value.Type.As<ProcedureType>(), kv.Value.Visibility);
            }

            var records = types.Where(x => x.Value.Type.IsRecord);
            foreach (var rec in records) {
                var type = rec.Value.Type.As<RecordType>();
                var ptr = new UnresolvedType(types.First(x => {
                    if (!x.Value.Type.IsPointer) return false;
                    var resType = x.Value.Type.As<PointerType>().ResolvedType;
                    if (!(resType is UnresolvedType)) return false;
                    var unresType = (UnresolvedType) resType;
                    if (unresType.Module != null && unresType.Module != module.Identifier) return false;
                    return unresType.Identifier == rec.Key;
                }).Key, module.Identifier);
                foreach (var proc in type.Procedures.Where(x => x.Value.Visibility != AccessModifier.Private)) {
                    ctx.WriteProcedure(module, proc.Key, proc.Value.Type.As<ProcedureType>(), proc.Value.Visibility, ptr);
                }
            }

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

        static GenerationContext WriteConstDecl(this GenerationContext ctx, ModuleType module, String identifier, String value, AccessModifier visibility)
        {
            return ctx.Write(identifier).WriteAccessModifier(visibility).Anchor().Write(" = ").Write(value).Write(";").Ln();
        }

        static GenerationContext WriteProcDecl(this GenerationContext ctx, ModuleType module, String identifier, ProcedureType type, AccessModifier visibility)
        {
            ctx.Write("PROCEDURE ");

            if (!(type is ExternalProcedureType)) ctx.Write("^ ");

            ctx.Write(identifier).WriteAccessModifier(visibility);
            if (type.Params.Length > 0) {
                ctx.Write(" (");
                for(int i = 0; i < type.Params.Length; ++i) {
                    var param = type.Params[i];
                    if (param.ByReference) ctx.Write("VAR ");
                    ctx.Write(param.Identifier);
                    if (i < type.Params.Length - 1 && type.Params[i + 1].Type == param.Type) {
                        ctx.Write(", ");
                    } else {
                        ctx.Write(" : ").WriteType(module, param.Type);
                        if (i < type.Params.Length - 1) ctx.Write("; ");
                    }
                }
                ctx.Write(")");
            }
            if (type.ReturnType != null) {
                ctx.Write(" : ").WriteType(module, type.ReturnType);
            }
            ctx.Write(";").Ln();

            if (type is ExternalProcedureType) {
                var externType = (ExternalProcedureType) type;
                ctx.Write("EXTERNAL ");
                if (externType.IsImported) ctx.Write("IMPORT ");
                ctx.Write("\"{0}\";", externType.ExternalSymbol).Ln();
            }

            return ctx.Ln();
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

                foreach (var kv in rec.Fields) {
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
            } else if (type is PointerType) {
                var ptr = (PointerType) type;
                ctx.Write("POINTER TO ");
                ctx.WriteType(module, ptr.ResolvedType);
                return ctx;
            } else {
                return ctx.Write(type.ToString());
            }
        }

        static GenerationContext WriteProcedure(this GenerationContext ctx, ModuleType module, String ident,
            ProcedureType proc, AccessModifier visibility, UnresolvedType receiver = null)
        {
            ctx.Write("PROCEDURE ^ ");
            if (receiver != null) { 
                ctx.Write("(this : {0}) ", receiver.Identifier);
            }
            ctx.Write(ident).WriteAccessModifier(visibility);

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

            ctx.Write(";").Ln();

            return ctx;
        }
    }
}
