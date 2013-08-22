using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DUO2C.Nodes.Oberon2;

namespace DUO2C.Semantics
{
    public abstract class OberonType
    {
        public static bool CanTestEquality(OberonType a, OberonType b)
        {
            return a.CanTestEquality(b) || b.CanTestEquality(a);
        }

        public static bool CanCompare(OberonType a, OberonType b)
        {
            return a.CanCompare(b) || b.CanCompare(a);
        }

        public bool IsResolved { get; private set; }

        public virtual bool IsModule { get { return false; } }
        public virtual bool IsPointer { get { return false; } }
        public virtual bool IsRecord { get { return false; } }
        public virtual bool IsArray { get { return false; } }
        public virtual bool IsVector { get { return false; } }
        public virtual bool IsProcedure { get { return false; } }

        public virtual bool IsBool { get { return false; } }
        public virtual bool IsSet { get { return false; } }
        public virtual bool IsChar { get { return false; } }
        public virtual bool IsNumeric { get { return false; } }
        public virtual bool IsInteger { get { return false; } }
        public virtual bool IsReal { get { return false; } }

        protected OberonType()
        {
            IsResolved = false;
        }
        
        public TDst As<TDst>()
            where TDst : OberonType
        {
            if (this is TDst) {
                return (TDst) this;
            } else if (this is UnresolvedType) {
                return ((UnresolvedType) this).ReferencedType.As<TDst>();
            } else {
                throw new InvalidOperationException(
                    String.Format("Cannot cast type '{0}' to '{1}'",
                        GetType().Name, typeof(TDst).Name));
            }
        }

        public OberonType Resolve(Scope scope)
        {
            if (!IsResolved) {
                IsResolved = true;
                OnResolve(scope);
            }
            return this;
        }

        protected virtual void OnResolve(Scope scope) { }

        public abstract bool CanTestEquality(OberonType other);
        public abstract bool CanCompare(OberonType other);
        
