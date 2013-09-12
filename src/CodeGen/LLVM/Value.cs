using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DUO2C.Nodes.Oberon2;
using DUO2C.Semantics;

namespace DUO2C.CodeGen.LLVM
{
    public static partial class IntermediaryCodeGenerator
    {
        public interface IComplexWrite
        {
            GenerationContext Write(GenerationContext ctx);
        }

        public abstract class Value
        {
            public override bool Equals(object obj)
            {
                if (obj == null) return false;

                if (this is TempIdent || obj is TempIdent) {
                    return this == obj;
                } else {
                    return GetType() == obj.GetType() && ToString().Equals(obj.ToString());
                }
            }

            public override int GetHashCode()
            {
                return GetType().GetHashCode() ^ ToString().GetHashCode();
            }
        }

        public class TypeIdent : Value
        {
            private String _ident;

            public TypeIdent(String ident)
            {
                _ident = ident;
            }

            public TypeIdent(String ident, String module)
            {
                _ident = String.Format("{0}.{1}", module, ident);
            }

            public override string ToString()
            {
                return String.Format("%{0}", _ident);
            }
        }

        public class GlobalIdent : Value
        {
            public enum Options
            {
                Default = 1,
                NoUnwind = 2,
                NoAlias = 4,
                AlwaysInline = 8
            }

            private String _ident;

            public bool IsPublic { get; private set; }
            public Options OptionTags { get; private set; }

            public GlobalIdent(String ident, bool isPublic, Options options = Options.Default)
            {
                _ident = ident;
                IsPublic = isPublic;
                OptionTags = options;
            }

            public override string ToString()
            {
                return String.Format("@{0}", _ident);
            }
        }

        public class Literal : Value
        {
            public static readonly Literal Undef = new Literal("undef");

            public static Literal GetDefault(OberonType type)
            {
                if (type.IsRecord) {
                    return new RecordLiteral(type.As<RecordType>());
                } else if (type.IsPointer) {
                    return new Literal("null");
                } else {
                    return new Literal("zeroinitializer");
                }
            }

            String _str;

            public long IntegerValue
            {
                get
                {
                    return long.Parse(_str);
                }
            }

            public Literal(byte[] bytes)
            {
                _str = String.Format("c\"{0}\"", bytes.Aggregate(String.Empty,
                    (s, x) => String.Format("{0}{1}", s, 32 <= x && x < 127 ? ((char) x).ToString() : "\\" + x.ToString("X2"))));
            }

            public Literal(String str)
            {
                _str = str;
            }

            public Literal(long integer)
            {
                _str = integer.ToString();
            }

            public Literal(double real)
            {
                _str = real.ToString();
            }

            public Literal(NNumber num)
            {
                _str = num.Inner.String;
            }

            public override string ToString()
            {
                return _str;
            }
        }

        public class OpenArrayLiteral : Literal, IComplexWrite
        {
            public OberonType ElementType { get; private set; }
            public Value Length { get; private set; }

            public OpenArrayLiteral(ArrayType type)
                : base(String.Empty)
            {
                ElementType = type.ElementType;
                Length = new Literal(type.Length);
            }

            public OpenArrayLiteral(OberonType elementType, Value length)
                : base(String.Empty)
            {
                ElementType = elementType;
                Length = length;
            }

            public GenerationContext Write(GenerationContext ctx)
            {
                var elementPtrType = new PointerType(ElementType);

                ctx.Write("{").EndArguments();
                ctx.Argument(IntegerType.Integer, Length);
                ctx.Argument(elementPtrType, Literal.GetDefault(elementPtrType));
                return ctx.Write("}");
            }
        }

        public class RecordLiteral : Literal, IComplexWrite
        {
            public RecordType Type { get; private set; }

            public RecordLiteral(RecordType type)
                : base(String.Empty)
            {
                Type = type;
            }

            public GenerationContext Write(GenerationContext ctx)
            {
                ctx.Write("{");
                ctx.Argument(new PointerType(IntegerType.Byte),
                    new BitCast(true, new PointerType(GetRecordTableType(Type)), GetRecordTableIdent(Type),
                        new PointerType(IntegerType.Byte)));

                int count = Type.Fields.Count();
                for (int i = 0; i < count; ++i) {
                    var type = Type.GetFieldType(i);
                    ctx.Argument(type, GetDefault(type));
                }

                return ctx.Write("}");
            }
        }

