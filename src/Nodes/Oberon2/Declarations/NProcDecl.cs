using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DUO2C.Semantics;

namespace DUO2C.Nodes.Oberon2
{
    [SubstituteToken("Receiver")]
    public class NReceiver : SubstituteNode, IDeclarationSource, ITypeErrorSource, IAccessibilityErrorSource
    {
        public bool ByReference { get; private set; }

        public String Identifier
        {
            get { return Children.First().String; }
        }

        [Serialize("type")]
        public String TypeName { get; private set; }

        public NReceiver(ParseNode original)
            : base(original, true)
        {
            ByReference = Children.ElementAt(1).Token == "keyword";
            Children = Children.Where(x => x.Token != "keyword");
            TypeName = Children.Last().String;
            Children = Children.Where(x => x is NIdent);
        }

        public void FindDeclarations(Scope scope)
        {
            scope.DeclareSymbol(Identifier, new UnresolvedType(TypeName), AccessModifier.Private, DeclarationType.Parameter);
        }

        public IEnumerable<CompilerException> FindTypeErrors(Scope scope)
        {
            if (!scope.IsTypeDeclared(TypeName)) {
                yield return new UndeclaredIdentifierException(Children.Last());
                yield break;
            }

            var decl = scope.GetTypeDecl(TypeName);
            if (!decl.Type.IsPointer || !decl.Type.As<PointerType>().ResolvedType.IsRecord) {
                yield return new TypeMismatchException(new PointerType(RecordType.Base), decl.Type, this);
            }
        }

        public IEnumerable<CompilerException> FindAccessibilityErrors(Scope scope)
        {
            if (scope.GetTypeVisibility(TypeName) == AccessModifier.Private) {
                yield return new AccessibilityException(Children.Last());
            }
        }
    }

    [SubstituteToken("FPSection")]
    public class NFPSection : SubstituteNode, IDeclarationSource, ITypeErrorSource, IAccessibilityErrorSource
    {
        [Serialize("byref")]
        public bool ByReference { get; private set; }

        public NType Type
        {
            get { return (NType) Children.Last(); }
        }

        public IEnumerable<String> Identifiers
        {
            get { return Children.Where(x => x is NIdent).Select(x => x.String); }
        }

        public NFPSection(ParseNode original)
            : base(original, false)
        {
            ByReference = Children.First().Token == "keyword";

            Children = Children.Where(x => x is NIdent || x is NType);
        }

        public void FindDeclarations(Scope scope)
        {
            foreach (var ident in Identifiers) {
                scope.DeclareSymbol(ident, Type.Type, AccessModifier.Private, DeclarationType.Parameter);
            }
        }

        public IEnumerable<CompilerException> FindTypeErrors(Scope scope)
        {
            return Type.FindTypeErrors(scope);
        }

        public IEnumerable<CompilerException> FindAccessibilityErrors(Scope scope)
        {
            return Type.FindAccessibilityErrors(scope);
        }
    }

    [SubstituteToken("FormalPars")]
    public class NFormalPars : SubstituteNode, IDeclarationSource, ITypeErrorSource, IAccessibilityErrorSource
    {
        public IEnumerable<NFPSection> FPSections
        {
            get { return Children.Where(x => x is NFPSection).Select(x => (NFPSection) x); }
        }

        public NType ReturnType
        {
            get { return (NType) Children.FirstOrDefault(x => x is NType); }
        }

        public NFormalPars(ParseNode original)
            : base(original, false)
        {
            Children = Children.Where(x => x is NFPSection || x is NType);
        }

        public void FindDeclarations(Scope scope)
        {
            foreach (var section in FPSections) {
                section.FindDeclarations(scope);
            }
        }

        public IEnumerable<CompilerException> FindTypeErrors(Scope scope)
        {
            if (ReturnType != null) {
                bool found = false;
                foreach (var e in ReturnType.FindTypeErrors(scope)) {
                    found = true;
                    yield return e;
                }
                if (!found && ReturnType.Type.IsArray || ReturnType.Type.IsRecord) {
                    yield return new CompilerException(ParserError.Semantics, "Illegal return type", StartIndex, Length);
                }
            }

            foreach (var sec in FPSections) {
                foreach (var e in sec.FindTypeErrors(scope)) {
                    yield return e;
                }
            }
        }

        public IEnumerable<CompilerException> FindAccessibilityErrors(Scope scope)
        {
            if (ReturnType != null) {
                foreach (var e in ReturnType.FindAccessibilityErrors(scope)) {
                    yield return e;
                }
            }

            foreach (var sec in FPSections) {
                foreach (var e in sec.FindAccessibilityErrors(scope)) {
                    yield return e;
                }
            }
        }
    }

    [SubstituteToken("ForwardDecl")]
    public class NForwardDecl : DeclarationStatement
    {
        public NReceiver Receiver
        {
            get { return Children.FirstOrDefault() as NReceiver; }
        }

        public NFormalPars FormalParams
        {
            get { return (NFormalPars) Children.FirstOrDefault(x => x is NFormalPars); }
        }

        public IEnumerable<NFPSection> FPSections
        {
            get {
                var pars = FormalParams;
                if (pars == null) return null;
                return pars.FPSections;
            }
        }

        public NType ReturnType
        {
            get {
                var pars = FormalParams;
                if (pars == null) return null;
                return pars.ReturnType;
            }
        }

