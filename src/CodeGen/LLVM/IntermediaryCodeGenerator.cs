using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Runtime.Serialization;
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

        static GlobalIdent _printfProc;
        static ProcedureType _printfProcType;

        static GlobalIdent _gcMallocProc;
        static ProcedureType _gcMallocProcType;

        public static String Generate(NModule module, Guid uniqueID, bool entryModule)
        {
            _stringConsts = new Dictionary<string, GlobalStringIdent>();

            _printfProc = new GlobalIdent("printf", false, GlobalIdent.Options.NoUnwind);
            _printfProcType = new ProcedureType(IntegerType.Integer,
                new Parameter(false, "format", new PointerType(CharType.Default)),
                new Parameter(false, "args", VarArgsType.Default));

            _gcMallocProc = new GlobalIdent("GC_malloc", false, GlobalIdent.Options.NoAlias);
            _gcMallocProcType = new ProcedureType(PointerType.Byte,
                new Parameter(false, "size", IntegerType.Integer));

            var ctx = new GenerationContext();

            GlobalStringIdent.Reset();

            ctx.Header(module.Identifier, uniqueID);

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
                .DataLayoutEnd().Ln();

            ctx = ctx.Enter(0);
            ctx.TypeDecl(new TypeIdent(CharType.Default.ToString()), IntegerType.Byte);
            ctx.TypeDecl(new TypeIdent(SetType.Default.ToString()), IntegerType.LongInt);
            ctx = ctx.Leave().Ln().Ln();

            ctx.Lazy(x => {
                foreach (var kv in _stringConsts.OrderBy(y => y.Value.ID)) {
                    x.StringConstant(kv.Value, kv.Key);
                }
            });

            ctx.Global(_printfProc, _printfProcType);
            ctx.Global(_gcMallocProc, _gcMallocProcType);
            ctx.Ln();

            ctx.Module(module, true, entryModule);

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

        static int GetStringLength(String str)
        {
            return Encoding.UTF8.GetByteCount(str + "\0");
        }

        static OberonType GetStringType(String str)
        {
            return new PointerType(new StaticArrayType(CharType.Default, GetStringLength(str)));
        }

        static GenerationContext PushScope(this GenerationContext ctx, Scope scope)
        {
            _scope = scope;
            return ctx;
        }

        static GenerationContext PopScope(this GenerationContext ctx)
        {
            _scope = _scope.Parent;
            return ctx;
        }

        static GenerationContext Module(this GenerationContext ctx, NModule module, bool define, bool entry)
        {
            if (define) {
                foreach (var mdl in module.Imports) {
                    ctx.Module(((ModuleType) module.Type.Scope.GetSymbol(mdl)).Module, false, false);
                }
            }

            _module = module;
            _scope = module.Type.Scope;

            var types = _scope.GetTypes(false);
            if (types.Count() > 0) {
                ctx = ctx.Enter(0);
                foreach (var kv in types) {
                    var type = kv.Value.Type;
                    ctx.TypeDecl(new TypeIdent(kv.Key, module.Identifier), type);
                }
                ctx = ctx.Leave().Ln().Ln();
            }

            var records = _scope.GetTypes(false).Where(x => x.Value.Type is RecordType);
            if (records.Count() > 0) {
                ctx = ctx.Enter(0);
                foreach (var kv in records) {
                    ctx.RecordTable(kv.Key, (RecordType) kv.Value.Type, define).Ln();
                }
                ctx = ctx.Leave().Ln();
            }
            
            var globals = _scope.GetSymbols(false).Where(y => !y.Value.Type.IsProcedure && !y.Value.IsConstant);
            if (globals.Count() > 0) {
                ctx = ctx.Ln().Enter(0);
                foreach (var v in globals) {
                    ctx.Global(new QualIdent(v.Key), v.Value.Type);
                }
                ctx = ctx.Leave().Ln().Ln();
            }

            var procs = _module.Declarations.Procedures;
            if (procs.Count() > 0) {
                ctx = ctx.Enter(0);
                foreach (var proc in procs) {
                    ctx.Procedure(proc);
                }
                ctx = ctx.Leave().Ln();
            }

            var initIdent = new GlobalIdent(String.Format("{0}._init", module.Identifier), true);
            var initType = new ProcedureType(IntegerType.Integer);
            if (define) {
                var initFlag = new GlobalIdent(String.Format("{0}._hasInit", module.Identifier), false);
                ctx.Ln().Global(initFlag, BooleanType.Default).Ln();

                ctx.Procedure(initIdent, initType, new Scope(_scope),
                    (context) => {
                        var error = new TempIdent();
                        if (module.Body != null) {
                            var body = new TempIdent();
                            var end = new TempIdent();
                            var temp = new TempIdent();

                            context.Load(temp, BooleanType.Default, initFlag);
                            context.Branch(temp, end, body);
                            
                            context.LabelMarker(body);
                            context.Assign(initFlag, BooleanType.Default, new Literal(1)).Ln();

                            if (module.Imports.Count() > 0) {
                                TempIdent next = null;
                                context = context.Enter(0);
                                foreach (var import in module.Imports) {
                                    temp = new TempIdent();
                                    context.Call(temp, initType, new GlobalIdent(String.Format("{0}._init", import), true));

                                    var comp = new TempIdent();
                                    context.BinaryComp(comp, "eq", IntegerType.Integer, temp, new Literal(0));

                                    next = new TempIdent();
                                    context.Branch(comp, next, error);
                                    context.LabelMarker(next);
                                }
                                context = context.Leave().Ln().Ln();
                            }

                            context.Statements(module.Body);
                            context.Branch(end);

                            context.LabelMarker(end);
                        }
                        context.Keyword("ret").Argument(IntegerType.Integer, new Literal(0)).EndOperation();

                        if (error.Predecessors.Count() > 0) {
                            context.LabelMarker(error);
                            context.Keyword("ret").Argument(IntegerType.Integer, new Literal(1)).EndOperation();
                        }
                    });

                if (entry) {
                    var mainIdent = new GlobalIdent(String.Format("main", module.Identifier), false);
                    var mainType = new ProcedureType(IntegerType.Integer);
                    ctx.Procedure(mainIdent, mainType, new Scope(_scope),
                        (context) => {
                            var temp = new TempIdent();
                            context.Call(temp, initType, initIdent);
                            context.Keyword("ret").Argument(IntegerType.Integer, temp).EndOperation();
                        });
                }
            } else {
                ctx.Global(initIdent, initType);
            }

            return ctx.Ln();
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
                    return typeof(SubstituteNode).IsAssignableFrom(paras.Last().ParameterType);
                }).ToDictionary(x => x.GetParameters().Last().ParameterType);
            }

            if (_nodeMethods.ContainsKey(node.GetType())) {
                try {
                    return (GenerationContext) _nodeMethods[node.GetType()].Invoke(null, new object[] { ctx, node });
                } catch (TargetInvocationException e) {
                    ExceptionDispatchInfo.Capture(e.InnerException).Throw();
                    return null;
                }
            } else {
                throw new NotImplementedException(String.Format("No method of generating IR for nodes of type '{0}' found", node.GetType().Name));
            }
        }
    }
}
