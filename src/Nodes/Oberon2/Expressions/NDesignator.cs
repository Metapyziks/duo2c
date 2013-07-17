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
            get { return String.Format("{0}{1}", Element.String, Operation.String); }
        }

        public override OberonType GetFinalType(Scope scope)
        {
            if (IsRoot) {
                var ident = (NQualIdent) Element;
                return scope[ident.Identifier, ident.Module];
            } else {
                var prev = (NDesignator) Element;
                var type = prev.GetFinalType(scope);

                while (type.IsPointer) {
                    type = type.As<PointerType>().ResolvedType;
                }

                if (Operation is NMemberAccess) {
                    var op = (NMemberAccess) Operation;
                    if (type.IsModule) {
                        var mdl = type.As<ModuleType>();
                        return mdl.Scope[op.Identifier];
                    } else if (type.IsRecord) {
                        var rec = type.As<RecordType>();
                        return rec.GetFieldType(op.Identifier);
                    }
                } else if (Operation is NIndexer) {
                    var op = (NIndexer) Operation;
                } else if (Operation is NPtrResolve) {
                    var op = (NPtrResolve) Operation;
                } else if (Operation is NTypeGuard) {
                    var op = (NTypeGuard) Operation;
                }

                return PointerType.NilPointer;
            }
        }

        public override bool IsConstant(Scope scope)
        {
            return false;
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

        public override IEnumerable<ParserException> FindTypeErrors(Scope scope)
        {
            bool foundInner = false;
            if (!IsRoot) {
                var exceptions = ((NDesignator) Element).FindTypeErrors(scope);
                foreach (var e in exceptions) {
                    foundInner = true;
                    yield return e;
                }
            } else {
                if (GetFinalType(scope) == null) {
                    yield return new UndeclaredIdentifierException(this);
                }
            }

            if (!foundInner && !IsRoot) {
                var prev = (NDesignator) Element;
                var type = prev.GetFinalType(scope);
                type.Resolve(scope);

                while (type != null && type.IsPointer) {
                    type = type.As<PointerType>().ResolvedType;
                    if (type != null) type.Resolve(scope);
                }

                if (type == null) {
                    yield return new UndeclaredIdentifierException(Element);
                } else {
                    if (Operation is NMemberAccess) {
                        var op = (NMemberAccess) Operation;
                        if (type.IsModule) {
                            var mdl = type.As<ModuleType>();
                            if (!mdl.Scope.IsDeclared(op.Identifier)) {
                                yield return new MemberNotFoundException(type, this);
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
                    } else if (Operation is NPtrResolve) {
                        var op = (NPtrResolve) Operation;
                    } else if (Operation is NTypeGuard) {
                        var op = (NTypeGuard) Operation;
                    }
                }
            }
        }
    }
}