        public class StringLiteral : Value, IComplexWrite
        {
            public String String { get; private set; }

            public GlobalStringIdent Identifier
            {
                get { return GetStringIdent(String); }
            }

            public OberonType ConstType
            {
                get { return GetStringType(String); }
            }

            public int Length

            {
                get { return GetStringLength(String); }
            }

            public StringLiteral(String str)
            {
                String = str;
            }

            public GenerationContext Write(GenerationContext ctx)
            {
                ctx.Write("{").EndArguments();
                ctx.Argument(IntegerType.Integer, new Literal(Length.ToString()));
                ctx.Argument(new PointerType(CharType.Default),
                    new ElementPointer(true, new PointerType(new StaticArrayType(CharType.Default,
                        Length)), Identifier, 0, 0));
                ctx.EndArguments();
                return ctx.Write("}");
            }
        }

        public class VectorLiteral : Literal, IComplexWrite
        {
            public OberonType ElementType { get; private set; }
            public IEnumerable<Value> Elements { get; private set; }

            public VectorLiteral(OberonType elementType, IEnumerable<Value> vals)
                : base(String.Empty)
            {
                ElementType = elementType;
                Elements = vals;
            }
            
            public GenerationContext Write(GenerationContext ctx)
            {
                ctx.Write("<").EndArguments();

                foreach (var elem in Elements) {
                    ctx.Argument(ElementType, elem);    
                }

                return ctx.Write(">").EndArguments();
            }
        }

        public class QualIdent : Value
        {
            NQualIdent _ident;
            Scope _scopeAtDecl;

            public String Identifier
            {
                get { return _ident.Identifier; }
            }

            public virtual String Module
            {
                get {
                    if (_ident.Module != null) return _ident.Module;
                    if (Visibility != AccessModifier.Private) return _module.Identifier;
                    return null;
                }
            }

            public virtual Declaration Declaration
            {
                get { return _scopeAtDecl.GetSymbolDecl(_ident.Identifier, _ident.Module); }
            }

            public AccessModifier Visibility
            {
                get {
                    return Declaration != null ? Declaration.Visibility : AccessModifier.Private;
                }
            }

            public DeclarationType DeclarationType
            {
                get {
                    return Declaration.DeclarationType;
                }
            }

            public QualIdent(String ident, String module = null)
                : this(new NQualIdent(ident, module)) { }

            public QualIdent(NQualIdent ident)
            {
                _ident = ident;
                _scopeAtDecl = _scope.GetDeclaringScope(ident.Identifier, ident.Module) ?? _scope;
            }

            public override string ToString()
            {
                String module = Module;
                String ident = module != null ? String.Format("{0}.{1}", module, Identifier) : Identifier;
                
                if (_scopeAtDecl.Parent is RootScope && _scopeAtDecl.GetType(Identifier, module) == null) {
                    return String.Format("@{0}", ident);
                } else {
                    return String.Format("%{0}", ident);
                }
            }
        }

        public class BoundProcedureIdent : Value
        {
            Scope _scopeAtDecl;

            public RecordType ReceiverType { get; private set; }
            public String Identifier { get; private set; }

            public BoundProcedureIdent(RecordType receiver, String ident)
            {
                ReceiverType = receiver;
                Identifier = ident;

                _scopeAtDecl = _scope;
            }

            public override string ToString()
            {
                return String.Format("@{0}.{1}.{2}", _scopeAtDecl.GetDeclaringScope(ReceiverType).CurrentModule.Identifier,
                    _scopeAtDecl.GetTypes(true).First(x => x.Value.Type == ReceiverType).Key, Identifier);
            }
        }

        public class RecordTableConst : Value, IComplexWrite
        {
            String _ident;
            RecordType _type;

            public RecordTableConst(String ident, RecordType type)
            {
                _ident = ident;
                _type = type;
            }

