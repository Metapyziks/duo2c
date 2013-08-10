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
                
                ctx.Keyword("declare");

                if (dest is GlobalIdent && ((GlobalIdent) dest).OptionTags.HasFlag(GlobalIdent.Options.NoAlias)) {
                    ctx.Keyword("noalias");
                }

                if (proc.ReturnType != null) {
                    ctx.Type(proc.ReturnType);
                } else {
                    ctx.Type(VoidType.Default);
                }

                ctx.Write(" \t{0}\t(", dest);
                foreach (var t in proc.Params.Select(x => x.Type)) {
                    ctx.Argument(t);
                }
                ctx.Write(") \t");
                
                if (!(dest is GlobalIdent) || ((GlobalIdent) dest).OptionTags.HasFlag(GlobalIdent.Options.NoUnwind)) {
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
            return ctx.Constant(ident, new ConstArrayType(IntegerType.Byte, bytes.Length), new Literal(bytes));
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
                var ident = new RecordTableIdent();
                _recordTableIdents.Add(type, ident);
                return ident;
            }
        }

        static ConstArrayType GetRecordTableType(RecordType type)
        {
            return new ConstArrayType(new PointerType(IntegerType.Byte), 2 + type.Procedures.Count());
        }

        static GenerationContext RecordTable(this GenerationContext ctx, String ident, RecordType type)
        {
            ctx.Assign(GetRecordTableIdent(type)).Keyword("global").Argument(GetRecordTableType(type), new RecordTableConst(ident, type));
            return ctx.EndOperation();
        }
    }
}