        private NIdentDef _identDef;
        public override NIdentDef IdentDef
        {
            get { return _identDef; }
        }

        public NForwardDecl(ParseNode original)
            : base(original)
        {
            _identDef = (NIdentDef) Children.First(x => x is NIdentDef);

            if (GetType() == typeof(NForwardDecl)) {
                Children = Children.Where(x => !(x is NIdent || x.Token == "keyword"));
            }
        }

        protected virtual ProcedureType GetSignature()
        {
            return new ProcedureType(FormalParams);
        }

        public override void FindDeclarations(Scope scope)
        {
            if (FormalParams != null && FormalParams.FindTypeErrors(scope).Count() > 0) return;

            if (Receiver == null) {
                scope.DeclareSymbol(Identifier, GetSignature(), Visibility, DeclarationType.Global);
            } else {
                var type = scope.GetType(Receiver.TypeName);

                while (type != null && type.IsPointer) {
                    type = type.As<PointerType>().ResolvedType;
                    if (type != null) type.Resolve(scope);
                }

                if (type != null && type.IsRecord) {
                    type.As<RecordType>().BindProcedure(Identifier, Visibility, new ProcedureType(FormalParams),
                        new UnresolvedType(Receiver.TypeName, scope.CurrentModule.Identifier), Receiver.Identifier);
                }
            }
        }

        public override IEnumerable<CompilerException> FindTypeErrors(Scope scope)
        {
            bool exported = Visibility != AccessModifier.Private;
            if (Receiver != null) {
                bool found = false;
                foreach (var e in Receiver.FindTypeErrors(scope)) {
                    yield return e;
                    found = true;
                }

                if (!found && exported) {
                    foreach (var e in Receiver.FindAccessibilityErrors(scope)) {
                        yield return e;
                        found = true;
                    }
                    exported = scope.GetTypeDecl(Receiver.TypeName, null).Visibility != AccessModifier.Private;
                }
            }

            if (exported && FormalParams != null) {
                foreach (var e in FormalParams.FindAccessibilityErrors(scope)) {
                    yield return e;
                }
            }

            if (FormalParams != null) {
                foreach (var e in FormalParams.FindTypeErrors(scope)) {
                    yield return e;
                }
            }
        }
    }

    [SubstituteToken("ExternalDecl")]
    public class NExternalDecl : NForwardDecl
    {
        public Scope Scope { get; private set; }
        public bool IsImport { get; private set; }
        public String SymbolName { get; private set; }

        public NExternalDecl(ParseNode original)
            : base(original)
        {
            IsImport = Children.ElementAt(Children.Count() - 2).String == "IMPORT";
            SymbolName = Children.Last().String;

            Children = Children.Where(x => !(x is NIdent || x.Token == "keyword"));
        }

        protected override ProcedureType GetSignature()
        {
            return new ExternalProcedureType(FormalParams, SymbolName, IsImport);
        }

        public override void FindDeclarations(Scope scope)
        {
            base.FindDeclarations(scope);

            Scope = new Scope(scope);

            foreach (var child in Children.Where(x => x is IDeclarationSource)) {
                ((IDeclarationSource) child).FindDeclarations(Scope);
            }
        }
    }

    [SubstituteToken("ProcDecl")]
    public class NProcDecl : NForwardDecl
    {
        public Scope Scope { get; private set; }

        public NStatementSeq Statements
        {
            get { return (NStatementSeq) Children.FirstOrDefault(x => x is NStatementSeq); }
        }

        public NProcDecl(ParseNode original)
            : base(original)
        {
            Children = Children.Where(x => !(x is NIdent || x.Token == "keyword"));
        }

        public override void FindDeclarations(Scope scope)
        {
            base.FindDeclarations(scope);

            Scope = new Scope(scope);

            foreach (var child in Children.Where(x => x is IDeclarationSource)) {
                ((IDeclarationSource) child).FindDeclarations(Scope);
            }
        }

        public override IEnumerable<CompilerException> FindTypeErrors(Scope scope)
        {
            foreach (var e in base.FindTypeErrors(scope).Union(Children.SelectMany(x => (x is ITypeErrorSource)
                ? ((ITypeErrorSource) x).FindTypeErrors(Scope)
                : new CompilerException[0]))) {
                yield return e;   
            }

            if (ReturnType == null || ReturnType.Inner.FindTypeErrors(scope).Count() == 0) {
                var retType = ReturnType != null ? ReturnType.Type.Resolve(scope) : null;
                foreach (var stmnt in Statements.Statements) {
                    if (stmnt.Inner is NReturn) {
                        var ret = (NReturn) stmnt.Inner;
                        if (ret.Expression == null && retType != null) {
                            yield return new CompilerException(ParserError.Semantics,
                                "Expected return expression", ret.EndIndex, 0);
                        } else if (ret.Expression != null && retType == null) {
                            yield return new CompilerException(ParserError.Semantics,
                                "Unexpected return expression", ret.Expression.StartIndex, ret.Expression.Length);
                        } else if (ret.Expression != null && ret.Expression.FindTypeErrors(scope).Count() == 0) {
                            var t = ret.Expression.GetFinalType(scope);
                            if (!retType.CanTestEquality(t)) {
                                yield return new TypeMismatchException(retType, t, ret.Expression);
                            }
                        }
                    }
                }
            }
        }
    }
}
