﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using DUO2C.Nodes;
using DUO2C.Nodes.Oberon2;
using DUO2C.Semantics;

namespace DUO2C.CodeGen.LLVM
{
    public static partial class IntermediaryCodeGenerator
    {
        static NModule _module;
        static Scope _scope;
        static Dictionary<String, GlobalStringIdent> _stringConsts;

        public static String Generate(NModule module, Guid uniqueID)
        {
            var ctx = new GenerationContext();
            ctx.WriteModule(module, uniqueID);
            return ctx.ToString();
        }

        static GlobalStringIdent GetStringIdent(String str)
        {
            if (_stringConsts.ContainsKey(str)) {
                return _stringConsts[str];
            } else {
                var ident = new GlobalStringIdent();
                _stringConsts.Add(str, ident);
                return ident;
            }
        }

        static GenerationContext WriteModule(this GenerationContext ctx, NModule module, Guid uniqueID)
        {
            _module = module;
            _scope = module.Type.Scope;
            _stringConsts = new Dictionary<string, GlobalStringIdent>();

            GlobalStringIdent.Reset();
            TempIdent.Reset();

            ctx.Header(uniqueID);

            ctx.DataLayoutStart(false)
                .PointerAlign(0, 32)
                .IntegerAlign(1, 8, 8)
                .IntegerAlign(8)
                .IntegerAlign(16)
                .IntegerAlign(32)
                .IntegerAlign(64)
                .FloatAlign(32)
                .FloatAlign(64)
                .AgregateAlign(64)
                .NativeAlign(8, 16, 32)
                .StackAlign(32)
                .DataLayoutEnd().NewLine();

            var printIntStr = new GlobalStringIdent();
            var printFloatStr = new GlobalStringIdent();
            var printLineStr = new GlobalStringIdent();
            var printTrueStr = new GlobalStringIdent();
            var printFalseStr = new GlobalStringIdent();

            ctx.Lazy(x => {
                foreach (var kv in _stringConsts.OrderBy(y => y.Value.ID)) {
                    x.StringConstant(kv.Key, kv.Value);
                }
            });

            ctx.Write("declare i32 @printf(i8*, ...) nounwind").NewLine().NewLine();

            ctx.Lazy(x => {
                x.WriteTypeDecl("CHAR", IntegerType.Byte);
                x.WriteTypeDecl("SET", IntegerType.LongInt);

                foreach (var kv in _scope.GetTypes().Where(y => !(y.Value.Type is ProcedureType))) {
                    x.WriteTypeDecl(kv.Key, kv.Value.Type);
                }
            });


            foreach (var v in _scope.GetSymbols()) {
                if (!v.Value.Type.IsProcedure) {
                    ctx.WriteGlobalDecl(new QualIdent(v.Key), v.Value.Type);
                }
            }

            if (_scope.GetSymbols().Count() > 0) {
                ctx.NewLine();
            }

            ctx = ctx.Write("define i32 @").Write("main() {").Enter().NewLine().NewLine();
            if (module.Body != null) {
                ctx.WriteStatements(module.Body.Statements.Select(x => x.Inner));
            }
            ctx = ctx.Write("ret i32 0").NewLine().Leave().Write("}").NewLine();

            return ctx;
        }

        static GenerationContext Write(this GenerationContext ctx, Value val)
        {
            return ctx.Write(val.ToString());
        }

        static GenerationContext WriteGlobalDecl(this GenerationContext ctx, QualIdent ident, OberonType type)
        {
            bool isPublic = _scope.GetSymbolDecl(ident.Identifier, ident.Module).Visibility != AccessModifier.Private;
            return ctx.Global(ident, type, isPublic);
        }

        static GenerationContext WriteTypeDecl(this GenerationContext ctx, String identifier, OberonType type)
        {
            ctx.Type(new UnresolvedType(identifier)).Write(" ").Anchor().Write("= type ").Type(type);
            return ctx.NewLine();
        }

        static GenerationContext WriteStatements(this GenerationContext ctx, IEnumerable<Statement> statements)
        {
            foreach (var stmnt in statements) {
#if DEBUG
                ctx.Write("; {0}", stmnt.String);
#endif
                ctx.Enter(0).NewLine().Node(stmnt).NewLine().Leave();
            }
            return ctx;
        }

        static GenerationContext WriteOperation(this GenerationContext ctx, String op, params Object[] args)
        {
            ctx.Write("{0} ", op).Anchor();
            foreach (var arg in args) {
                if (arg is OberonType) {
                    ctx.Type((OberonType) arg).Write(" ").Anchor();
                } else {
                    ctx.Write(arg.ToString());
                    if (arg != args.Last()) {
                        ctx.Write(", ").Anchor();
                    }
                }
            }
            return ctx.NewLine();
        }

        static Dictionary<Type, MethodInfo> _nodeMethods;

        static GenerationContext Node(this GenerationContext ctx, SubstituteNode node)
        {
            if (_nodeMethods == null) {
                var flags = BindingFlags.Static | BindingFlags.NonPublic;
                _nodeMethods = typeof(IntermediaryCodeGenerator).GetMethods(flags).Where(x => {
                    if (x.Name != "Node" || x.ReturnType != typeof(GenerationContext)) return false;
                    var paras = x.GetParameters();
                    if (paras.Count() != 2) return false;
                    if (paras.First().ParameterType != typeof(GenerationContext)) return false;
                    if (paras.Last().ParameterType == typeof(SubstituteNode)) return false;
                    return paras.Last().ParameterType.Extends(typeof(SubstituteNode));
                }).ToDictionary(x => x.GetParameters().Last().ParameterType);
            }

            if (_nodeMethods.ContainsKey(node.GetType())) {
                return (GenerationContext) _nodeMethods[node.GetType()].Invoke(null, new object[] { ctx, node });
            } else {
                throw new NotImplementedException(String.Format("No method of generating IR for nodes of type '{0}' found", node.GetType().Name));
            }
        }
    }
}