            public GenerationContext Write(GenerationContext ctx)
            {
                ctx.Write("[").EndOperation();
                ctx = ctx.Enter();
                ctx.Argument(new PointerType(IntegerType.Byte), new ElementPointer(true,
                    GetStringType(_ident), GetStringIdent(_ident), 0, 0));
                ctx.Write(",").EndOperation();

                if (_type.SuperRecordName == null) {
                    ctx.Argument(new PointerType(IntegerType.Byte), new Literal("null"));
                } else {
                    ctx.Argument(new PointerType(IntegerType.Byte), new BitCast(true,
                        new PointerType(GetRecordTableType(_type.SuperRecord)),
                        GetRecordTableIdent(_type.SuperRecord), new PointerType(IntegerType.Byte)));
                }

                var procs = _type.Procedures;
                if (procs.Count() > 0) ctx.Write(",");

                ctx.EndOperation();
                foreach (var proc in procs) {
                    ctx.Argument(new PointerType(IntegerType.Byte),
                        new BitCast(true, new PointerType(proc.Value.Type),
                        new BoundProcedureIdent(_type.GetProcedureDefiner(proc.Key), proc.Key),
                        new PointerType(IntegerType.Byte)));
                    if (proc.Key != procs.Last().Key) ctx.Write(",");
                    ctx.EndOperation();
                }
                return ctx.Leave().Write("]").EndArguments();
            }
        }

        public class TempIdent : Value
        {
            static int _sLast = 0;

            public static readonly TempIdent Zero = new TempIdent();

            public static void Reset()
            {
                _blockLabel = null;
                _sLast = 0;
            }

            int _id;
            List<TempIdent> _preds;

            public IEnumerable<TempIdent> Predecessors
            {
                get { return _preds; }
            }

            public int ID
            {
                get
                {
                    ResolveID();
                    return _id;
                }
            }

            public TempIdent()
            {
                _id = 0;
                _preds = new List<TempIdent>();
            }

            public void AddPredecessor(TempIdent pred)
            {
                _preds.Add(pred);
            }

            public void ResolveID()
            {
                if (_id == 0 && this != Zero) {
                    _id = ++_sLast;
                }
            }

            public override string ToString()
            {
                return String.Format("%{0}", ID);
            }
        }

        public class GlobalStringIdent : Value
        {
            static int _sNext = 0;

            public static void Reset()
            {
                _sNext = 0;
            }

            int _id;

            public int ID
            {
                get
                {
                    if (_id == -1) {
                        _id = _sNext ++;
                    }
                    return _id;
                }
            }

            public GlobalStringIdent()
            {
                _id = -1;
            }

            public override string ToString()
            {
                return String.Format("@.str{0}", ID);
            }
        }

        public class RecordTableIdent : Value
        {
            public String Module { get; private set; }
            public String TypeName { get; private set; }

            public RecordTableIdent(RecordType type)
            {
                TypeName = _scope.GetTypes(true).First(x => x.Value.Type == type).Key;
                Module = _scope.GetDeclaringScope(type).CurrentModule.Identifier;
            }

            public override string ToString()
            {
                return String.Format("@{0}.{1}._vtable", Module, TypeName);
            }
        }

        public class ElementPointer : Value, IComplexWrite
        {
            public bool InBraces { get; private set; }

            public bool InBounds { get; private set; }
            public OberonType StructureType { get; private set; }
            public Value Structure { get; private set; }
            public Value[] Indices { get; private set; }

            public ElementPointer(bool inBraces, OberonType structType, Value structure, params Object[] indices)
            {
                InBraces = inBraces;

                InBounds = true;
                StructureType = structType;
                Structure = structure;
                Indices = indices.Select(x => x is Value ? (Value) x : new Literal((int) x)).ToArray();
            }

            public ElementPointer(bool inBraces, ElementPointer clone)
            {
                InBraces = inBraces;

                InBounds = clone.InBounds;
                StructureType = clone.StructureType;
                Structure = clone.Structure;
                Indices = clone.Indices;
            }

