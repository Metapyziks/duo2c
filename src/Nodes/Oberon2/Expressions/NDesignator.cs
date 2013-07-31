using System;
using System.Collections.Generic;
using System.Linq;

using DUO2C.Semantics;

namespace DUO2C.Nodes.Oberon2
{
    public abstract class Selector : SubstituteNode
    {
        public Selector(ParseNode original, bool leaf = false, bool hasPayload = true)
            : base(original, leaf, hasPayload) { }
    }

    /// <summary>
    /// Substitution node for designators, which will either
    /// contain a single NQualifiedIdentifier or a NDesignator
    /// followed by an operation. 
    /// </summary>
    [SubstituteToken("Designator")]
    public class NDesignator : ExpressionElement
    {
        public bool IsRoot
        {
            get { return Element is NQualIdent; }
        }

        public ParseNode Element
        {
            get { return Children.First(); }
        }

        public Selector Operation
        {
            get { return (Selector) Children.Last(); }
        }

        public override string String
        {
            get {return IsRoot ? Element.String : String.Format("{0}{1}", Element.String, Operation.String); }
        }

        public override OberonType GetFinalType(Scope scope)
        {
            if (IsRoot) {
                var ident = (NQualIdent) Element;
                return scope.GetSymbol(ident.Identifier, ident.Module);
            } else {
                var prev = (NDesignator) Element;
                var type = prev.GetFinalType(scope);
                
                if (Operation is NIndexer || Operation is NMemberAccess) {
                    while (type.IsPointer) {
                        type = type.As<PointerType>().ResolvedType;
                    }
                }

                if (Operation is NMemberAccess) {
                    var op = (NMemberAccess) Operation;
                    if (type.IsModule) {
                        var mdl = type.As<ModuleType>();
                        return mdl.Scope.GetSymbol(op.Identifier).Resolve(scope);
                    } else if (type.IsRecord) {
                        var rec = type.As<RecordType>();
                        return rec.GetFieldType(op.Identifier).Resolve(scope);
                    }
                } else if (Operation is NIndexer) {
                    return type.As<ArrayType>().ElementType.Resolve(scope);
                } else if (Operation is NPtrResolve) {
                    return type.As<PointerType>().ResolvedType.Resolve(scope);
                } else if (Operation is NInvocation) {
                    var op = (NInvocation) Operation;
                    if (type.IsProcedure) {
                        return (type.As<ProcedureType>().ReturnType ?? PointerType.NilPointer).Resolve(scope);
                    } else {
                        var arg = op.Args.Expressions.First();
                        var fact = arg.SimpleExpr.Term.Factor.Inner as NDesignator;
                        var ident = (NQualIdent) fact.Element;
                        return scope.GetType(ident.Identifier, ident.Module);
                    }
                }

                return PointerType.NilPointer;
            }
        }

        public override bool IsConstant(Scope scope)
        {
            if (IsRoot) {
                var ident = (NQualIdent) Element;
                return scope.IsSymbolConstant(ident.Identifier, ident.Module);
            } else {
                return false;
            }
        }

        public NDesignator(ParseNode original)
            : base(original, false)
        {
            if (Children.Count() >= 2 && Element is NQualIdent) {
                var prev = Children.Take(Children.Count() - 1);
                Children = new ParseNode[] { 
                    new NDesignator(new BranchNode(prev, Token)),
                    Children.Last()
                };
            }
        }

        public override IEnumerable<CompilerException> FindTypeErrors(Scope scope)
        {
            bool foundInner = false;
            if (!IsRoot) {
                var exceptions = ((NDesignator) Element).FindTypeErrors(scope);
                foreach (var e in exceptions) {
                    foundInner = true;
                    yield return e;
                }
            } else {
                var type = GetFinalType(scope);
                if (type == null || (type is UnresolvedType && type.As<OberonType>() == null)) {
                    yield return new UndeclaredIdentifierException(this);
                }
            }

            if (!foundInner && !IsRoot) {
                var prev = (NDesignator) Element;
                var type = prev.GetFinalType(scope);
                type.Resolve(scope);

                if (Operation is NIndexer || Operation is NMemberAccess) {
                    while (type != null && type.IsPointer) {
                        type = type.As<PointerType>().ResolvedType;
                        if (type != null) type.Resolve(scope);
                    }
                }

                if (type == null) {
                    yield return new UndeclaredIdentifierException(Element);
                } else {
                    if (Operation is NMemberAccess) {
                        var op = (NMemberAccess) Operation;
                        if (type.IsModule) {
                            var mdl = type.As<ModuleType>();
                            if (!mdl.Scope.IsSymbolDeclared(op.Identifier)) {
                                yield return new MemberNotFoundException(type, this);
                            } else {
                                var arr = (ParseNode[]) Children;
                                Children = new ParseNode[] { new NQualIdent(op.Identifier, mdl.Identifier, this) };
                            }
                        } else if (type.IsRecord) {
                            var rec = type.As<RecordType>();
                            if (!rec.HasField(op.Identifier)) {
                                yield return new MemberNotFoundException(type, this);
                            }
                        } else {
                            yield return new OperandTypeException(type, ".", this);
                        }
                    } else if (Operation is NIndexer) {
                        var op = (NIndexer) Operation;
                        if (!type.IsArray) {
                            yield return new ArrayExpectedException(type, this);
                        }
                    } else if (Operation is NPtrResolve) {
                        if (!type.IsPointer) {
                            yield return new PointerExpectedException(type, this);
                        }
                    } else if (Operation is NInvocation) {
                        var op = (NInvocation) Operation;

                        if (type.IsProcedure) {
                            if (op.Args != null) {
                                foreach (var e in op.Args.Expressions.SelectMany(x => x.FindTypeErrors(scope))) {
                                    yield return e;
                                }
                            }

                            var proc = type.As<ProcedureType>();
                            foreach (var e in proc.MatchParameters(op, scope)) {
                                yield return e;
                            }
                        } else {
                            var badProc = true;
                            if (op.Args.Expressions.Count() == 1) {
                                var arg = op.Args.Expressions.First();
                                if (arg.Prev == null && arg.SimpleExpr.Prev == null
                                    && arg.SimpleExpr.Term.Prev == null) {
                                    var fact = arg.SimpleExpr.Term.Factor.Inner as NDesignator;
                                    if (fact != null && fact.IsRoot) {
                                        badProc = false;
                                        var ident = (NQualIdent) fact.Element;
                                        var newType = scope.GetType(ident.Identifier, ident.Module);
                                        if (newType == null) {
                                            yield return new UndeclaredIdentifierException(ident);
                                        } else if (!OberonType.CanTestEquality(type, newType)) {
                                            yield return new CompilerException(ParserError.Semantics,
                                                String.Format("Cannot convert from {0} to {1}", type, newType),
                                                StartIndex, Length);
                                        }
                                    }
                                }
                            }
                            if (badProc) {
                                yield return new ProcedureExpectedException(type, Element);
                            }
                        }
                    }
                }
            }
        }
    }
}
