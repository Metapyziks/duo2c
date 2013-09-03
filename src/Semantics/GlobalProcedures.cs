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
        }
    }
}
