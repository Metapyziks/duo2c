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
        public abstract bool CanTestEquality(OberonType other);
        public abstract bool CanCompare(OberonType other);
    }

    public abstract class StaticType : OberonType { }

    public class UnresolvedType : OberonType
    {
        public String Identifier { get; private set; }
        public String Module { get; private set; }

        public UnresolvedType(String identifier, String module = null)
        {
            Identifier = identifier;
            Module = module;
        }

        public override bool CanCompare(OberonType other)
        {
            throw new NotImplementedException();
        }

        public override bool CanTestEquality(OberonType other)
        {
            throw new NotImplementedException();
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

        public PointerType(OberonType resolvedType)
        {
            ResolvedType = resolvedType;
        }

        public override bool CanTestEquality(OberonType other)
        {
            if (other is PointerType) {
                var type = ((PointerType) other).ResolvedType;
                return type == null || type.CanTestEquality(ResolvedType);
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

    public enum AccessModifier : byte
    {
        Private = 0,
        ReadOnly = 1,
        Public = 2
    }

    public class RecordType : OberonType
    {
        private class Field
        {
            public String Identifier { get; private set; }
            public AccessModifier Visibility { get; private set; }
            public OberonType Type { get; private set; }

            public Field(String identifier, AccessModifier visibility, OberonType type)
            {
                Identifier = identifier;
                Visibility = visibility;
                Type = type;
            }

            public override string ToString()
            {
                return String.Format("{0} : {1}", Identifier, Type);
            }
        }

        private NNamedType _superRecordName;
        private Dictionary<String, Field> _fields;

        public RecordType(NRecordType node)
        {
            _superRecordName = node.SuperRecord;

            _fields = node.Fields.Select(x =>
                new Field(x.Key.Identifier, x.Key.Visibility, x.Value.Type)
            ).ToDictionary(x => x.Identifier);
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
            var sb = new StringBuilder();
            sb.Append("RECORD ");
            if (_superRecordName != null) sb.AppendFormat("({0}) ", _superRecordName.Identifier);
            foreach (var field in _fields) {
                sb.AppendFormat("{0}; ", field.Value);
            }
            sb.Append("END");
            return sb.ToString();
        }
    }

    public class ProcedureType : OberonType
    {
        private class Parameter
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

        public OberonType ReturnType { get; private set; }
        private Parameter[] _params;

        public ProcedureType(NFormalPars paras)
        {
            if (paras != null) {
                ReturnType = paras.ReturnType != null ? paras.ReturnType.Type : null;
                _params = paras.FPSections.SelectMany(x => x.Identifiers.Select(y =>
                    new Parameter(x.ByReference, y, x.Type.Type))).ToArray();
            } else {
                ReturnType = null;
                _params = new Parameter[0];
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
            String paramStr = String.Join(", ", _params.Select(x => x.Identifier + " : " + x.Type));
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
            if (ElementType is CharType) {
                if (other is CharType) {
                    return true;
                } else if (other is ArrayType) {
                    var otherArray = (ArrayType) other;
                    return otherArray.ElementType is CharType;
                }
            }

            return false;
        }
    }

    public class SetType : OberonType
    {
        public static readonly SetType Default = new SetType();

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

    public class BooleanType : StaticType
    {
        public static readonly BooleanType Default = new BooleanType();

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

    public class NumericType : StaticType
    {
        public static readonly NumericType Default = new NumericType();

        public override string ToString()
        {
            return "NUMERIC";
        }

        public static NumericType Largest(NumericType a, NumericType b)
        {
            if (a is RealType && b is RealType) {
                return RealType.Largest((RealType) a, (RealType) b);
            } else if (a is IntegerType && b is IntegerType) {
                return IntegerType.Largest((IntegerType) a, (IntegerType) b);
            } else if (a is RealType) {
                return a;
            } else {
                return b;
            }
        }

        public override bool CanTestEquality(OberonType other)
        {
            return other is NumericType;
        }

        public override bool CanCompare(OberonType other)
        {
            return other is NumericType;
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

        public static IntegerType Largest(IntegerType a, IntegerType b)
        {
            return a.Range >= b.Range ? a : b;
        }

        public IntegerRange Range { get; private set; }

        private IntegerType(IntegerRange range)
        {
            Range = range;
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

        public static RealType Largest(RealType a, RealType b)
        {
            return a.Range >= b.Range ? a : b;
        }

        public RealRange Range { get; private set; }

        private RealType(RealRange range)
        {
            Range = range;
        }

        public override string ToString()
        {
            return Range.ToString().ToUpper();
        }
    }
}
