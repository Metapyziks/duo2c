using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DUO2C.CodeGen;
using DUO2C.Semantics;

namespace DUO2C.Nodes.Oberon2
{
    public abstract class TypeDefinition : SubstituteNode, ITypeErrorSource
    {
        public abstract OberonType Type { get; }

        public TypeDefinition(ParseNode original, bool leaf, bool hasPayload = true)
            : base(original, leaf, hasPayload) { }

        public virtual IEnumerable<ParserException> FindTypeErrors(Scope scope)
        {
            return Children.SelectMany(x => (x is ITypeErrorSource)
                   ? ((ITypeErrorSource) x).FindTypeErrors(scope)
                   : new ParserException[0]);
        }
    }

    [SubstituteToken("ArrayType")]
    public class NArrayType : TypeDefinition
    {
        [Serialize("length")]
        public int ArrayLength { get; private set; }

        public NConstExpr LengthExpr
        {
            get { return (NConstExpr) Children.First(); }
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

            // Temporary
            ArrayLength = int.Parse(LengthExpr.String);

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
            switch (String.ToUpper()) {
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

        public override void GenerateCode(GenerationContext ctx)
        {
            if (_type is IntegerType) {
                var it = (IntegerType) _type;
                ctx.Write("i{0}", (int) it.Range);
            } else if (_type is RealType) {
                var rt = (RealType) _type;
                if (rt.Range == RealRange.Real) {
                    ctx.Write("float");
                } else {
                    ctx.Write("double");
                }
            } else if (_type is SetType) {
                ctx.Write("i64");
            } else if (_type is BooleanType) {
                ctx.Write("i1");
            } else if (_type is CharType) {
                ctx.Write("i16");
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

        public override void GenerateCode(GenerationContext ctx)
        {
            ctx = ctx + (NType) Children.First() + " *";
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

        public override IEnumerable<ParserException> FindTypeErrors(Scope scope)
        {
            if (scope[Identifier.Identifier, Identifier.Module] == null) {
                yield return new UndeclaredIdentifierException(this);
            }
        }

        public override void GenerateCode(GenerationContext ctx)
        {
            ctx = ctx + "%" + Identifier.Identifier;
        }
    }

    [SubstituteToken("FieldList")]
    public class NFieldList : SubstituteNode, ITypeErrorSource
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

        public IEnumerable<ParserException> FindTypeErrors(Scope scope)
        {
            return Children.SelectMany(x => (x is ITypeErrorSource)
                   ? ((ITypeErrorSource) x).FindTypeErrors(scope)
                   : new ParserException[0]);
        }

        public override void GenerateCode(GenerationContext ctx)
        {
            foreach (var ident in Identifiers.IdentDefs) {
                ctx += Type;
            }
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

        public override void GenerateCode(GenerationContext ctx)
        {
            ctx += "{";
            if (SuperRecord != null) {
                ctx += SuperRecord;
                if (FieldLists.Count() > 0) ctx += ", ";
            }
            foreach (var fl in FieldLists) {
                if (fl != FieldLists.First()) ctx += ", ";
                ctx += fl;
            }
            ctx += "}";
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