        public override bool Equals(object obj)
        {
            return obj is OberonType && CanTestEquality((OberonType) obj) && ((OberonType) obj).CanTestEquality(this);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public class ModuleType : OberonType
    {
        public NModule Module { get; private set; }
        public String Identifier
        {
            get { return Module.Identifier; }
        }

        public Scope Scope { get; private set; }

        public override bool IsModule
        {
            get { return true; }
        }

        public ModuleType(NModule module, RootScope scope)
        {
            Module = module;
            Scope = scope.CreateModuleScope(this);
        }

        public override bool CanTestEquality(OberonType other)
        {
            return false;
        }

        public override bool CanCompare(OberonType other)
        {
            return false;
        }

        public override string ToString()
        {
            return "MODULE";
        }
    }

    public class UnresolvedType : OberonType
    {
        public OberonType ReferencedType { get; private set; }

        public String Identifier { get; private set; }
        public String Module { get; private set; }

        public override bool IsModule
        {
            get { return ReferencedType != null && ReferencedType.IsModule; }
        }

        public override bool IsPointer
        {
            get { return ReferencedType != null && ReferencedType.IsPointer; }
        }

        public override bool IsRecord
        {
            get { return ReferencedType != null && ReferencedType.IsRecord; }
        }

        public override bool IsArray
        {
            get { return ReferencedType != null && ReferencedType.IsArray; }
        }

        public override bool IsVector
        {
            get { return ReferencedType != null && ReferencedType.IsVector; }
        }

        public override bool IsProcedure
        {
            get { return ReferencedType != null && ReferencedType.IsProcedure; }
        }

        public override bool IsBool
        {
            get { return ReferencedType != null && ReferencedType.IsBool; }
        }

        public override bool IsSet
        {
            get { return ReferencedType != null && ReferencedType.IsSet; }
        }

        public override bool IsChar
        {
            get { return ReferencedType != null && ReferencedType.IsChar; }
        }

        public override bool IsNumeric
        {
            get { return ReferencedType != null && ReferencedType.IsNumeric; }
        }

        public override bool IsInteger
        {
            get { return ReferencedType != null && ReferencedType.IsInteger; }
        }

        public override bool IsReal
        {
            get { return ReferencedType != null && ReferencedType.IsReal; }
        }

        public UnresolvedType(String identifier, String module = null)
        {
            Identifier = identifier;
            Module = module;

            ReferencedType = null;
        }

        protected override void OnResolve(Scope scope)
        {
            ReferencedType = scope.GetType(Identifier, Module);
        }

        public override bool CanCompare(OberonType other)
        {
            return ReferencedType.CanCompare(other);
        }

        public override bool CanTestEquality(OberonType other)
        {
            return ReferencedType == null || ReferencedType.CanTestEquality(other);
        }

        public override string ToString()
        {
            return Module == null ? Identifier : String.Format("{0}.{1}", Module, Identifier);
        }
    }

    public class PointerType : OberonType
    {
        public static readonly PointerType Null = new PointerType(null);
        public static readonly PointerType Byte = new PointerType(IntegerType.Byte);

        public OberonType ResolvedType { get; private set; }

        public override bool IsPointer
        {
            get { return true; }
        }

        public PointerType(OberonType resolvedType)
        {
            ResolvedType = resolvedType;
        }

        protected override void OnResolve(Scope scope)
        {
            if (ResolvedType != null) {
                ResolvedType.Resolve(scope);
            }
        }

        public override bool CanTestEquality(OberonType other)
        {
            if (other.IsPointer) {
                var type = other.As<PointerType>().ResolvedType;
                return ResolvedType == null || type == null || ResolvedType.CanTestEquality(type);
            } else {
                return false;
            }
        }

        public override bool CanCompare(OberonType other)
        {
            return false;
        }

        public override string ToString()
        {
            return ResolvedType == null ? "NIL" : "POINTER TO " + ResolvedType.ToString();
        }
    }

    public class RecordType : OberonType
    {
        public static readonly RecordType Base = new RecordType();

        private NRecordType _recNode;

        private NQualIdent _superRecordIdent;
        private Dictionary<String, Declaration> _fields;
        private Dictionary<String, Declaration> _procedures;

        public bool HasSuperRecord
        {
            get { return _superRecordIdent != null; }
        }

        public String SuperRecordName
        {
            get { return _superRecordIdent != null ? _superRecordIdent.String : null; }
        }

        public RecordType SuperRecord { get; private set; }

        public override bool IsRecord
        {
            get { return true; }
        }

        public IEnumerable<String> FieldNames
        {
            get { return Fields.Select(x => x.Key); }
        }

        public IEnumerable<Declaration> FieldDecls
        {
            get { return Fields.Select(x => x.Value); }
        }

        public IEnumerable<KeyValuePair<String, Declaration>> Fields
        {
            get {
                if (HasSuperRecord) {
                    return SuperRecord.Fields.Concat(_fields);
                } else {
                    return _fields;
                }
            }
        }

        public IEnumerable<String> ProcedureNames
        {
            get { return Procedures.Select(x => x.Key); }
        }

        public IEnumerable<Declaration> ProcedureDecls
        {
            get { return Procedures.Select(x => x.Value); }
        }

        public IEnumerable<KeyValuePair<String, Declaration>> Procedures
        {
            get {
                if (HasSuperRecord) {
                    var superProcedures = SuperRecord.Procedures.Select(x => _procedures.ContainsKey(x.Key)
                        ? new KeyValuePair<String, Declaration>(x.Key, _procedures[x.Key]) : x);
                    return superProcedures.Concat(_procedures.Where(x => !superProcedures.Any(y => y.Key == x.Key)));
                } else {
                    return _procedures;
                }
            }
        }

        private RecordType()
        {
            _superRecordIdent = null;
            _fields = new Dictionary<string, Declaration>();
            _procedures = new Dictionary<string,Declaration>();
        }

        public RecordType(NRecordType node)
        {
            _superRecordIdent = (node.SuperRecord != null ? node.SuperRecord.Identifier : null);
            _recNode = node;

            _procedures = new Dictionary<string, Declaration>();
        }

        public void BindProcedure(String ident, AccessModifier visibility,
            ProcedureType signature, UnresolvedType receiverType, String receiverIdent)
        {
            signature = new ProcedureType(signature.ReturnType, receiverType, receiverIdent, signature.Params);
            _procedures.Add(ident, new Declaration(signature, visibility, DeclarationType.BoundProcedure));
        }

        public RecordType GetProcedureDefiner(String ident)
        {
            if (_procedures.Any(x => x.Key == ident)) return this;
            if (HasSuperRecord) return SuperRecord.GetProcedureDefiner(ident);
            return null;
        }

        public bool HasField(String ident)
        {
            return FieldNames.Contains(ident);
        }

        public OberonType GetFieldType(int index)
        {
            var decl = GetFieldDecl(index);
            return decl != null ? decl.Type : null;
        }

        public OberonType GetFieldType(String ident)
        {
            var decl = GetFieldDecl(ident);
            return decl != null ? decl.Type : null;
        }

        public Declaration GetFieldDecl(int index)
        {
            return Fields.ElementAtOrDefault(index).Value;
        }

        public Declaration GetFieldDecl(String ident)
        {
            return Fields.FirstOrDefault(x => x.Key == ident).Value;
        }

        public int GetFieldIndex(String ident)
        {
            int i = 0;
            foreach (var field in FieldNames) {
                if (field == ident) return i; else ++i;
            }
            return -1;
        }

        public bool HasProcedure(String ident)
        {
            return ProcedureNames.Contains(ident);
        }

        public ProcedureType GetProcedureSignature(int index)
        {
            var decl = GetProcedureDecl(index);
            return decl != null ? decl.Type.As<ProcedureType>() : null;
        }

        public ProcedureType GetProcedureSignature(String ident)
        {
            var decl = GetProcedureDecl(ident);
            return decl != null ? decl.Type.As<ProcedureType>() : null;
        }

        public Declaration GetProcedureDecl(int index)
        {
            return Procedures.ElementAtOrDefault(index).Value;
        }

        public Declaration GetProcedureDecl(String ident)
        {
            return Procedures.First(x => x.Key == ident).Value;
        }

        public int GetProcedureIndex(String ident)
        {
            int i = 0;
            foreach (var proc in ProcedureNames) {
                if (proc == ident) return i; else ++i;
            }
            return -1;
        }

        protected override void OnResolve(Scope scope)
        {
            _fields = _recNode.Fields.Where(x => x.Value.FindTypeErrors(scope).Count() == 0)
                .Select(x => new KeyValuePair<String, Declaration>(x.Key.Identifier,
                    new Declaration(x.Value.Type, x.Key.Visibility, DeclarationType.Field)))
                .ToDictionary(x => x.Key, x => x.Value);

            if (HasSuperRecord) {
                SuperRecord = scope.GetType(_superRecordIdent.Identifier, _superRecordIdent.Module).As<RecordType>();
            } else if (this != Base) {
                SuperRecord = Base;
            }
        }

        public override bool CanTestEquality(OberonType other)
        {
            if (!other.IsRecord) return false;

            var rec = other.As<RecordType>();
            if (rec == this || (rec.SuperRecord != null && CanTestEquality(rec.SuperRecord))) return true;
            return false;
        }

        public override bool CanCompare(OberonType other)
        {
            return false;
        }

        public override string ToString()
        {
            if (this == Base) return "RECORD";

            var sb = new StringBuilder();
            sb.Append("RECORD ");
            if (HasSuperRecord) sb.AppendFormat("({0}) ", _superRecordIdent.Identifier);
            foreach (var field in _fields) {
                sb.AppendFormat("{0}; ", field.Value);
            }
            sb.Append("END");
            return sb.ToString();
        }
    }

    public class Parameter
    {
        public bool ByReference { get; private set; }
        public String Identifier { get; private set; }
        public OberonType Type { get; private set; }

        public Parameter(bool byRef, String ident, OberonType type)
        {
            ByReference = byRef;
            Identifier = ident;
            Type = type;
        }
    }

    public class ProcedureType : OberonType
    {
        public OberonType ReturnType { get; private set; }
        public Parameter[] Params { get; private set; }
        public Parameter[] ParamsWithReceiver { get; private set; }

        public UnresolvedType ReceiverType { get; private set; }
        public String ReceiverIdent { get; private set; }

        public override bool IsProcedure
        {
            get { return true; }
        }

        public ProcedureType(NFormalPars paras)
        {
            if (paras != null) {
                ReturnType = paras.ReturnType != null ? paras.ReturnType.Type : null;
                Params = paras.FPSections.SelectMany(x => x.Identifiers.Select(y =>
                    new Parameter(x.ByReference, y, x.Type.Type))).ToArray();
            } else {
                ReturnType = null;
                Params = new Parameter[0];
            }
            ParamsWithReceiver = Params;
        }

        public ProcedureType(OberonType returnType, params Parameter[] args)
        {
            ReturnType = returnType;
            ReceiverType = null;
            ReceiverIdent = null;
            Params = args;
            ParamsWithReceiver = Params;
        }

        public ProcedureType(OberonType returnType, UnresolvedType receiverType, String receiverIdent, params Parameter[] args)
        {
            ReturnType = returnType;
            ReceiverType = receiverType;
            ReceiverIdent = receiverIdent;
            Params = args;
            ParamsWithReceiver = new Parameter[] { new Parameter(true, receiverIdent, receiverType) }
                .Concat(Params).ToArray();
        }

        public IEnumerable<CompilerException> MatchParameters(NInvocation invoc, Scope scope)
        {
            var args = invoc.Args != null ? invoc.Args.Expressions.ToArray() : new NExpr[0];
            if (args.Length != Params.Length) {
                yield return new CompilerException(ParserError.Semantics, String.Format("Argument count mismatch, "
                    + "expected {0}, received {1}", Params.Length, args.Count()), invoc.StartIndex, 0);
            } else {
                for (int i = 0; i < Params.Length; ++i) {
                    var param = Params[i];
                    var arg = args[i];

                    if (arg.FindTypeErrors(scope).Count() == 0) { 
                        var argType = arg.GetFinalType(scope).Resolve(scope);

                        if (!param.Type.Resolve(scope).CanTestEquality(argType)) {
                            yield return new TypeMismatchException(param.Type, argType, arg);
                        }
                    }
                }
            }
        }

        public override bool CanTestEquality(OberonType other)
        {
            return other.IsProcedure;
        }

        public override bool CanCompare(OberonType other)
        {
            return false;
        }

        public override string ToString()
        {
            String paramStr = String.Join("; ", Params.Select(x => x.Identifier + " : " + x.Type));
            if (ReturnType != null) {
                return String.Format("PROCEDURE ({0}) : {1}", paramStr, ReturnType);
            } else {
                return String.Format("PROCEDURE ({0})", paramStr);
            }
        }
    }

    public class ExternalProcedureType : ProcedureType
    {
        public String ExternalSymbol { get; private set; }
        public bool IsImported { get; private set; }

        public ExternalProcedureType(NFormalPars paras, String externalSymbol, bool isImported)
            : base(paras)
        {
            ExternalSymbol = externalSymbol;
            IsImported = isImported;
        }

        public override string ToString()
        {
            return "EXTERNAL " + base.ToString();
        }
    }

    public abstract class IndexableType : OberonType
    {
        public OberonType ElementType { get; private set; }
        public int Length { get; private set; }

        public IndexableType(OberonType elementType, int length = -1)
        {
            ElementType = elementType;
            Length = length;
        }
    }

    public class ArrayType : IndexableType
    {
        public override bool IsArray
        {
            get { return true; }
        }

        public ArrayType(OberonType elementType, int length = -1)
            : base(elementType, length) { }

        public override string ToString()
        {
            return String.Format("ARRAY {0}OF {1}", (Length > -1 ? Length + " " : ""), ElementType);
        }

        public override bool CanTestEquality(OberonType other)
        {
            return CanCompare(other);
        }

        public override bool CanCompare(OberonType other)
        {
            if (ElementType.IsChar && other.IsArray) {
                var otherArray = other.As<ArrayType>();
                return otherArray.ElementType.IsChar;
            }

            return false;
        }
    }

    public class VectorType : IndexableType
    {
        public override bool IsVector
        {
            get { return true; }
        }

        public VectorType(OberonType elementType, int length)
            : base(elementType, length) { }

        public override string ToString()
        {
            return String.Format("VECTOR {0} OF {1}", Length, ElementType);
        }

        public override bool CanTestEquality(OberonType other)
        {
            return CanCompare(other);
        }

        public override bool CanCompare(OberonType other)
        {
            if (other.IsVector) {
                var vec = other.As<VectorType>();
                return ElementType.CanCompare(vec.ElementType) && Length == vec.Length;
            }

            return ElementType.CanCompare(other);
        }
    }

    public class SetType : OberonType
    {
        public static readonly SetType Default = new SetType();

        public override bool IsSet { get { return true; } }

        public override string ToString()
        {
            return "SET";
        }

        public override bool CanTestEquality(OberonType other)
        {
            return other is SetType;
        }

        public override bool CanCompare(OberonType other)
        {
            return false;
        }
    }

    public class BooleanType : OberonType
    {
        public static readonly BooleanType Default = new BooleanType();

        public override bool IsBool { get { return true; } }

        public override string ToString()
        {
            return "BOOLEAN";
        }

        public override bool CanTestEquality(OberonType other)
        {
            return other is BooleanType;
        }

        public override bool CanCompare(OberonType other)
        {
            return false;
        }
    }

    public class CharType : OberonType
    {
        public static readonly CharType Default = new CharType();

        public override bool IsChar { get { return true; } }

        public override string ToString()
        {
            return "CHAR";
        }

        public override bool CanTestEquality(OberonType other)
        {
            return CanCompare(other);
        }

        public override bool CanCompare(OberonType other)
        {
            return other.IsChar || (other.IsArray && other.As<ArrayType>().ElementType.IsChar);
        }
    }

    public class NumericType : OberonType
    {
        public static readonly NumericType Default = new NumericType();

        public override bool IsNumeric { get { return true; } }

        public override string ToString()
        {
            return "NUMERIC";
        }

        public static NumericType Largest(NumericType a, NumericType b)
        {
            if (a.IsReal && b.IsReal) {
                return RealType.Largest(a.As<RealType>(), b.As<RealType>());
            } else if (a.IsInteger && b.IsInteger) {
                return IntegerType.Largest(a.As<IntegerType>(), b.As<IntegerType>());
            } else if (a.IsReal) {
                return a.As<RealType>();
            } else {
                return b.As<RealType>();
            }
        }

        public override bool CanTestEquality(OberonType other)
        {
            return other.IsNumeric;
        }

        public override bool CanCompare(OberonType other)
        {
            return other.IsNumeric;
        }
    }

    /// <summary>
    /// An enumeration of all integer types in Oberon-2
    /// </summary>
    public enum IntegerRange : byte
    {
        Byte = 1,
        ShortInt = 2,
        Integer = 4,
        LongInt = 8
    }

    public class IntegerType : NumericType
    {
        public static readonly IntegerType LongInt = new IntegerType(IntegerRange.LongInt);
        public static readonly IntegerType Integer = new IntegerType(IntegerRange.Integer);
        public static readonly IntegerType ShortInt = new IntegerType(IntegerRange.ShortInt);
        public static readonly IntegerType Byte = new IntegerType(IntegerRange.Byte);

        public override bool IsInteger { get { return true; } }

        public static IntegerType Largest(IntegerType a, IntegerType b)
        {
            return a.Range >= b.Range ? a : b;
        }

        public IntegerRange Range { get; private set; }

        private IntegerType(IntegerRange range)
        {
            Range = range;
        }

        public override bool CanTestEquality(OberonType other)
        {
            return other.IsInteger && other.As<IntegerType>().Range <= Range;
        }

        public override string ToString()
        {
            return Range.ToString().ToUpper();
        }
    }

    /// <summary>
    /// An enumeration of all real number types in Oberon-2
    /// </summary>
    public enum RealRange : byte
    {
        Real = 4,
        LongReal = 8
    }

    public class RealType : NumericType
    {
        public static readonly RealType LongReal = new RealType(RealRange.LongReal);
        public static readonly RealType Real = new RealType(RealRange.Real);

        public override bool IsReal { get { return true; } }

        public static RealType Largest(RealType a, RealType b)
        {
            return a.Range >= b.Range ? a : b;
        }

        public RealRange Range { get; private set; }

        private RealType(RealRange range)
        {
            Range = range;
        }

        public override bool CanTestEquality(OberonType other)
        {
            return other.IsInteger || (other.IsReal && other.As<RealType>().Range <= Range);
        }

        public override string ToString()
        {
            return Range.ToString().ToUpper();
        }
    }

    class VoidType : OberonType
    {
        public static readonly VoidType Default = new VoidType();

        public override bool CanCompare(OberonType other)
        {
            throw new NotImplementedException();
        }

        public override bool CanTestEquality(OberonType other)
        {
            throw new NotImplementedException();
        }
    }

    class VarArgsType : OberonType
    {
        public static readonly VarArgsType Default = new VarArgsType();

        public override bool CanCompare(OberonType other)
        {
            throw new NotImplementedException();
        }

        public override bool CanTestEquality(OberonType other)
        {
            throw new NotImplementedException();
        }
    }
}
