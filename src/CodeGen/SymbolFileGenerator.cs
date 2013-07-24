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
    public static class SymbolFileGenerator
    {
        public static String Generate(ModuleType module)
        {
            var ctx = new GenerationContext();
            ctx.WriteModule(module);
            return ctx.GeneratedCode;
        }

        static GenerationContext WriteModule(this GenerationContext ctx, ModuleType module)
        {
            ctx.Write("(**").NewLine();
            ctx.Write(" *  Generated {0}", DateTime.Now.ToString()).NewLine();
            ctx.Write(" *  GlobalUID {0}", Guid.NewGuid().ToString()).NewLine();
            ctx.Write(" *").NewLine();
            ctx.Write(" *  Exported symbol file for module \"{0}\"", module.Identifier).NewLine();
            ctx.Write(" *").NewLine();
            ctx.Write(" *  WARNING: This file is automatically").NewLine();
            ctx.Write(" *  generated and should not be edited").NewLine();
            ctx.Write("**)").NewLine().NewLine();
            ctx.Write("MODULE {0};", module.Identifier).NewLine().Enter();

            ctx.Write("TYPE").NewLine().Enter();
            foreach (var kv in module.Scope.GetTypes()) {
                ctx.WriteTypeDecl(kv.Key, kv.Value.Type, kv.Value.Visibility);
            }
            ctx.Leave();

            ctx.Write("VAR").NewLine().Enter();
            foreach (var kv in module.Scope.GetSymbols().Where(x => x.Value.Visibility != AccessModifier.Private)) {
                ctx.WriteVarDecl(kv.Key, kv.Value.Type, kv.Value.Visibility);
            }
            ctx.Leave();

            return ctx.Leave().Write("END {0}.", module.Identifier).NewLine();
        }

        static GenerationContext WriteAccessModifier(this GenerationContext ctx, AccessModifier visibility)
        {
            return ctx.Write(visibility == AccessModifier.Public ? "*" : visibility == AccessModifier.ReadOnly ? "-" : "");
        }

        static GenerationContext WriteTypeDecl(this GenerationContext ctx, String identifier, OberonType type, AccessModifier visibility)
        {
            return ctx.Write(identifier).WriteAccessModifier(visibility).Write(" = ").WriteType(type).Write(";").NewLine();
        }

        static GenerationContext WriteVarDecl(this GenerationContext ctx, String identifier, OberonType type, AccessModifier visibility)
        {
            return ctx.Write(identifier).WriteAccessModifier(visibility).Write(" : ").WriteType(type).Write(";").NewLine();
        }

        static GenerationContext WriteType(this GenerationContext ctx, OberonType type)
        {
            if (type is RecordType) {
                var rec = (RecordType) type;
                ctx.Write("RECORD");
                if (rec.SuperRecordName != null) {
                    ctx.Write(" ({0})", rec.SuperRecordName);
                }
                ctx.NewLine().Enter();

                foreach (var field in rec.Fields.Where(x => x.Visibility != AccessModifier.Private)) {
                    ctx.WriteVarDecl(field.Identifier, field.Type, field.Visibility);
                }

                return ctx.Leave().Write("END");
            } else {
                return ctx.Write(type.ToString());
            }
        }
    }
}
