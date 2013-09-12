using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUO2C.Semantics
{
    public partial class RootScope
    {
        public void DeclareGlobals()
        {
            DeclareSymbol("NEW", new OverloadedProcedureType(null,
                (invoc, argPairs, scope) => {
                    var exceptions = new List<CompilerException>();
                    if (argPairs.Length == 0) {
                        exceptions.Add(new CompilerException(ParserError.Semantics, "Arguments expected",
                            invoc.StartIndex, invoc.Length));
                    } else {
                        if (!argPairs.First().Key.IsPointer && !argPairs.First().Key.IsArray) {
                            var argNode = invoc.Args.Expressions.First();
                            exceptions.Add(new CompilerException(ParserError.Semantics, "Pointer or open array expected",
                                argNode.StartIndex, argNode.Length));
                        }
                        if (argPairs.First().Key.IsPointer) {
                            if (argPairs.Length > 1) {
                                exceptions.Add(new CompilerException(ParserError.Semantics,
                                    String.Format("Argument count mismatch, expected "
                                        + "{0}, received {1}", 1, argPairs.Length),
                                        invoc.StartIndex, invoc.Length));
                            }
                        } else {
                            int depth = 0;
                            var outer = argPairs.First().Key.As<ArrayType>();

                            if (!outer.IsOpen) {
                                exceptions.Add(new CompilerException(ParserError.Semantics, "Pointer or open array expected",
                                    argPairs.First().Value.StartIndex, argPairs.First().Value.Length));
                            } else {
                                var inner = outer;
                                while (inner != null) {
                                    ++depth;
                                    inner = inner.ElementType.IsArray
                                        ? inner.ElementType.As<ArrayType>()
                                        : null;
                                }

                                if (argPairs.Length != depth + 1) {
                                    exceptions.Add(new CompilerException(ParserError.Semantics,
                                        String.Format("Argument count mismatch, expected {0}, received {1}",
                                            depth + 1, argPairs.Length),
                                        invoc.StartIndex, invoc.Length));
                                }

                                for (int i = 1; i < argPairs.Length; ++i) {
                                    if (!argPairs[i].Key.IsInteger) {
                                        exceptions.Add(new TypeMismatchException(IntegerType.Integer,
                                            argPairs[i].Key, invoc.Args.Expressions.ElementAt(i)));
                                    }
                                }
                            }
                        }
                    }

                    return exceptions;
                }
            ), AccessModifier.Private, DeclarationType.Global);

            ArgumentCheckDelegate vecOpArgCheck = (invoc, argPairs, scope) => {
                var exceptions = new List<CompilerException>();
                if (argPairs.Length < 2 || argPairs.Length > 3) {
                    exceptions.Add(new CompilerException(ParserError.Semantics,
                        String.Format("Argument count mismatch, expected {0}, received {1}",
                            2, argPairs.Length),
                        invoc.StartIndex, invoc.Length));
                } else {
                    var arg1 = argPairs.First();
                    var arg2 = argPairs.ElementAt(1);
                    var arg3 = argPairs.ElementAtOrDefault(2);

                    if (!arg1.Key.IsArray) {
                        exceptions.Add(new TypeMismatchException(new ArrayType(null), arg1.Key, arg1.Value));
                    }
                    if (!arg2.Key.IsVector) {
                        exceptions.Add(new TypeMismatchException(new VectorType(null, 0), arg2.Key, arg2.Value));
                    }
                    if (exceptions.Count == 0) {
                        var arr = arg1.Key.As<ArrayType>();
                        var vec = arg2.Key.As<VectorType>();

                        if (!arr.ElementType.Equals(vec.ElementType)) {
                            exceptions.Add(new TypeMismatchException(new ArrayType(vec.ElementType), arr, arg1.Value));
                        }
                    }
                    if (arg3.Key != null && !arg3.Key.IsInteger) {
                        exceptions.Add(new TypeMismatchException(IntegerType.Integer, arg3.Key, arg3.Value));
                    }
                }
                return exceptions;
            };

            DeclareSymbol("VECLOAD", new OverloadedProcedureType(null, vecOpArgCheck),
                AccessModifier.Private, DeclarationType.Global);

            DeclareSymbol("VECSTORE", new OverloadedProcedureType(null, vecOpArgCheck),
                AccessModifier.Private, DeclarationType.Global);
        }
    }
}
