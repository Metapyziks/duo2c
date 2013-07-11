using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUO2C.Semantics
{
    public abstract class OberonType
    {
        protected abstract bool IsEqual(OberonType t);

        public override bool Equals(object obj)
        {
            return obj.GetType() == GetType() && IsEqual((OberonType) obj);
        }
    }

    public abstract class StaticType : OberonType { }

    public abstract class NumericType : StaticType { }

    /// <summary>
    /// An enumeration of all integer types in Oberon-2
    /// </summary>
    public enum IntegerRange : byte
    {
        BYTE = 1,
        SHORTINT = 2,
        INTEGER = 4,
        LONGINT = 8
    }

    public class IntegerType : NumericType
    {
        public IntegerRange Range { get; private set; }

        public IntegerType(IntegerRange range)
        {
            Range = range;
        }

        protected override bool IsEqual(OberonType t)
        {
            return ((IntegerType) t).Range == Range;
        }
    }

    /// <summary>
    /// An enumeration of all real number types in Oberon-2
    /// </summary>
    public enum RealRange : byte
    {
        REAL = 4,
        LONGREAL = 8
    }

    public class RealType : NumericType
    {
        public RealRange Range { get; private set; }

        public RealType(RealRange range)
        {
            Range = range;
        }

        protected override bool IsEqual(OberonType t)
        {
            return ((RealType) t).Range == Range;
        }
    }
}
