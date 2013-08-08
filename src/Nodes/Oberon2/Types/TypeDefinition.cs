using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DUO2C.CodeGen;
using DUO2C.Semantics;

namespace DUO2C.Nodes.Oberon2
{
    public abstract class TypeDefinition : SubstituteNode, ITypeErrorSource, IAccessibilityErrorSource
    {
        public abstract OberonType Type { get; }

        public override string String
        {
            get { return (Type ?? PointerType.Null).ToString(); }
        }

        public TypeDefinition(ParseNode original, bool leaf, bool hasPayload = true)
            : base(original, leaf, hasPayload) { }

        public virtual IEnumerable<CompilerException> FindTypeErrors(Scope scope)
        {
            return Children.SelectMany(x => (x is ITypeErrorSource)
                   ? ((ITypeErrorSource) x).FindTypeErrors(scope)
                   : new CompilerException[0]);
        }

        public virtual IEnumerable<CompilerException> FindAccessibilityErrors(Scope scope)
        {
            return Children.SelectMany(x => (x is IAccessibilityErrorSource)
                ? ((IAccessibilityErrorSource) x).FindAccessibilityErrors(scope)
                : new CompilerException[0]);
        }
    }

    [SubstituteToken("ArrayType")]
    public class NArrayType : TypeDefinition
    {
        [Serialize("length")]
        public int ArrayLength { get; private set; }

        public NConstExpr LengthExpr
        {
            get { return Children.First() as NConstExpr; }
        }

        public NType ElementDefinition
        {
            get { return (NType) Children.Last(); }
        }

        public override OberonType Type
        {
            get { return new ArrayType(ElementDefinition.Type, ArrayLength); }
        }

        public NArrayType(ParseNode original)
            : base(original, false)
        {
            Children = Children.Where(x => x is NConstExpr || x is NType);

            if (LengthExpr != null) {
                ArrayLength = int.Parse(LengthExpr.String);
            } else {
                ArrayLength = -1;
            }

            if (Children.Count() > 2) {
                Children = new ParseNode[] {
                    LengthExpr,
                    new NType(new BranchNode(new ParseNode[] { new NArrayType(new BranchNode(Children.Skip(1), Token)) }, "Type"))
                };
            }

            Children = Children.Where(x => x is NType);
        }
    }

    [SubstituteToken("OberonType")]
    public class NOberonType : TypeDefinition
    {
        private OberonType _type;
        public override OberonType Type
        {
            get { return _type; }
        }

        public NOberonType(ParseNode original)
            : base(original, true)
        {
            switch (original.String) {
                case "LONGINT":
                    _type = IntegerType.LongInt; break;
                case "INTEGER":
                    _type = IntegerType.Integer; break;
                case "SHORTINT":
                    _type = IntegerType.ShortInt; break;
                case "BYTE":
                    _type = IntegerType.Byte; break;
                case "LONGREAL":
                    _type = RealType.LongReal; break;
                case "REAL":
                    _type = RealType.Real; break;
                case "BOOLEAN":
                    _type = BooleanType.Default; break;
                case "SET":
                    _type = SetType.Default; break;
                case "CHAR":
                    _type = CharType.Default; break;
            }
        }
    }

    [SubstituteToken("PtrType")]
    public class NPtrType : TypeDefinition
    {
        public override OberonType Type
        {
            get { return new PointerType(((NType) Children.First()).Type); }
        }

        public NPtrType(ParseNode original)
            : base(original, false)
        {
            Children = Children.Where(x => x is NType);
        }
    }

    [SubstituteToken("NamedType")]
    public class NNamedType : TypeDefinition
    {
        public NQualIdent Identifier
        {
            get { return (NQualIdent) Children.First(); }
        }

        public override OberonType Type
        {
            get { return new UnresolvedType(Identifier.Identifier, Identifier.Module); }
        }

        public NNamedType(ParseNode original)
            : base(original, false)
        {
            Children = Children.Where(x => x is NQualIdent);
        }

        public override IEnumerable<CompilerException> FindTypeErrors(Scope scope)
        {
            if (!scope.IsTypeDeclared(Identifier.Identifier, Identifier.Module)) {
                yield return new UndeclaredIdentifierException(this);
            }
        }

        public override IEnumerable<CompilerException> FindAccessibilityErrors(Scope scope)
        {
            if (scope.IsTypeDeclared(Identifier.Identifier, Identifier.Module)
                && scope.GetTypeDecl(Identifier.Identifier, Identifier.Module).Visibility == AccessModifier.Private) {
                yield return new AccessibilityException(Identifier);
            }
        }
    }

    [SubstituteToken("FieldList")]
    public class NFieldList : SubstituteNode, ITypeErrorSource, IAccessibilityErrorSource
    {
        public NIdentList Identifiers
        {
            get { return (NIdentList) Children.First(); }
        }

        public NType Type
        {
            get { return (NType) Children.Last(); }
        }

        public NFieldList(ParseNode original)
            : base(original, false)
        {
            Children = Children.Where(x => x is NIdentList || x is NType);
        }

        public IEnumerable<CompilerException> FindTypeErrors(Scope scope)
        {
            return Children.SelectMany(x => (x is ITypeErrorSource)
                   ? ((ITypeErrorSource) x).FindTypeErrors(scope)
                   : new CompilerException[0]);
        }

        public IEnumerable<CompilerException> FindAccessibilityErrors(Scope scope)
        {
            return Type.FindAccessibilityErrors(scope);
        }
    }

    [SubstituteToken("RecordType")]
    public class NRecordType : TypeDefinition, ITypeErrorSource
    {
        public NNamedType SuperRecord
        {
            get { return Children.FirstOrDefault() as NNamedType; }
        }

        public IEnumerable<NFieldList> FieldLists
        {
            get { return Children.Where(x => x is NFieldList).Select(x => (NFieldList) x); }
        }

        public override OberonType Type
        {
            get { return new RecordType(this); }
        }

        public IEnumerable<KeyValuePair<NIdentDef, NType>> Fields
        {
            get {
                return Children.Where(x => x is NFieldList).SelectMany(x => {
                    var fl = (NFieldList) x;
                    return fl.Identifiers.IdentDefs.Select(y => new KeyValuePair<NIdentDef, NType>(y, fl.Type));
                });
            }
        }

        public NRecordType(ParseNode original)
            : base(original, false)
        {
            Children = Children.Where(x => x is NNamedType || x is NFieldList);
        }
    }

    [SubstituteToken("ProcType")]
    public class NProcType : TypeDefinition
    {
        public IEnumerable<NFPSection> FPSections
        {
            get {
                var pars = (NFormalPars) Children.FirstOrDefault(x => x is NFormalPars);
                if (pars == null) return null;
                return pars.FPSections;
            }
        }

        public NType ReturnType
        {
            get {
                var pars = (NFormalPars) Children.FirstOrDefault(x => x is NFormalPars);
                if (pars == null) return null;
                return pars.ReturnType;
            }
        }

        public override OberonType Type
        {
            get { return new ProcedureType((NFormalPars) Children.FirstOrDefault()); }
        }

        public NProcType(ParseNode original)
            : base(original, false)
        {
            Children = Children.Where(x => x is NFormalPars);
        }
    }
}
