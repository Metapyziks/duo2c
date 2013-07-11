using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUO2C.Semantics
{
    public abstract class OberonType { }

    public abstract class StaticType : OberonType { }

    public class SetType : OberonType
    {
        public static readonly SetType Default = new SetType();

        public override string ToString()
        {
            return "SET";
        }
    }

    public class BooleanType : StaticType
    {
        public static readonly BooleanType Default = new BooleanType();

        public override string ToString()
        {
            return "BOOLEAN";
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
        public static readonly new IntegerType Default = new IntegerType(IntegerRange.Integer);

        public static IntegerType Largest(IntegerType a, IntegerType b)
        {
            return a.Range >= b.Range ? a : b;
        }

        public IntegerRange Range { get; private set; }

        public IntegerType(IntegerRange range)
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
        public static readonly new RealType Default = new RealType(RealRange.Real);

        public static RealType Largest(RealType a, RealType b)
        {
            return a.Range >= b.Range ? a : b;
        }

        public RealRange Range { get; private set; }

        public RealType(RealRange range)
        {
            Range = range;
        }

        public override string ToString()
        {
            return Range.ToString().ToUpper();
        }
    }
}
