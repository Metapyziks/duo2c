using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DUO2C.Semantics;

namespace DUO2C.Nodes.Oberon2
{
    public abstract class TypeDefinition : SubstituteNode
    {
        public abstract OberonType Type { get; }

        public TypeDefinition(ParseNode original, bool leaf, bool hasPayload = true)
            : base(original, leaf, hasPayload) { }
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
    }

    [SubstituteToken("FieldList")]
    public class NFieldList : SubstituteNode
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
    }

    [SubstituteToken("RecordType")]
    public class NRecordType : TypeDefinition
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
