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

        static int GetStringLength(String str)
        {
            return Encoding.UTF8.GetByteCount(str + "\0");
        }

        static OberonType GetStringType(String str)
        {
            return new PointerType(new ConstArrayType(CharType.Default, GetStringLength(str)));
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

        static GenerationContext WriteModule(this GenerationContext ctx, NModule module, Guid uniqueID)
        {
            _module = module;
            _scope = module.Type.Scope;
            _stringConsts = new Dictionary<string, GlobalStringIdent>();

            _printfProc = new GlobalIdent("printf", false, GlobalIdent.Options.NoUnwind);
            _printfProcType = new ProcedureType(IntegerType.Integer,
                new Parameter(false, "format", new PointerType(CharType.Default)),
                new Parameter(false, "args", VarArgsType.Default));

            _gcMallocProc = new GlobalIdent("GC_malloc", false, GlobalIdent.Options.NoAlias);
            _gcMallocProcType = new ProcedureType(PointerType.Byte,
                new Parameter(false, "size", IntegerType.Integer));

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
                .DataLayoutEnd().Ln();
            
            ctx = ctx.Enter(0);
            ctx.TypeDecl(new TypeIdent(CharType.Default.ToString()), IntegerType.Byte);
            ctx.TypeDecl(new TypeIdent(SetType.Default.ToString()), IntegerType.LongInt);

            foreach (var kv in _scope.GetTypes(false)) {
                ctx.TypeDecl(new TypeIdent(kv.Key, module.Identifier), kv.Value.Type);
            }
            ctx = ctx.Leave().Ln().Ln();

            ctx = ctx.Enter(0);
            foreach (var kv in _scope.GetTypes(false).Where(x => x.Value.Type is RecordType)) {
                ctx.RecordTable(kv.Key, (RecordType) kv.Value.Type).Ln();
            }
            ctx = ctx.Leave().Ln();

            ctx.Lazy(x => {
                foreach (var kv in _stringConsts.OrderBy(y => y.Value.ID)) {
                    x.StringConstant(kv.Value, kv.Key);
                }
            });
            
            ctx.Global(_printfProc, _printfProcType);
            ctx.Global(_gcMallocProc, _gcMallocProcType);
            
            var globals = _scope.GetSymbols(false).Where(y => !y.Value.Type.IsProcedure);
            if (globals.Count() > 0) {
                ctx = ctx.Ln().Enter(0);
                foreach (var v in globals) {
                    ctx.Global(new QualIdent(v.Key), v.Value.Type);
                }
                ctx = ctx.Leave().Ln().Ln();
            }

            var procs = _module.Declarations.Procedures.Where(y => y is NProcDecl).Cast<NProcDecl>();
            if (procs.Count() > 0) {
                ctx = ctx.Enter(0);
                foreach (var proc in procs) {
                    ctx.Procedure(proc);
                }
                ctx = ctx.Leave().Ln();
            }

            ctx.Procedure(new GlobalIdent("main", false), new ProcedureType(IntegerType.Integer), new Scope(_scope),
                (context) => {
                    context.Statements(module.Body);
                    context.Keyword("ret").Argument(IntegerType.Integer, new Literal(0.ToString())).EndOperation();
                });

            return ctx;
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
