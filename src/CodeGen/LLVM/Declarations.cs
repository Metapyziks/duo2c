using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DUO2C.Semantics;

namespace DUO2C.CodeGen.LLVM
{
    public static partial class IntermediaryCodeGenerator
    {
        static GenerationContext Global(this GenerationContext ctx, Value dest, OberonType type)
        {
            bool isPublic = false;
            if (dest is QualIdent) {
                var ident = (QualIdent) dest;
                isPublic = _scope.GetSymbolDecl(ident.Identifier, ident.Module).Visibility != AccessModifier.Private;
            } else if (dest is GlobalIdent) {
                var ident = (GlobalIdent) dest;
                isPublic = ident.IsPublic;
            }

            if (type.IsProcedure) {
                var proc = type.As<ProcedureType>();
                var isExternal = proc is ExternalProcedureType;
                
                ctx.Keyword("declare");

                if (isExternal) {
                    var externType = (ExternalProcedureType) proc;
                    if (externType.IsImported) {
                        ctx.Keyword("dllimport");
                    }
                    ctx.Keyword("x86_stdcallcc");
                }
                
                if (dest is GlobalIdent && ((GlobalIdent) dest).OptionTags.HasFlag(GlobalIdent.Options.NoAlias)) {
                    ctx.Keyword("noalias");
                }

                if (proc.ReturnType != null) {
                    ctx.Type(proc.ReturnType);
                } else {
                    ctx.Type(VoidType.Default);
                }

                ctx.Write(" \t{0}\t(", isExternal ? "@" + ((ExternalProcedureType) proc).ExternalSymbol : dest.ToString());
                foreach (var p in proc.ParamsWithReceiver) {
                    var ptype = isExternal ? p.Type.Marshalled(_scope) : p.Type;
                    if (p.ByReference) {
                        ctx.Argument(new PointerType(ptype));
                    } else {
                        ctx.Argument(ptype);
                    }
                }
                ctx.Write(") \t");
                
                if (!isExternal && (!(dest is GlobalIdent) || ((GlobalIdent) dest).OptionTags.HasFlag(GlobalIdent.Options.NoUnwind))) {
                    ctx.Keyword("nounwind");
                }
                
                return ctx.EndOperation();
            } else {
                ctx.Assign(dest);
                if (!isPublic) ctx.Keyword("private"); else ctx.Write("\t");
                return ctx.Keyword("global").Argument(type, Literal.GetDefault(type)).EndOperation();
            }
        }

        static GenerationContext Local(this GenerationContext ctx, Value dest, OberonType type)
        {
            return ctx.Assign(dest).Keyword("alloca").Argument(type).EndOperation();
        }

        static GenerationContext TypeDecl(this GenerationContext ctx, TypeIdent ident, OberonType type)
        {
            return ctx.Assign(ident).Keyword("type").Argument(type).EndOperation();
        }

        static GenerationContext StringConstant(this GenerationContext ctx, GlobalStringIdent ident, String str)
        {
            var bytes = UTF8Encoding.UTF8.GetBytes(str + "\0");
            return ctx.Constant(ident, new StaticArrayType(IntegerType.Byte, bytes.Length), new Literal(bytes));
        }

        static GenerationContext Constant(this GenerationContext ctx, GlobalStringIdent ident, OberonType type, Value value)
        {
            return ctx.Assign(ident).Keyword("private", "constant").Argument(type, value).EndOperation();
        }

        static Dictionary<RecordType, RecordTableIdent> _recordTableIdents = new Dictionary<RecordType,RecordTableIdent>();
        static RecordTableIdent GetRecordTableIdent(RecordType type)
        {
            if (_recordTableIdents.ContainsKey(type)) {
                return _recordTableIdents[type];
            } else {
                var ident = new RecordTableIdent(type);
                _recordTableIdents.Add(type, ident);
                return ident;
            }
        }

        static StaticArrayType GetRecordTableType(RecordType type)
        {
            return new StaticArrayType(new PointerType(IntegerType.Byte), 2 + type.Procedures.Count());
        }

        static GenerationContext RecordTable(this GenerationContext ctx, String ident, RecordType type, bool define)
        {
            ctx.Assign(GetRecordTableIdent(type));
            if (define) {
                ctx.Keyword("global").Argument(GetRecordTableType(type), new RecordTableConst(ident, type));
            } else {
                ctx.Keyword("linkonce", "global").Argument(new StaticArrayType(PointerType.Byte, 0)).Keyword("[]");
            }
            return ctx.EndOperation();
        }
    }
}