            public virtual GenerationContext Write(GenerationContext ctx)
            {
                ctx.Keyword("getelementptr");
                if (InBounds) ctx.Keyword("inbounds");
                if (InBraces) ctx.Write("(");
                ctx.Argument(StructureType, Structure);
                foreach (var ind in Indices) {
                    ctx.Argument(IntegerType.Integer, ind);
                }
                if (InBraces) ctx.Write("\t)");
                return ctx;
            }
        }

        public class InsertValue : ElementPointer
        {
            public OberonType ValueType { get; private set; }

            public Value Value { get; private set; }

            public InsertValue(bool inBraces, OberonType structType, Value structure,
                OberonType valueType, Value value, params Object[] indices)
                : base(inBraces, structType, structure, indices)
            {
                ValueType = valueType;
                Value = value;
            }

            public override GenerationContext Write(GenerationContext ctx)
            {
                ctx.Keyword("insertvalue");
                if (InBraces) ctx.Write("(");
                ctx.Argument(StructureType, Structure);
                ctx.Argument(ValueType, Value);
                foreach (var index in Indices) {
                    ctx.Argument(index);
                }
                if (InBraces) ctx.Write("\t)");
                return ctx;
            }
        }

        public class ExtractValue : ElementPointer
        {
            public ExtractValue(bool inBraces, OberonType structType, Value structure, params Object[] indices)
                : base(inBraces, structType, structure, indices) { }

            public override GenerationContext Write(GenerationContext ctx)
            {
                ctx.Keyword("extractvalue");
                if (InBraces) ctx.Write("(");
                ctx.Argument(StructureType, Structure);
                foreach (var index in Indices) {
                    ctx.Argument(index);
                }
                if (InBraces) ctx.Write("\t)");
                return ctx;
            }
        }

        public abstract class VectorElement : ElementPointer
        {
            public Value Index { get { return Indices.First(); } }

            public Value Vector { get { return Structure; } }

            public VectorType VectorType { get { return (VectorType) StructureType; } }

            public VectorElement(bool inBraces, VectorType vectorType, Value structure, Object index)
                : base(inBraces, vectorType, structure, index) { }
        }

        public class ExtractElement : VectorElement
        {
            public ExtractElement(bool inBraces, VectorType vectorType, Value structure, Object index)
                : base(inBraces, vectorType, structure, index) { }

            public override GenerationContext Write(GenerationContext ctx)
            {
                ctx.Keyword("extractelement");
                if (InBraces) ctx.Write("(");
                ctx.Argument(StructureType, Structure);
                ctx.Argument(IntegerType.Integer, Index);
                if (InBraces) ctx.Write("\t)");
                return ctx;
            }
        }

        public class InsertElement : VectorElement
        {
            public Value Value { get; private set; }

            public InsertElement(bool inBraces, VectorType vectorType, Value structure, Value value, Object index)
                : base(inBraces, vectorType, structure, index)
            {
                Value = value;
            }

            public override GenerationContext Write(GenerationContext ctx)
            {
                ctx.Keyword("insertelement");
                if (InBraces) ctx.Write("(");
                ctx.Argument(StructureType, Structure);
                ctx.Argument(VectorType.ElementType, Value);
                ctx.Argument(IntegerType.Integer, Index);
                if (InBraces) ctx.Write("\t)");
                return ctx;
            }
        }

        public class BitCast : Value, IComplexWrite
        {
            public bool InBraces { get; private set; }

            OberonType _from;
            Value _val;
            OberonType _to;

            public BitCast(bool inBraces, OberonType from, Value val, OberonType to)
            {
                InBraces = inBraces;

                _from = from;
                _val = val;
                _to = to;
            }

            public GenerationContext Write(GenerationContext ctx)
            {
                ctx.Keyword("bitcast");
                if (InBraces) ctx.Write("(");
                ctx.Argument(_from, _val).Keyword(" to").Argument(_to);
                if (InBraces) ctx.Write(")");
                return ctx;
            }
        }

        static GenerationContext Write(this GenerationContext ctx, Value val)
        {
            if (val is IComplexWrite) {
                return ((IComplexWrite) val).Write(ctx);
            } else {
                return ctx.Write(() => val.ToString());
            }
        }
    }
}
