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
            if (this is UnresolvedType) {
                return (TDst) ((UnresolvedType) this).ReferencedType;
            } else {
                return (TDst) (Object) this;
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
        public String Identifier { get; private set; }

        public Scope Scope { get; private set; }

        public override bool IsModule
        {
            get { return true; }
        }

        public ModuleType(String identifier, RootScope scope)
        {
            Identifier = identifier;

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
            return ReferencedType.CanTestEquality(other);
        }

        public override string ToString()
        {
            return Module == null ? Identifier : String.Format("{0}.{1}", Module, Identifier);
        }
    }

    public class PointerType : OberonType
    {
        public static readonly PointerType NilPointer = new PointerType(null);

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

        private NQualIdent _superRecordIdent;
        private Dictionary<String, Declaration> _fields;

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
            get { return _fields.Keys; }
        }

        public IEnumerable<Declaration> FieldDecls
        {
            get { return _fields.Values; }
        }

        public IEnumerable<KeyValuePair<String, Declaration>> Fields
        {
            get { return _fields; }
        }

        public IEnumerable<KeyValuePair<String, Declaration>> Procedures
        {
            get { 
                var procs = _fields.Where(x => x.Value.Type.IsProcedure);
                if (SuperRecord != null) {
                    procs = SuperRecord.Procedures.Where(x => !procs.Any(y => y.Key == x.Key)).Concat(procs);
                }
                return procs;
            }
        }

        private RecordType()
        {
            _superRecordIdent = null;
            _fields = new Dictionary<string, Declaration>();
        }

        public RecordType(NRecordType node)
        {
            _superRecordIdent = (node.SuperRecord != null ? node.SuperRecord.Identifier : null);

            _fields = node.Fields.Select(x =>
                new KeyValuePair<String, Declaration>(x.Key.Identifier,
                    new Declaration(x.Value.Type, x.Key.Visibility, DeclarationType.Field))
            ).ToDictionary(x => x.Key, x => x.Value);
        }

        public void BindProcedure(String ident, AccessModifier visibility, ProcedureType signature)
        {
            _fields.Add(ident, new Declaration(signature, visibility, DeclarationType.BoundProcedure));
        }

        public bool HasField(String ident)
        {
            return _fields.ContainsKey(ident) || (SuperRecord != null && SuperRecord.HasField(ident));
        }

        public OberonType GetFieldType(String ident)
        {
            return _fields.ContainsKey(ident) ? _fields[ident].Type
                : SuperRecord != null ? SuperRecord.GetFieldType(ident) : null;
        }

        public Declaration GetFieldDecl(String ident)
        {
            return _fields.ContainsKey(ident) ? _fields[ident]
                : SuperRecord != null ? SuperRecord.GetFieldDecl(ident) : null;
        }

        protected override void OnResolve(Scope scope)
        {
            if (_superRecordIdent != null) {
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
            if (_superRecordIdent != null) sb.AppendFormat("({0}) ", _superRecordIdent.Identifier);
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
        }

        public ProcedureType(OberonType returnType, params Parameter[] args)
        {
            ReturnType = returnType;
            Params = args;
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
            return other is ProcedureType;
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

    public class ArrayType : OberonType
    {
        public OberonType ElementType { get; private set; }
        public int Length { get; private set; }

        public override bool IsArray
        {
            get { return true; }
        }

        public ArrayType(OberonType elementType, int length = -1)
        {
            ElementType = elementType;
            Length = length;
        }

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
            if (ElementType.IsChar) {
                if (other.IsChar) {
                    return true;
                } else if (other.IsArray) {
                    var otherArray = other.As<ArrayType>();
                    return otherArray.ElementType.IsChar;
                }
            }

            return false;
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
            return other is CharType || (other is ArrayType && ((ArrayType) other).ElementType is CharType);
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
}
