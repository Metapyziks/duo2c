using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DUO2C.Semantics;

namespace DUO2C.Nodes.Oberon2
{
    [SubstituteToken("Receiver")]
    public class NReceiver : SubstituteNode, IDeclarationSource
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
            scope.Declare(Identifier, new UnresolvedType(TypeName));
        }
    }

    [SubstituteToken("FPSection")]
    public class NFPSection : SubstituteNode, IDeclarationSource
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
                scope.Declare(ident, Type.Type);
            }
        }
    }

    [SubstituteToken("FormalPars")]
    public class NFormalPars : SubstituteNode, IDeclarationSource
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
    }

    [SubstituteToken("ForwardDecl")]
    public class NForwardDecl : Declaration
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
            Children = Children.Where(x => x.Token != "keyword");
            _identDef = (NIdentDef) Children.First(x => x is NIdentDef);
            Children = Children.Where(x => !(x is NIdent));
        }

        public override void FindDeclarations(Scope scope)
        {
            if (Receiver == null) {
                scope.Declare(Identifier, new ProcedureType(FormalParams));
            } else {
                var type = scope[Receiver.TypeName];

                while (type != null && type.IsPointer) {
                    type = type.As<PointerType>().ResolvedType;
                    if (type != null) type.Resolve(scope);
                }

                if (type != null && type.IsRecord) {
                    type.As<RecordType>().BindProcedure(Identifier, Visibility, new ProcedureType(FormalParams));
                }
            }
        }

        public override IEnumerable<ParserException> FindTypeErrors(Scope scope)
        {
            if (Receiver != null) {
                var type = scope[Receiver.TypeName];

                while (type != null && type.IsPointer) {
                    type = type.As<PointerType>().ResolvedType;
                    if (type != null) type.Resolve(scope);
                }

                if (type == null) {
                    yield return new UndeclaredIdentifierException(Receiver.Children.Last());
                } else if (!type.IsRecord) {
                    yield return new TypeMismatchException(RecordType.Base, type, Receiver);
                }
            }
        }
    }

    [SubstituteToken("ProcDecl")]
    public class NProcDecl : NForwardDecl
    {
        private Scope _scope;

        public NStatementSeq Statements
        {
            get { return (NStatementSeq) Children.FirstOrDefault(x => x is NStatementSeq); }
        }

        public NProcDecl(ParseNode original)
            : base(original) { }

        public override void FindDeclarations(Scope scope)
        {
            base.FindDeclarations(scope);

            _scope = new Scope(scope);

            foreach (var child in Children.Where(x => x is IDeclarationSource)) {
                ((IDeclarationSource) child).FindDeclarations(_scope);
            }
        }

        public override IEnumerable<ParserException> FindTypeErrors(Scope scope)
        {
            return base.FindTypeErrors(scope).Union(Children.SelectMany(x => (x is ITypeErrorSource)
                ? ((ITypeErrorSource) x).FindTypeErrors(_scope)
                : new ParserException[0]));
        }
    }
}
